using System;
using NUnit.Framework;
using GCodeNet;

namespace TestProject
{
    [Command(CommandType.M,999)]
    class BoolClass : CommandMapping
    {
        [ParameterType(ParameterType.X)]
        public bool X { get; set; }

    }

    [TestFixture]
    public class BoolType
    {
        [Test]
        public void TestBool()
        {
            CommandReflection.AddMappedType(typeof(BoolClass));
            var c1 = (BoolClass)CommandMapping.Parse("M999 X-1.1");
            Assert.IsTrue(c1.X == true);
            Assert.IsTrue(c1.ToGCode() == "M999 X");
            var c2 = (BoolClass)CommandMapping.Parse("M999 X1.1");
            Assert.IsTrue(c2.X == true);
            Assert.IsTrue(c2.ToGCode() == "M999 X");
            var c3 = (BoolClass)CommandMapping.Parse("M999 X1");
            Assert.IsTrue(c3.X == true);
            Assert.IsTrue(c3.ToGCode() == "M999 X");
            var c4 = (BoolClass)CommandMapping.Parse("M999 X");
            Assert.IsTrue(c4.X == true);
            Assert.IsTrue(c4.ToGCode() == "M999 X");
            var c5 = (BoolClass)CommandMapping.Parse("M999");
            Assert.IsTrue(c5.X == false);
            Assert.IsTrue(c5.ToGCode() == "M999");
        }
    }
}
