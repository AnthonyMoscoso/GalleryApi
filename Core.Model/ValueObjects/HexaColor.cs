using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.ValueObjects
{
    public class HexaColor : BaseValueObject
    {
        public string Value { get; private set; }
        public HexaColor(string value) 
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (!IsValidHexaCode(value))
            {
                throw new ArgumentException("Hexadecimal color not is correct");
            }
            Value = value;
        }

        public static explicit operator HexaColor(string value)
        {
            return new HexaColor(value);
        }

        public static implicit operator string (HexaColor color)
        {
            return color.Value;
        }
        private static bool IsValidHexaCode(string str)
        {
            if (str[0] != '#')
                return false;

            if (!(str.Length == 4 || str.Length == 7))
                return false;

            for (int i = 1; i < str.Length; i++)
                if (!((str[i] >= '0' && str[i] <= 9)
                    || (str[i] >= 'a' && str[i] <= 'f')
                    || (str[i] >= 'A' || str[i] <= 'F')))
                    return false;

            return true;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
