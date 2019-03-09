using System;
using System.Collections.Generic;

namespace PinFun
{
    internal class Program
    {
        private static void Main()
        {
            const int pinLength = 4;
            const int numberOfRequestedPins = 1000;

            var policies = new List<Func<IList<ushort>, bool>>
            {
                PinNumberPolicies.DigitsAreNotIncremental,
                PinNumberPolicies.NoDuplicateDigits,
                //PinNumberPolicies.NoEvenNumbers /*can add more...*/
            };

            var sortedSetOfPinNumbers = PinNumber.GenerateSet(numberOfRequestedPins, pinLength, policies);
            DisplayResults(pinLength, numberOfRequestedPins, sortedSetOfPinNumbers);
        }

        private static void DisplayResults(int pinLnegth, int numberOfRequestedPins, ICollection<PinNumber> sortedSet)
        {
            Console.WriteLine($"Requested Pin Length: {pinLnegth}");
            Console.WriteLine($"Number of Requested Pins: {numberOfRequestedPins}");
            Console.WriteLine($"Number of possible pin Combinations: {PinNumber.NumberOfPinCombinations(pinLnegth)}");
            Console.WriteLine($"Based on current policies, the program was able to generate '{sortedSet.Count}' unique pins.");
            Console.WriteLine("Press any key to display pins");
            Console.Read();
            foreach (var pin in sortedSet)
            {
                Console.WriteLine(pin);
            }
        }
    }
}
