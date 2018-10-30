using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PinFun.UnitTests
{
    [TestClass]
    public class PinNumberTests
    {
        [TestMethod]
        public void Length_Should_Be_Four()
        {
            //Arrange
            //Act
            var pin = new PinNumber(1, 1, 1, 1);

            //Assert
            Assert.IsTrue(pin.Length == 4);
        }

        [TestMethod]
        public void GeneratedPin_Length_Should_Be_Five()
        {
            //Arrange
            //Act
            var pin = PinNumber.Generate(5);

            //Assert
            Assert.IsTrue(pin.Length == 5);
        }

        [TestMethod]
        public void Two_PinNumbers_Should_Be_Equal()
        {
            //Arrange
            var pinA = new PinNumber(1, 5, 5, 1);
            var pinB = new PinNumber(1, 5, 5, 1);

            //Act
            var resultviaOperand = pinA == pinB;
            var resultviaEqualsMethod = pinA.Equals(pinB);

            //Assert
            Assert.IsTrue(resultviaOperand);
            Assert.IsTrue(resultviaEqualsMethod);
        }

        [TestMethod]
        public void Two_PinNumbers_Should_Be_NotEqual()
        {
            //Arrange
            var pinA = new PinNumber(1, 5, 5, 1);
            var pinB = new PinNumber(1, 5, 5, 1, 6, 8);

            //Act
            var resultviaOperand = pinA == pinB;
            var resultviaEqualsMethod = pinA.Equals(pinB);

            //Assert
            Assert.IsFalse(resultviaOperand);
            Assert.IsFalse(resultviaEqualsMethod);
        }

        [TestMethod]
        public void Pin_With_Policy_NoEvenNumbers_Should_Be_Valid()
        {
            //Arrange
            var pin = PinNumber.Generate(pinLength: 4, policies: new List<Func<IList<ushort>, bool>> { PinNumberPolicies.NoEvenNumbers });

            //Act
            var actual = pin.DigitArray.All(d => d % 2 != 0);

            //Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void Pin_With_Policy_NoDuplicateDigits_Should_Be_Valid()
        {
            //Arrange
            var pin = new PinNumber(1, 4, 5, 9);

            //Act
            var isValid = pin.Validate(new List<Func<IList<ushort>, bool>> { PinNumberPolicies.NoDuplicateDigits });

            //Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Pin_With_Policy_NoDuplicateDigits_Should_Be_InValid()
        {
            //Arrange
            var pin = new PinNumber(1, 1, 1, 1, 1, 1);

            //Act
            var isValid = !pin.Validate(new List<Func<IList<ushort>, bool>> { PinNumberPolicies.NoDuplicateDigits });

            //Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Pin_With_Policy_DigitsAreNotIncremental_Should_Be_Valid()
        {
            //Arrange
            var pin = new PinNumber(1, 4, 5, 0);

            //Act
            var isValid = pin.Validate(new List<Func<IList<ushort>, bool>> { PinNumberPolicies.DigitsAreNotIncremental });

            //Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void NumberOfPinCombinations_Should_be_OneHundred()
        {
            //Arrange
            //Act
            const int expected = 100;
            var actual = PinNumber.NumberOfPinCombinations(pinLength: 2);

            //Assert
            Assert.AreEqual(expected, actual, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Generate_ShouldFail_If_PinLenth_Is_LessThan_Zero()
        {
            PinNumber.Generate(pinLength: -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NumberOfCombinations_ShouldFail_If_PinLenth_Is_LessThan_Zero()
        {
            PinNumber.NumberOfPinCombinations(pinLength: -1);
        }

        //Etc.
    }
}
