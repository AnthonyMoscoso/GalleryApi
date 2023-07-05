using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class DataTable<T>
    {
        public DataTable() 
        { 
            Items = new HashSet<T>();
        }
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
   
    }
}
