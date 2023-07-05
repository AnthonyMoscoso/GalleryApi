using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Ensures
{
    public class EnsureObject
    {
        private readonly object? _argument;
        private readonly string _argumentName;
        public EnsureObject(object? argument, string argumentName = "")
        {
            _argument = argument;
            _argumentName = string.IsNullOrWhiteSpace(argumentName) ? nameof(_argument) : argumentName;
        }
        /// <summary>
        /// Ensure that Argument not null
        /// </summary>
        /// <param name="mesage">Error message</param>
        /// <exception cref="ArgumentNullException">Thrower if argument is null</exception>
        public void IsNotNull(string mesage = "")
        {
            if (_argument == null)
            {
                mesage = string.IsNullOrWhiteSpace(mesage) ? _argumentName : mesage; 
                throw new ArgumentNullException(mesage); 
            }
        }

        public void IsNull(string mesage = "")
        {
            if (_argument != null)
            {
                mesage = string.IsNullOrWhiteSpace(mesage) ? _argumentName : mesage;
                throw new ArgumentException(mesage);
            }
        }
    }
}
