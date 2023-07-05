using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Ensures
{
    public static  class Ensure
    {
        /// <summary>
        /// Ensure method for object
        /// </summary>
        /// <param name="argument">object to ensure</param>
        /// <param name="argumentName">name of argument</param>
        /// <returns></returns>
        public static EnsureObject That(object? argument,string argumentName = "")
        {
            return new EnsureObject(argument, argumentName);
        }
        /// <summary>
        /// Ensure method for string
        /// </summary>
        /// <param name="argument">string to ensure</param>
        /// <param name="argumentName">name of argument</param>
        /// <returns></returns>
        public static EnsureString That(string? argument, string argumentName = "")
        {
            return new EnsureString(argument, argumentName);
        }
        /// <summary>
        /// Ensure method for integers
        /// </summary>
        /// <param name="argument">int to ensure</param>
        /// <param name="argumentName">name of argument</param>
        /// <returns></returns>
        public static EnsureInt That(int argument, string argumentName = "")
        {
            return new EnsureInt(argument, argumentName);
        }
        /// <summary>
        /// Ensure method for integers
        /// </summary>
        /// <param name="argument">double to ensure</param>
        /// <param name="argumentName">name of argument</param>
        /// <returns></returns>
        public static EnsureDouble That(double argument, string argumentName = "")
        {
            return new EnsureDouble(argument, argumentName);
        }
        /// <summary>
        /// Ensure method for integers
        /// </summary>
        /// <param name="argument">decimal to ensure</param>
        /// <param name="argumentName">name of argument</param>
        /// <returns></returns>
        public static EnsureDecimal That(decimal argument, string argumentName = "")
        {
            return new EnsureDecimal(argument, argumentName);
        }
        /// <summary>
        /// Ensure method for collection
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="argument">collection to ensure</param>
        /// <param name="argumentName">name of argument</param>
        /// <returns></returns>
        public static EnsureCollection<T> That<T>(ICollection<T>? argument,string argumentName = "")
        {
            return new EnsureCollection<T>(argument, argumentName);
        }

    }
}
