using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCodeNet;
using System.Linq;

namespace TestProject
{
    [TestClass]
    public class CommandTest
    {
        [TestMethod]
        public void CommandCtor()
        {
            Command cmd = new Command(CommandType.G, 3);
            Assert.IsTrue(cmd.ToGCode() == "G3");
        }

        [TestMethod]
        public void SetParameterValue()
        {
            Command cmd = new Command(CommandType.G, 3);
            cmd.SetParameterValue(ParameterType.X, -23);
            Assert.IsTrue(cmd.ToGCode() == "G3 X-23");
        }

        [TestMethod]
        public void SetParameterEmptyValue()
        {
            Command cmd = new Command(CommandType.G, 3);
            cmd.SetParameterValue(ParameterType.X, null);
            Assert.IsTrue(cmd.ToGCode() == "G3 X");
        }

        [TestMethod]
        public void ToGCodeWithLineNumber()
        {
            Command cmd = new Command(CommandType.G, 3);
            cmd.SetParameterValue(ParameterType.X, -23.1234m);
            Assert.IsTrue(cmd.ToGCode(false, 5) == "N5 G3 X-23.1234");
        }

        [TestMethod]
        public void ToGCodeWithCRC()
        {
            Command cmd = new Command(CommandType.G, 3);
            cmd.SetParameterValue(ParameterType.X, -23);
            Assert.IsTrue(cmd.ToGCode(true) == "G3 X-23*32");
        }

        [TestMethod]
        public void ToGCodeWithLineNumberAndCRC()
        {
            Command cmd = new Command(CommandType.G, 3);
            cmd.SetParameterValue(ParameterType.X, -23);
            Assert.IsTrue(cmd.ToGCode(true, 5) == "N5 G3 X-23*123");
        }

        [TestMethod]
        public void CommandParse()
        {
            var cmd = Command.Parse("G1 X Y-3 Z1.4");
            Assert.IsTrue(cmd.CommandType == CommandType.G);
            Assert.IsTrue(cmd.CommandSubType == 1);
            var parameters = cmd.GetParameters().ToArray();
            Assert.IsTrue(parameters[0] == ParameterType.X);
            Assert.IsTrue(parameters[1] == ParameterType.Y);
            Assert.IsTrue(parameters[2] == ParameterType.Z);

            Assert.IsTrue(cmd.GetParameterValue(ParameterType.X) == null);
            Assert.IsTrue((decimal)cmd.GetParameterValue(ParameterType.Y) == -3);
            Assert.IsTrue((decimal)cmd.GetParameterValue(ParameterType.Z) == 1.4m);
        }

        [TestMethod]
        public void CommandParseLowercase()
        {
            var cmd = Command.Parse("g1 x");
            Assert.IsTrue(cmd.CommandType == CommandType.G);
            Assert.IsTrue(cmd.CommandSubType == 1);
            var parameters = cmd.GetParameters().ToArray();
            Assert.IsTrue(parameters[0] == ParameterType.X);

            Assert.IsTrue(cmd.GetParameterValue(ParameterType.X) == null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CommandParseMultipleCommands()
        {
            var cmd = Command.Parse("N2 G1");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CommandParseCrcException()
        {
            var cmd = Command.Parse("G1*34");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CommandParseCommentException()
        {
            var cmd = Command.Parse("G1;comment");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CommandParseEmptyStringException()
        {
            var cmd = Command.Parse("");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CommandParseNoSubtypeException()
        {
            var cmd = Command.Parse("G");
        }

        [TestMethod]
        public void CommandParseWhitespace()
        {
            var cmd = Command.Parse("M \n 1  \t\t\t   X \t \n\n  23");
            Assert.IsTrue(cmd.CommandType == CommandType.M);
            Assert.IsTrue(cmd.CommandSubType == 1);
            Assert.IsTrue((decimal)cmd.GetParameterValue( ParameterType.X) == 23m);
        }

        [TestMethod]
        public void CommandParseNoWhitespace()
        {
            var cmd = Command.Parse("M1X23YZ");
            Assert.IsTrue(cmd.CommandType == CommandType.M);
            Assert.IsTrue(cmd.CommandSubType == 1);
            Assert.IsTrue((decimal)cmd.GetParameterValue(ParameterType.X) == 23m);
            Assert.IsTrue(cmd.GetParameterValue(ParameterType.Y) == null);
            Assert.IsTrue(cmd.GetParameterValue(ParameterType.Z) == null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CommandParseInvalidCommandTypeException()
        {
            var cmd = Command.Parse("Z3");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CommandParseInvalidParameterException()
        {
            var cmd = Command.Parse("M3 A");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CommandParseNoCommandTypeException()
        {
            var cmd = Command.Parse("34");
        }

        [TestMethod]
        public void CommandParseLeadingZeros()
        {
            var cmd = Command.Parse("G0003 X00020");
            Assert.IsTrue(cmd.CommandSubType == 3);
            Assert.IsTrue((decimal)cmd.GetParameterValue(ParameterType.X) == 20);
        }
    }
}
