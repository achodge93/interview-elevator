using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string comparer, string input)
        {
            // string.equals returns an error on nulls, so return false to emulate.
            if(comparer == null)
            {
                return false;
            }
            return comparer.Equals(input, StringComparison.OrdinalIgnoreCase);
        }
    }
}
