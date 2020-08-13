using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data;

namespace Web.Models
{
    public class User : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
