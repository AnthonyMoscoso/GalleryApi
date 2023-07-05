using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Utilities.Validations
{
    public class ValidatesDate
    {
        private readonly DateTime? _value;
        public ValidatesDate(DateTime? value)
        {
            _value = value;
        }
        public bool NotIsNull()
        {
            return _value != null;
        }
        public bool NotIsDefault()
        {

            return _value.HasValue && _value.Value != DateTime.MinValue;
        }

    }
}
