using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Validations
{
    public static class Validate
    {
        public static ValidateString That(string value)
        {
            return new ValidateString(value);
        }
        public static ValidatesDate That(DateTime? value)
        {
            return new ValidatesDate(value);
        }
    }
}
