using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Web.Data.Repositories;

namespace Web.Data
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly ApplicationContext _applicationContext;
        private IDictionary<Type, object> _repositories;

        public UnitOfWork(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _repositories = new ConcurrentDictionary<Type, object>();
        }

        public TRepository GetRepository<TRepository>()
        {
            Type key = typeof(TRepository);

            if (!_repositories.ContainsKey(key))
                throw new ArgumentNullException("Current repository has not been registered. Verify that interface and implementation both exist.");

            return (TRepository)_repositories[key];
        }

        public ApplicationContext GetApplicationContext()
        {
            return _applicationContext;
        }

        public void RegisterRepositoriesFromAssembly(Assembly assembly)
        {
            IDictionary<Type, object> repositories = ScanAssembly(assembly);

            _repositories = new ConcurrentDictionary<Type, object>(repositories);
        }

        public void RegisterRepository<TInterface, TRepository>() where TRepository : TInterface
        {
            _repositories[typeof(TInterface)] = Activator.CreateInstance(typeof(TRepository), new object[] { this._applicationContext });
        }

        #region LoadRepositories
        private IDictionary<Type, object> ScanAssembly(Assembly assembly)
        {
            IEnumerable<Type> interfaces = LoadRepositoriesInterfaces();
            IEnumerable<Type> implementation = LoadRepositoriesImplementation(assembly);
            IDictionary<Type, object> repositories = JoinAbstractionAndImplementation(interfaces, implementation);

            return repositories;
        }

        private IEnumerable<Type> LoadRepositoriesInterfaces()
        {
            return
                from t in Assembly.GetExecutingAssembly().GetTypes()
                from ti in t.GetInterfaces()
                where t.IsInterface && ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IGenericRepository<,>)
                select t;
        }

        private IEnumerable<Type> LoadRepositoriesImplementation(Assembly assembly)
        {
            return
                from t in assembly.GetTypes()
                from ti in t.GetInterfaces()
                where t.IsClass && !t.IsAbstract && ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IGenericRepository<,>)
                select t;
        }

        private IDictionary<Type, object> JoinAbstractionAndImplementation(IEnumerable<Type> abstraction, IEnumerable<Type> implementation)
        {
            return
                (from impl in implementation
                 from i in impl.GetInterfaces()
                 join abst in abstraction on i equals abst
                 where abst.IsAssignableFrom(impl)
                 select new { abst, impl })
                .ToDictionary(k => k.abst, v => Activator.CreateInstance(v.impl, new object[] { this._applicationContext }));
        }
        #endregion

        public virtual async Task Save()
        {
            await _applicationContext.SaveChangesAsync();
        }

        #region Dispose
        private bool _disposed = false;

        //todo: find out whether dispose code is correct
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repositories.Clear();
                    _applicationContext.Dispose();
                }
            }
            _disposed = true;
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
