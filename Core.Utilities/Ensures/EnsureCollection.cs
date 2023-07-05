using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Ensures
{
    public class EnsureCollection<T>
    {
        private readonly ICollection<T>? _argument;
        private readonly string _argumentName;
        public EnsureCollection(ICollection<T>? argument, string argumentName = "")
        {
            _argument = argument;
            _argumentName = string.IsNullOrWhiteSpace(argumentName) ? nameof(_argument) : argumentName;
        }
        /// <summary>
        /// Ensure thar collection no is empty or null
        /// </summary>
        /// <param name="mesage">error message</param>
        /// <exception cref="ArgumentNullException">type of argument that is thrower</exception>
        public void NotNullOrEmpty(string mesage = "")
        {
            if (_argument!= null && _argument.Any())
            {
                mesage = string.IsNullOrWhiteSpace(mesage) ? _argumentName : mesage;
                throw new ArgumentNullException(mesage);
            }
           
        }
    }
}
