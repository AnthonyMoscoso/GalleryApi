using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Exceptions
{
    public class NotFoundException : Exception
    {
        private static readonly string DefaultMessage = "Not Found";
        private static readonly int HRESULT = 404;
        public NotFoundException(string? message = null) : base(message ?? DefaultMessage)
        {
            HResult = HRESULT;
        }

        public static void ThrowIfTrue(bool condition, string? message = null)
        {
            if (condition)
            {
                Throw(message);
            }
        }
        public static void ThrowIfFalse(bool condition, string? message = null)
        {
            if (!condition)
            {
                Throw(message);
            }
        }

        [DoesNotReturn]
        private static void Throw(string? paramName) =>
          throw new NotFoundException(paramName);
    }

}