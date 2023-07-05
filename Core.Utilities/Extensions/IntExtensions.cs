using Core.Utilities.Ensures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Extensions
{
    public static class IntExtensions
    {
        public static void EnsureIsGreatherThat(this int value,int greatherThat,string message= "")
        {

            Ensure.That(value).IsGreatherThat(greatherThat,message);
        }
    }
}
