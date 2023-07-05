using Core.Utilities.Ensures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.ValueObjects
{
    public class PositiveNum : BaseValueObject
    {
        public int Value { get; private set; }
        public PositiveNum(int value)
        {
            Ensure.That(value, nameof(value)).IsGreatherThat(0);
            Value = value;
        }

        public static implicit operator int(PositiveNum positiveNum)
        {
            return positiveNum.Value;
        }

        public static explicit operator PositiveNum(int value)
        {
            return new PositiveNum(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
