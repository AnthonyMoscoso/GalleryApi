using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Extensions
{
    public static class ThrowExceptionExtension
    {
        public static void ThrowExcetionIfFalse(this bool value, Exception ex)
        {
            if (!value)
            { throw ex; }
        }
        public static void ThrowExcetionIsTrue(this bool value, Exception ex)
        {
            if (value)
            { throw ex; }
        }
    }
}
