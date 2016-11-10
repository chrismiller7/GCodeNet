using System;
using NUnit.Framework;
using GCodeNet;

namespace TestProject
{
    enum EnumObject
    {
        A = 1,
        B = 2,
    }

    [Command(CommandType.M,999)]
    class EnumClass : CommandMapping
    {
        [ParameterType(ParameterType.X)]
        public EnumObject X { get; set; }
        [ParameterType(ParameterType.Y)]
        public EnumObject? Y { get; set; }
    }

    [TestFixture]
    public class EnumType
    {
        [Test]
        public void TestEnum()
        {
            CommandReflection.AddMappedType(typeof(EnumClass));
            var c1 = (EnumClass)CommandMapping.Parse("M999 X1");
            Assert.IsTrue(c1.X == EnumObject.A);
            Assert.IsTrue(c1.ToGCode() == "M999 X1");
            var c2 = (EnumClass)CommandMapping.Parse("M999 X2");
            Assert.IsTrue(c2.X == EnumObject.B);
            Assert.IsTrue(c2.ToGCode() == "M999 X2");
            var c3 = (EnumClass)CommandMapping.Parse("M999 X5");
            Assert.IsTrue(c3.X == (EnumObject)5);
            Assert.IsTrue(c3.ToGCode() == "M999 X5");
            var c4 = (EnumClass)CommandMapping.Parse("M999 X");
            Assert.IsTrue(c4.X == (EnumObject)0);
            Assert.IsTrue(c4.ToGCode() == "M999 X0");
            var c5 = (EnumClass)CommandMapping.Parse("M999");
            Assert.IsTrue(c5.X == (EnumObject)0);
            Assert.IsTrue(c5.ToGCode() == "M999 X0");
            var c6 = (EnumClass)CommandMapping.Parse("M999 X-1.1");
            Assert.IsTrue(c6.X == (EnumObject)(-1));
            Assert.IsTrue(c6.ToGCode() == "M999 X-1");
        }

        [Test]
        public void TestNullableEnum()
        {
            CommandReflection.AddMappedType(typeof(EnumClass));
            var c1 = (EnumClass)CommandMapping.Parse("M999 Y1");
            Assert.IsTrue(c1.Y == EnumObject.A);
            Assert.IsTrue(c1.ToGCode() == "M999 X0 Y1");
            var c2 = (EnumClass)CommandMapping.Parse("M999 Y2");
            Assert.IsTrue(c2.Y == EnumObject.B);
            Assert.IsTrue(c2.ToGCode() == "M999 X0 Y2");
            var c3 = (EnumClass)CommandMapping.Parse("M999 Y5");
            Assert.IsTrue(c3.Y == (EnumObject)5);
            Assert.IsTrue(c3.ToGCode() == "M999 X0 Y5");
            var c4 = (EnumClass)CommandMapping.Parse("M999 Y");
            Assert.IsTrue(c4.Y == null);
            Assert.IsTrue(c4.ToGCode() == "M999 X0");
            var c5 = (EnumClass)CommandMapping.Parse("M999");
            Assert.IsTrue(c5.Y == null);
            Assert.IsTrue(c5.ToGCode() == "M999 X0");
            var c6 = (EnumClass)CommandMapping.Parse("M999 Y-1.1");
            Assert.IsTrue(c6.Y == (EnumObject)(-1));
            Assert.IsTrue(c6.ToGCode() == "M999 X0 Y-1");
        }
    }
}
