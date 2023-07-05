using System.Diagnostics.CodeAnalysis;

namespace Core.Utilities.Exceptions
{
    public class UnauthorizedException : Exception
    {
        private static readonly string DefaultMessage = "UnauthorizedAccess";
        private static readonly int HRESULT = 401;
        public UnauthorizedException(string? message = null) : base(message ?? DefaultMessage)
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
          throw new UnauthorizedException(paramName);
    }

}
