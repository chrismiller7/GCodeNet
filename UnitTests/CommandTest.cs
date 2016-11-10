using System;
using NUnit.Framework;
using GCodeNet;
using System.Linq;

namespace TestProject
{
    [TestFixture]
    public class CommandTest
    {
        [Test]
        public void CommandCtor()
        {
            Command cmd = new Command(CommandType.G, 3);
            Assert.IsTrue(cmd.ToGCode() == "G3");
        }

        [Test]
        public void SetParameterValue()
        {
            Command cmd = new Command(CommandType.G, 3);
            cmd.SetParameterValue(ParameterType.X, -23);
            Assert.IsTrue(cmd.ToGCode() == "G3 X-23");
        }

        [Test]
        public void SetParameterEmptyValue()
        {
            Command cmd = new Command(CommandType.G, 3);
            cmd.SetParameterValue(ParameterType.X, null);
            Assert.IsTrue(cmd.ToGCode() == "G3 X");
        }

        [Test]
        public void ToGCodeWithLineNumber()
        {
            Command cmd = new Command(CommandType.G, 3);
            cmd.SetParameterValue(ParameterType.X, -23.1234m);
            Assert.IsTrue(cmd.ToGCode(false, 5) == "N5 G3 X-23.1234");
        }

        [Test]
        public void ToGCodeWithCRC()
        {
            Command cmd = new Command(CommandType.G, 3);
            cmd.SetParameterValue(ParameterType.X, -23);
            Assert.IsTrue(cmd.ToGCode(true) == "G3 X-23*32");
        }

        [Test]
        public void ToGCodeWithLineNumberAndCRC()
        {
            Command cmd = new Command(CommandType.G, 3);
            cmd.SetParameterValue(ParameterType.X, -23);
            Assert.IsTrue(cmd.ToGCode(true, 5) == "N5 G3 X-23*123");
        }

        [Test]
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

        [Test]
        public void CommandParseLowercase()
        {
            var cmd = Command.Parse("g1 x");
            Assert.IsTrue(cmd.CommandType == CommandType.G);
            Assert.IsTrue(cmd.CommandSubType == 1);
            var parameters = cmd.GetParameters().ToArray();
            Assert.IsTrue(parameters[0] == ParameterType.X);

            Assert.IsTrue(cmd.GetParameterValue(ParameterType.X) == null);
        }

        [Test]
        public void CommandParseMultipleCommands()
        {
            Assert.Catch(typeof(Exception), () => { var cmd = Command.Parse("N2 G1"); });
        }

        [Test]
        public void CommandParseCrcException()
        {
            Assert.Catch(typeof(Exception), () => { var cmd = Command.Parse("G1*34"); });
        }

        [Test]
        public void CommandParseCommentException()
        {
            Assert.Catch(typeof(Exception), () => { var cmd = Command.Parse("G1;comment"); });
            
        }

        [Test]
        public void CommandParseEmptyStringException()
        {
            Assert.Catch(typeof(Exception), () => { var cmd = Command.Parse(""); });
        }

        [Test]
        public void CommandParseNoSubtypeException()
        {
            Assert.Catch(typeof(Exception), () => { var cmd = Command.Parse("G"); });
        }

        [Test]
        public void CommandParseWhitespace()
        {
            var cmd = Command.Parse("M \n 1  \t\t\t   X \t \n\n  23");
            Assert.IsTrue(cmd.CommandType == CommandType.M);
            Assert.IsTrue(cmd.CommandSubType == 1);
            Assert.IsTrue((decimal)cmd.GetParameterValue( ParameterType.X) == 23m);
        }

        [Test]
        public void CommandParseNoWhitespace()
        {
            var cmd = Command.Parse("M1X23YZ");
            Assert.IsTrue(cmd.CommandType == CommandType.M);
            Assert.IsTrue(cmd.CommandSubType == 1);
            Assert.IsTrue((decimal)cmd.GetParameterValue(ParameterType.X) == 23m);
            Assert.IsTrue(cmd.GetParameterValue(ParameterType.Y) == null);
            Assert.IsTrue(cmd.GetParameterValue(ParameterType.Z) == null);
        }

        [Test]
        public void CommandParseInvalidCommandTypeException()
        {
            Assert.Catch(typeof(Exception), () => { var cmd = Command.Parse("Z3"); });
        }

        [Test]
        public void CommandParseInvalidParameterException()
        {
            Assert.Catch(typeof(Exception), () => { var cmd = Command.Parse("M3 A"); });
        }

        [Test]
        public void CommandParseNoCommandTypeException()
        {
            Assert.Catch(typeof(Exception), () => { var cmd = Command.Parse("34"); });
        }

        [Test]
        public void CommandParseLeadingZeros()
        {
            var cmd = Command.Parse("G0003 X00020");
            Assert.IsTrue(cmd.CommandSubType == 3);
            Assert.IsTrue((decimal)cmd.GetParameterValue(ParameterType.X) == 20);
        }
    }
}
