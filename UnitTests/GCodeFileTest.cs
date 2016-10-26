using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCodeNet;
using GCodeNet.Commands;

namespace TestProject
{
    [TestClass]
    public class GCodeFileTest
    {
        [TestMethod]
        public void MultipleCommands()
        {
            string gcode = "G0X1\nG0X2\nG0X3";
            GCodeFile file = new GCodeFile(gcode);
            Assert.IsTrue(file.Commands[0].GetParameterValue(ParameterType.X) == 1);
            Assert.IsTrue(file.Commands[1].GetParameterValue(ParameterType.X) == 2);
            Assert.IsTrue(file.Commands[2].GetParameterValue(ParameterType.X) == 3);
        }

        [TestMethod]
        public void MultipleCommandsOnSameLine()
        {
            string gcode = "G0X1G0X2G0X3";
            GCodeFile file = new GCodeFile(gcode);
            Assert.IsTrue(file.Commands[0].GetParameterValue(ParameterType.X) == 1);
            Assert.IsTrue(file.Commands[1].GetParameterValue(ParameterType.X) == 2);
            Assert.IsTrue(file.Commands[2].GetParameterValue(ParameterType.X) == 3);
        }

        [TestMethod]
        public void CommandsSplitUpOnDifferentLines()
        {
            string gcode = "G0\nX1G0\nX2\nG0\nX\n3";
            GCodeFile file = new GCodeFile(gcode);
            Assert.IsTrue(file.Commands[0].GetParameterValue(ParameterType.X) == 1);
            Assert.IsTrue(file.Commands[1].GetParameterValue(ParameterType.X) == 2);
            Assert.IsTrue(file.Commands[2].GetParameterValue(ParameterType.X) == 3);
        }

        [TestMethod]
        public void CommandsWithComments()
        {
            string gcode = "G0X1;G1X2\nG1X3";
            GCodeFile file = new GCodeFile(gcode);
            Assert.IsTrue(file.Commands[0].GetParameterValue(ParameterType.X) == 1);
            Assert.IsTrue(file.Commands[1].GetParameterValue(ParameterType.X) == 3);
        }

        [TestMethod]
        public void CommentsAtStartOfLine()
        {
            string gcode = ";comment\nG1X1\n;comment";
            GCodeFile file = new GCodeFile(gcode);
            Assert.IsTrue(file.Commands[0].GetParameterValue(ParameterType.X) == 1);
        }

