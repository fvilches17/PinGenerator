using System;
using System.Collections.Generic;
using System.Linq;

namespace PinFun
{
    public struct PinNumber: IComparable<PinNumber>, IEquatable<PinNumber>
    {
        public ushort[] DigitArray { get; }
        private static readonly Random Rand = new Random();

        public int Length => DigitArray.Length;

        public PinNumber(params ushort[] digitArray)
        {
            if (digitArray?.Length <= 0)
            {
                throw new ArgumentException($"{nameof(DigitArray)} has to have at least one value");
            }

            DigitArray = digitArray;
        }

        private PinNumber(int length)
        {
            DigitArray = new ushort[length];

            for (var i = 0; i < DigitArray.Length; i++)
            {
                DigitArray[i] = (ushort)Rand.Next(minValue: 0, maxValue: 10/*Exclusive*/);
            }
        }

        public static PinNumber Generate(int pinLength)
        {
            if (pinLength <= 0) throw new ArgumentOutOfRangeException($"{nameof(pinLength)} should be greater than 0");
            return new PinNumber(pinLength);
        }

        public static PinNumber Generate(int pinLength, IList<Func<IList<ushort>, bool>> policies)
        {
            if (pinLength <= 0) throw new ArgumentOutOfRangeException($"{nameof(pinLength)} should be greater than 0");
            return GenerateSet(numberOfRequestedPins: 1, pinLength: pinLength, policies: policies).First();
        }

        public static ISet<PinNumber> GenerateSet(int numberOfRequestedPins, int pinLength, IList<Func<IList<ushort>, bool>> policies)
        {
            if (numberOfRequestedPins <= 0) throw new ArgumentOutOfRangeException($"{nameof(numberOfRequestedPins)} should be greater than 0");
            if (pinLength <= 0) throw new ArgumentOutOfRangeException($"{nameof(pinLength)} should be greater than 0");

            var setOfAllAvailablePins = GenerateAllAvailablePins(pinLength);
            ISet<PinNumber> sortedSetOfPinNumbers = new SortedSet<PinNumber>();

            while (sortedSetOfPinNumbers.Count < numberOfRequestedPins && setOfAllAvailablePins.Any())
            {
                var pin = Generate(pinLength);
                setOfAllAvailablePins.Remove(pin);

                if (pin.Validate(policies))
                {
                    sortedSetOfPinNumbers.Add(pin);
                }
            }

            return sortedSetOfPinNumbers;
        }

        public bool Validate(IList<Func<IList<ushort>, bool>> policies)
        {
            if (policies == null || !policies.Any())
            {
                return true;
            }

            var digitArray = DigitArray;
            return policies.All(policy => policy(digitArray));
        }

        public static int NumberOfPinCombinations(int pinLength)
        {
            if (pinLength <= 0) throw new ArgumentOutOfRangeException(nameof(pinLength));
            return (int)Math.Pow(10, pinLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        ///<exception cref="FormatException"></exception>
        /// <returns></returns>
        public static PinNumber Parse(string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            var charArray = s.ToCharArray();
            var digitArray = new ushort[charArray.Length];
            for (var i = 0; i < charArray.Length; i++)
            {
                digitArray[i] = ushort.Parse(charArray[i].ToString());
            }
            
            return new PinNumber(digitArray);
        }

        private static HashSet<PinNumber> GenerateAllAvailablePins(int pinLength)
        {
            if (pinLength <= 0) throw new ArgumentOutOfRangeException(nameof(pinLength));

            var max = NumberOfPinCombinations(pinLength);

            var leadingZeros = max.ToString().Length - 1;

            var result = new HashSet<PinNumber>();
            foreach (var i in Enumerable.Range(start: 0, count: max))
            {
                var pin = Parse(i.ToString($"D{leadingZeros}"));
                result.Add(pin);
            }

            return result;
        }

        public override string ToString()
        {
            return string.Join(string.Empty, DigitArray);
        }

        #region Equality
        public static bool operator ==(PinNumber lhs, PinNumber rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(PinNumber lhs, PinNumber rhs)
        {
            return !lhs.Equals(rhs);
        }

        public bool Equals(PinNumber other)
        {
            return string.Join(string.Empty, DigitArray) == string.Join(string.Empty, other.DigitArray);
        }

        public int CompareTo(PinNumber other)
        {
            return String.Compare(string.Join(string.Empty, DigitArray), string.Join(string.Empty, other.DigitArray), StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is PinNumber other && Equals(other);
        }

        public override int GetHashCode()
        {
            return string.Join(string.Empty, DigitArray).GetHashCode();
        } 
        #endregion
    }
}
