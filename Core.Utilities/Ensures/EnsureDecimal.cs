using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Ensures
{
    public class EnsureDecimal
    {
        private readonly decimal _argument;
        private readonly string _argumentName;
        public EnsureDecimal(decimal argument, string argumentName = "")
        {
            _argument = argument;
            _argumentName = string.IsNullOrWhiteSpace(argumentName) ? nameof(_argument) : argumentName;
        }

        /// <summary>
        /// Ensure argument is greather that value
        /// </summary>
        /// <param name="value">value whitch our argument must be greather</param>
        /// <param name="message">error message</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrower if argument is smaller that value</exception>
        public void IsGreatherThat(decimal value, string message = "")
        {
            if (_argument < value)
            {
                message = string.IsNullOrWhiteSpace(message) ? _argumentName : message;
                throw new ArgumentOutOfRangeException(message);
            }
        }
        /// <summary>
        /// Ensure argument is lower that value
        /// </summary>
        /// <param name="value">value whitch our argument must be lower</param>
        /// <param name="message">custom error message</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrower if argument is bigger that value</exception>
        public void IsLowerThat(decimal value, string message = "")
        {
            if (_argument > value)
            {
                message = string.IsNullOrWhiteSpace(message) ? _argumentName : message;
                throw new ArgumentOutOfRangeException(message);
            }
        }
        /// <summary>
        /// Ensure argument is between two values
        /// </summary>
        /// <param name="num1">firt num</param>
        /// <param name="num2">second num</param>
        /// <param name="message">custom error message</param>
        /// <exception cref="ArgumentOutOfRangeException">thrower if argument not is between our values</exception>
        public void IsBetweenThat(decimal num1, decimal num2, string message = "")
        {
            if (num1 > num2)
            {
                throw new ArgumentOutOfRangeException("num1 must be lower that num2");
            }
            if (_argument < num1 || _argument > num2)
            {
                message = string.IsNullOrWhiteSpace(message) ? _argumentName : message;
                throw new ArgumentOutOfRangeException(message);
            }
        }
        /// <summary>
        /// Ensure that argument is on enum 
        /// </summary>
        /// <param name="enumType">type of enum</param>
        /// <param name="mesage">custom error message</param>
        /// <exception cref="ArgumentOutOfRangeException">Thorwer exception when argument not is enum</exception>
        public void IsEnum(Type enumType, string mesage = "")
        {
            if (!Enum.IsDefined(enumType, _argument))
            {
                mesage = string.IsNullOrWhiteSpace(mesage) ? _argumentName : mesage;
                throw new ArgumentOutOfRangeException(mesage);
            }
        }
    }
}