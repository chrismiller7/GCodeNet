using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCodeNet;
using GCodeNet.Commands;
using System.Linq;

namespace TestProject
{
    [TestClass]
    public class M117Test
    {
        [TestMethod]
        public void M117CommandParse()
        {
            var cmd = CommandBase.Parse("M117 Hello World");
            Assert.IsTrue(cmd.CommandType == CommandType.M);
            Assert.IsTrue(cmd.CommandSubType == 117);
            Assert.IsTrue(cmd.ToGCode() == "M117 Hello World");
        }

        [TestMethod]
        public void M117CommandParseEmpty()
        {
            var cmd = CommandBase.Parse("M117");
            Assert.IsTrue(cmd.CommandType == CommandType.M);
            Assert.IsTrue(cmd.CommandSubType == 117);
            Assert.IsTrue(cmd.ToGCode() == "M117");
        }

        [TestMethod]
        public void M117CommandNoWhiteSpace()
        {
            var cmd = CommandBase.Parse("M117Hello");
            Assert.IsTrue(cmd.CommandType == CommandType.M);
            Assert.IsTrue(cmd.CommandSubType == 117);
            Assert.IsTrue(cmd.ToGCode() == "M117 Hello");
        }

        [TestMethod]
        public void M117CommandExtraWhiteSpace()
        {
            var cmd = CommandBase.Parse("   M117        Hello");
            Assert.IsTrue(cmd.CommandType == CommandType.M);
            Assert.IsTrue(cmd.CommandSubType == 117);
            Assert.IsTrue(cmd.ToGCode() == "M117 Hello");
        }

        [TestMethod]
        public void M117CommandParseMultiple()
        {
            var cmd = CommandBase.Parse("M117 Hello M117 Hello");
            Assert.IsTrue(cmd.CommandType == CommandType.M);
            Assert.IsTrue(cmd.CommandSubType == 117);
            Assert.IsTrue(cmd.ToGCode() == "M117 Hello M117 Hello");
        }

        [TestMethod]
        public void M117CommandParseInvalidChars()
        {
            var cmd = CommandBase.Parse("M117 Hello:?.+-';*");
            Assert.IsTrue(cmd.CommandType == CommandType.M);
            Assert.IsTrue(cmd.CommandSubType == 117);
            Assert.IsTrue(cmd.ToGCode() == "M117 Hello:?.+-';*");
        }

        [TestMethod]
        public void M117FileParserGBeforeM()
        {
            string gcode = "G1M117 Hello";
            GCodeFile file = new GCodeFile(gcode);
            Assert.IsTrue(file.Commands.Count ==2);
            Assert.IsTrue(file.Commands[0].CommandType == CommandType.G);
            Assert.IsTrue(file.Commands[1].CommandType == CommandType.M);
            Assert.IsTrue(file.Commands[1].ToGCode() == "M117 Hello");
        }

        [TestMethod]
        public void M117FileParserGAfterM()
        {
            string gcode = "M117G1 Hello";
            GCodeFile file = new GCodeFile(gcode);
            Assert.IsTrue(file.Commands.Count == 1);
            Assert.IsTrue(file.Commands[0].CommandType == CommandType.M);
            Assert.IsTrue(file.Commands[0].ToGCode() == "M117 G1 Hello");
        }

        [TestMethod]
        public void M117FileParserMultipleLines()
        {
            string gcode = "M117 Hello\r\nM117 World";
            GCodeFile file = new GCodeFile(gcode);
            Assert.IsTrue(file.Commands.Count == 2);
            Assert.IsTrue(file.Commands[0].CommandType == CommandType.M);
            Assert.IsTrue(file.Commands[0].ToGCode() == "M117 Hello");
            Assert.IsTrue(file.Commands[1].CommandType == CommandType.M);
            Assert.IsTrue(file.Commands[1].ToGCode() == "M117 World");
        }

        [TestMethod]
        public void M117FileParserWithComment()
        {
            string gcode = "M117 Hello;World";
            GCodeFile file = new GCodeFile(gcode);
            Assert.IsTrue(file.Commands.Count == 1);
            Assert.IsTrue(file.Commands[0].CommandType == CommandType.M);
            Assert.IsTrue(file.Commands[0].ToGCode() == "M117 Hello");
        }

        [TestMethod]
        public void M117FileParserWithCRC()
        {
            string gcode = "M117 Hello*24";
            GCodeFile file = new GCodeFile(gcode);
            Assert.IsTrue(file.Commands.Count == 1);
            Assert.IsTrue(file.Commands[0].CommandType == CommandType.M);
            Assert.IsTrue(file.Commands[0].ToGCode() == "M117 Hello");
        }
    }
}