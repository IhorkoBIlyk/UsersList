using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Data
{
    public interface IUnitOfWork : System.IDisposable
    {
        TRepository GetRepository<TRepository>();

        ApplicationContext GetApplicationContext();

        void RegisterRepositoriesFromAssembly(System.Reflection.Assembly assembly);
        void RegisterRepository<TInterface, TRepository>()
            where TRepository : TInterface;

        System.Threading.Tasks.Task Save();
    }
}