        [TestMethod]
        public void CrcOnly()
        {
            GCodeFile file = new GCodeFile("*0");
            Assert.IsTrue(file.Commands.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void InvalidCrcOnly()
        {
            GCodeFile file = new GCodeFile("*2");
            Assert.IsTrue(file.Commands.Count == 0);
        }

        [TestMethod]
        public void CrcMultipleLines()
        {
            GCodeFile file = new GCodeFile("G1*118\nG2*117");
            Assert.IsTrue(file.Commands[0].CommandSubType == 1);
            Assert.IsTrue(file.Commands[1].CommandSubType == 2);
        }

        [TestMethod]
        public void CrcAfterComment()
        {
            GCodeFile file = new GCodeFile("G1;*118");
            Assert.IsTrue(file.Commands[0].CommandSubType == 1);
        }

        [TestMethod]
        public void CommentAfterCrc()
        {
            GCodeFile file = new GCodeFile("G1*118;comment");
            Assert.IsTrue(file.Commands[0].CommandSubType == 1);
        }

        [TestMethod]
        public void EmptyFile()
        {
            GCodeFile file = new GCodeFile("");
            Assert.IsTrue(file.Commands.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TwoCrcException()
        {
            GCodeFile file = new GCodeFile("G1*118*118");
            Assert.IsTrue(file.Commands.Count == 0);
        }

        [TestMethod]
        public void TwoCommentsSameLine()
        {
            GCodeFile file = new GCodeFile("G1;comment;comment");
            Assert.IsTrue(file.Commands[0].CommandSubType == 1);
        }

        [TestMethod]
        public void IgnoreCrcCheck()
        {
            GCodeFileOptions options = new GCodeFileOptions();
            options.CheckCRC = false;
            GCodeFile file = new GCodeFile("G1*0", options);
            Assert.IsTrue(file.Commands[0].CommandSubType == 1);
        }

        [TestMethod]
        public void LineNumbersInOrder()
        {
            GCodeFile file = new GCodeFile("N1N2N3");
            Assert.IsTrue(file.Commands.Count == 3);
        }

        [TestMethod]
        public void LineNumbersInOrder2()
        {
            GCodeFile file = new GCodeFile("N78N79N80");
            Assert.IsTrue(file.Commands.Count == 3);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void InvalidOrder()
        {
            GCodeFile file = new GCodeFile("N2N1N3");
        }

        [TestMethod]
        public void IgnoreInvalidOrder()
        {
            GCodeFileOptions options = new GCodeFileOptions();
            options.CheckLineNumers = false;
            GCodeFile file = new GCodeFile("N2N1N3", options);
            Assert.IsTrue(file.Commands.Count == 3);
        }

        [TestMethod]
        public void UseMappedObjects()
        {
            GCodeFileOptions options = new GCodeFileOptions();
            options.UseMappedObjects = true;
            GCodeFile file = new GCodeFile("G1X1", options);
            Assert.IsTrue(file.Commands[0] is RapidLinearMove);
        }

        [TestMethod]
        public void NotUseMappedObjects()
        {
            GCodeFileOptions options = new GCodeFileOptions();
            options.UseMappedObjects = false;
            GCodeFile file = new GCodeFile("G1X1", options);
            Assert.IsTrue(!(file.Commands[0] is RapidLinearMove));
        }

        [TestMethod]
        public void ExportWithNoLineNumbersOrCrc()
        {
            ExportFileOptions options = new ExportFileOptions();
            options.WriteCRC = false;
            options.WriteLineNumbers = false;
            GCodeFile file = new GCodeFile("G1X1G1X2");
            Assert.IsTrue(file.ToGCode(options) == "G1 X1 S0\r\nG1 X2 S0\r\n");
        }

        [TestMethod]
        public void ExportWithLineNumbersOnly()
        {
            ExportFileOptions options = new ExportFileOptions();
            options.WriteCRC = false;
            options.WriteLineNumbers = true;
            GCodeFile file = new GCodeFile("G1X1G1X2");
            Assert.IsTrue(file.ToGCode(options) == "N1 G1 X1 S0\r\nN2 G1 X2 S0\r\n");
        }

        [TestMethod]
        public void ExportWithCrcOnly()
        {
            ExportFileOptions options = new ExportFileOptions();
            options.WriteCRC = true;
            options.WriteLineNumbers = false;
            GCodeFile file = new GCodeFile("G1X1G1X2");
            Assert.IsTrue(file.ToGCode(options) == "G1 X1 S0*124\r\nG1 X2 S0*127\r\n");
        }

        [TestMethod]
        public void ExportWithLineNumbersAndCrc()
        {
            ExportFileOptions options = new ExportFileOptions();
            options.WriteCRC = true;
            options.WriteLineNumbers = true;
            GCodeFile file = new GCodeFile("G1X1G1X2");
            Assert.IsTrue(file.ToGCode(options) == "N1 G1 X1 S0*35\r\nN2 G1 X2 S0*35\r\n");
        }

        [TestMethod]
        public void RemoveOldLineNumbers()
        {
            ExportFileOptions options = new ExportFileOptions();
            options.WriteCRC = false;
            options.WriteLineNumbers = true;
            GCodeFile file = new GCodeFile("N5 G1X1 N6 G1X2");
            Assert.IsTrue(file.ToGCode(options) == "N1 G1 X1 S0\r\nN2 G1 X2 S0\r\n");
        }
    }
}
