using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCodeNet;

namespace TestProject
{
    [TestClass]
    public class CommandTest
    {
        [TestMethod]
        public void Ctor()
        {
            Command cmd = new Command(CommandType.G, 3);
            Assert.IsTrue(cmd.ToGCode() == "G3");
        }

        [TestMethod]
        public void SetParameterValue()
        {
            Command cmd = new Command(CommandType.G, 3);
            cmd.SetParameterValue(ParameterType.X, "-23");
            Assert.IsTrue(cmd.ToGCode() == "G3 X-23");
        }

        [TestMethod]
        public void ToGCodeWithLineNumber()
        {
            Command cmd = new Command(CommandType.G, 3);
            cmd.SetParameterValue(ParameterType.X, "-23");
            Assert.IsTrue(cmd.ToGCode(false, 5) == "N5 G3 X-23");
        }

        [TestMethod]
        public void ToGCodeWithCRC()
        {
            Command cmd = new Command(CommandType.G, 3);
            cmd.SetParameterValue(ParameterType.X, "-23");
            Assert.IsTrue(cmd.ToGCode(true) == "G3 X-23*32");
        }

        [TestMethod]
        public void ToGCodeWithLineNumberAndCRC()
        {
            Command cmd = new Command(CommandType.G, 3);
            cmd.SetParameterValue(ParameterType.X, "-23");
            Assert.IsTrue(cmd.ToGCode(true, 5) == "N5 G3 X-23*123");
        }
    }
}
