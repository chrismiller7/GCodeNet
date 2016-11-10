using System;
using NUnit.Framework;
using GCodeNet;

namespace TestProject
{
    [Command(CommandType.M,999)]
    class DoubleClass : CommandMapping
    {
        [ParameterType(ParameterType.X)]
        public double X { get; set; }
        [ParameterType(ParameterType.Y)]
        public double? Y { get; set; }
    }

    [TestFixture]
    public class DoubleType
    {
        [Test]
        public void TestDouble()
        {
            CommandReflection.AddMappedType(typeof(DoubleClass));
            var c1 = (DoubleClass)CommandMapping.Parse("M999 X-1.1");
            Assert.IsTrue(c1.X == -1.1);
            Assert.IsTrue(c1.ToGCode() == "M999 X-1.1");
            var c2 = (DoubleClass)CommandMapping.Parse("M999 X1.1");
            Assert.IsTrue(c2.X == 1.1);
            Assert.IsTrue(c2.ToGCode() == "M999 X1.1");
            var c3 = (DoubleClass)CommandMapping.Parse("M999 X1");
            Assert.IsTrue(c3.X == 1);
            Assert.IsTrue(c3.ToGCode() == "M999 X1");
            var c4 = (DoubleClass)CommandMapping.Parse("M999 X");
            Assert.IsTrue(c4.X == 0);
            Assert.IsTrue(c4.ToGCode() == "M999 X0");
            var c5 = (DoubleClass)CommandMapping.Parse("M999");
            Assert.IsTrue(c5.X == 0);
            Assert.IsTrue(c5.ToGCode() == "M999 X0");
        }

        [Test]
        public void TestNullableDouble()
        {
            CommandReflection.AddMappedType(typeof(DoubleClass));
            var c1 = (DoubleClass)CommandMapping.Parse("M999 Y-1.1");
            Assert.IsTrue(c1.Y == -1.1);
            Assert.IsTrue(c1.ToGCode() == "M999 X0 Y-1.1");
            var c2 = (DoubleClass)CommandMapping.Parse("M999 Y1.1");
            Assert.IsTrue(c2.Y == 1.1);
            Assert.IsTrue(c2.ToGCode() == "M999 X0 Y1.1");
            var c3 = (DoubleClass)CommandMapping.Parse("M999 Y1");
            Assert.IsTrue(c3.Y == 1);
            Assert.IsTrue(c3.ToGCode() == "M999 X0 Y1");
            var c4 = (DoubleClass)CommandMapping.Parse("M999 Y");
            Assert.IsTrue(c4.Y == null);
            Assert.IsTrue(c4.ToGCode() == "M999 X0");
            var c5 = (DoubleClass)CommandMapping.Parse("M999");
            Assert.IsTrue(c5.Y == null);
            Assert.IsTrue(c5.ToGCode() == "M999 X0");
        }
    }
}
