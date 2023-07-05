using Core.Utilities.Ensures;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Get specific value from dictionary on TValue object
        /// </summary>
        /// <typeparam name="TValue">Type of object to convert the value founded in dictionary </typeparam>
        /// <param name="keyValuePairs">Dictionary which we must search our value</param>
        /// <param name="key">key of the value to search</param>
        /// <returns>return TValue of value converter</returns>
        public static TValue? GetValue<TValue>(this IDictionary<string, string> keyValuePairs, string key)
        {
            Ensure.That(keyValuePairs).NotNullOrEmpty();
            Ensure.That(key,nameof(key)).NotNullOrEmpty();
            if (!keyValuePairs.ContainsKey(key))
            {
                return default;
            }
            string value = keyValuePairs[key];
            return (TValue)Convert.ChangeType(value, typeof(TValue));
        }

        
    }
}
