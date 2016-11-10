using System;
using NUnit.Framework;
using GCodeNet;

namespace TestProject
{
    [Command(CommandType.M,999)]
    class ByteClass : CommandMapping
    {
        [ParameterType(ParameterType.X)]
        public byte X { get; set; }
        [ParameterType(ParameterType.Y)]
        public byte? Y { get; set; }
    }

    [TestFixture]
    public class ByteType
    {
        [Test]
        public void TestByte()
        {
            CommandReflection.AddMappedType(typeof(ByteClass));
            var c2 = (ByteClass)CommandMapping.Parse("M999 X1.1");
            Assert.IsTrue(c2.X == 1);
            Assert.IsTrue(c2.ToGCode() == "M999 X1");
            var c3 = (ByteClass)CommandMapping.Parse("M999 X1");
            Assert.IsTrue(c3.X == 1);
            Assert.IsTrue(c3.ToGCode() == "M999 X1");
            var c4 = (ByteClass)CommandMapping.Parse("M999 X");
            Assert.IsTrue(c4.X == 0);
            Assert.IsTrue(c4.ToGCode() == "M999 X0");
            var c5 = (ByteClass)CommandMapping.Parse("M999");
            Assert.IsTrue(c5.X == 0);
            Assert.IsTrue(c5.ToGCode() == "M999 X0");
        }

        [Test]
        public void TestNullableByte()
        {
            CommandReflection.AddMappedType(typeof(ByteClass));
            var c2 = (ByteClass)CommandMapping.Parse("M999 Y1.1");
            Assert.IsTrue(c2.Y == 1);
            Assert.IsTrue(c2.ToGCode() == "M999 X0 Y1");
            var c3 = (ByteClass)CommandMapping.Parse("M999 Y1");
            Assert.IsTrue(c3.Y == 1);
            Assert.IsTrue(c3.ToGCode() == "M999 X0 Y1");
            var c4 = (ByteClass)CommandMapping.Parse("M999 Y");
            Assert.IsTrue(c4.Y == null);
            Assert.IsTrue(c4.ToGCode() == "M999 X0");
            var c5 = (ByteClass)CommandMapping.Parse("M999");
            Assert.IsTrue(c5.Y == null);
            Assert.IsTrue(c5.ToGCode() == "M999 X0");
        }
    }
}
