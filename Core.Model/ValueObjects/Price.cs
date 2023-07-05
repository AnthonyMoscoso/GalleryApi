using Core.Utilities.Ensures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.ValueObjects
{
    public class Price : BaseValueObject
    {
        public decimal Amounth { get; private set; }
        public string Currency { get; private set; }
        public Price(decimal amounth,string currency) 
        {
            Ensure.That(currency).NotNullOrEmpty();
            Ensure.That(amounth).IsGreatherThat(0);
            Amounth = amounth;
            Currency= currency;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amounth;
            yield return Currency; 
        }
    }
}
