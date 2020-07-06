using System;
using System.Globalization;
using Metars.Converters;
using NUnit.Framework;

namespace UnitTests.ConverterTests
{
    [TestFixture]
    public class BooleanNegatedConverterTests
    {
        [Test]
        public void TestTrueReturnsFalse()
        {
            var converter = new BooleanNegatedConverter();
            var tTest = converter.Convert(true, typeof(bool), null, CultureInfo.CurrentCulture);
            Assert.IsFalse((bool)tTest);
        }

        [Test]
        public void TestFalseReturnsTrue()
        {
            var converter = new BooleanNegatedConverter();
            var fTest = converter.Convert(false, typeof(bool), null, CultureInfo.CurrentCulture);
            Assert.IsTrue((bool)fTest);
        }

        [Test]
        public void TestFalseReturnsTrueBack()
        {
            var converter = new BooleanNegatedConverter();
            var fTest = converter.ConvertBack(false, typeof(bool), null, CultureInfo.CurrentCulture);
            Assert.IsTrue((bool)fTest);
        }

        [Test]
        public void TestTrueReturnsFalseBack()
        {
            var converter = new BooleanNegatedConverter();
            var tTest = converter.ConvertBack(true, typeof(bool), null, CultureInfo.CurrentCulture);
            Assert.IsFalse((bool)tTest);
        }
    }
}
