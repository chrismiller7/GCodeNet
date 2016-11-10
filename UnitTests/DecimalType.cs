using System;
using NUnit.Framework;
using GCodeNet;

namespace TestProject
{
    [Command(CommandType.M,999)]
    class DecimalClass : CommandMapping
    {
        [ParameterType(ParameterType.X)]
        public decimal X { get; set; }
        [ParameterType(ParameterType.Y)]
        public decimal? Y { get; set; }
    }

    [TestFixture]
    public class DecimalType
    {
        [Test]
        public void TestDecimal()
        {
            CommandReflection.AddMappedType(typeof(DecimalClass));
            var c1 = (DecimalClass)CommandMapping.Parse("M999 X-1.1");
            Assert.IsTrue(c1.X == -1.1m);
            Assert.IsTrue(c1.ToGCode() == "M999 X-1.1");
            var c2 = (DecimalClass)CommandMapping.Parse("M999 X1.1");
            Assert.IsTrue(c2.X == 1.1m);
            Assert.IsTrue(c2.ToGCode() == "M999 X1.1");
            var c3 = (DecimalClass)CommandMapping.Parse("M999 X1");
            Assert.IsTrue(c3.X == 1);
            Assert.IsTrue(c3.ToGCode() == "M999 X1");
            var c4 = (DecimalClass)CommandMapping.Parse("M999 X");
            Assert.IsTrue(c4.X == 0);
            Assert.IsTrue(c4.ToGCode() == "M999 X0");
            var c5 = (DecimalClass)CommandMapping.Parse("M999");
            Assert.IsTrue(c5.X == 0);
            Assert.IsTrue(c5.ToGCode() == "M999 X0");
        }

        [Test]
        public void TestNullableDecimal()
        {
            CommandReflection.AddMappedType(typeof(DecimalClass));
            var c1 = (DecimalClass)CommandMapping.Parse("M999 Y-1.1");
            Assert.IsTrue(c1.Y == -1.1m);
            Assert.IsTrue(c1.ToGCode() == "M999 X0 Y-1.1");
            var c2 = (DecimalClass)CommandMapping.Parse("M999 Y1.1");
            Assert.IsTrue(c2.Y == 1.1m);
            Assert.IsTrue(c2.ToGCode() == "M999 X0 Y1.1");
            var c3 = (DecimalClass)CommandMapping.Parse("M999 Y1");
            Assert.IsTrue(c3.Y == 1);
            Assert.IsTrue(c3.ToGCode() == "M999 X0 Y1");
            var c4 = (DecimalClass)CommandMapping.Parse("M999 Y");
            Assert.IsTrue(c4.Y == null);
            Assert.IsTrue(c4.ToGCode() == "M999 X0");
            var c5 = (DecimalClass)CommandMapping.Parse("M999");
            Assert.IsTrue(c5.Y == null);
            Assert.IsTrue(c4.ToGCode() == "M999 X0");
        }
    }
}
