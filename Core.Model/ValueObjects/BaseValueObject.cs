﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.ValueObjects
{
    public abstract class BaseValueObject
    {
        protected static bool EqualOperator(BaseValueObject left, BaseValueObject right)
        {
            if (left is null || right is null)
            {
                return false;
            }
            return ReferenceEquals(left, right) || left.Equals(right);
        }

        protected static bool NotEqualOperator(BaseValueObject left, BaseValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (BaseValueObject)obj;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
        public static bool operator ==(BaseValueObject one, BaseValueObject two)
        {
            return EqualOperator(one, two);
        }

        public static bool operator !=(BaseValueObject one, BaseValueObject two)
        {
            return NotEqualOperator(one, two);
        }
    }
}
