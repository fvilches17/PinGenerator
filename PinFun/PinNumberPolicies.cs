using System;
using System.Collections.Generic;
using System.Linq;

namespace PinFun
{
    public static class PinNumberPolicies
    {
        public static bool DigitsAreNotIncremental(IList<ushort> pinNumbers)
        {
            if (pinNumbers == null) throw new ArgumentNullException(nameof(pinNumbers));
            if (pinNumbers.Count == 1) return true;

            var index = 1;
            while (index < pinNumbers.Count)
            {
                bool isIncremental = pinNumbers[index - 1] < pinNumbers[index];
                if (!isIncremental) return true;
                index++;
            }

            return false;
        }

        public static bool NoDuplicateDigits(IList<ushort> pinNumbers)
        {
            return new HashSet<ushort>(pinNumbers).Count == pinNumbers.Count;
        }

        public static bool NoEvenNumbers(IList<ushort> pinNumbers)
        {
            return pinNumbers.All(number => number % 2 != 0);
        }
    }
}