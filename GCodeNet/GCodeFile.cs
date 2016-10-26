using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace GCodeNet
{
    public class GCodeFile
    {
        public List<CommandBase> Commands = new List<CommandBase>();

        public GCodeFile(string gcode) : this(gcode, new GCodeFileOptions())
        { 
        }

        public GCodeFile(string gcode, GCodeFileOptions options)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(gcode);
            writer.Flush();
            stream.Position = 0;
            Init(stream, options);
        }

        public GCodeFile(Stream stream)
        {
            Init(stream, new GCodeFileOptions());
        }

        public GCodeFile(Stream stream, GCodeFileOptions options)
        {
            Init(stream, options);
        }

        void Init(Stream stream, GCodeFileOptions options)
        {
            var gcodeLines = GetAllGCodeLines(stream).ToArray();

            if (options.CheckCRC)
            {
                CheckCRC(gcodeLines);
            }

            var gcodeString = string.Join("", gcodeLines.Select(l => l.GCode));
            var tokenizer = new GCodeTokenizer(gcodeString);
            var commandTokens = tokenizer.GetCommandTokens().ToArray();

            this.Commands = commandTokens.Select(c => CreateCommandFromTokens(c, options.UseMappedObjects)).ToList();

            if (options.CheckLineNumers)
            {
                CheckLineNumbers(this.Commands);
            }
        }

        IEnumerable<GCodeFileLine> GetAllGCodeLines(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var lineStr = reader.ReadLine();
                var line = new GCodeFileLine(lineStr);
                yield return line;
            }
        }

        void CheckCRC(GCodeFileLine[] gcodeLines)
        {
            for (int i = 0; i < gcodeLines.Length; i++)
            {
                if (!gcodeLines[i].IsChecksumValid)
                {
                    var expectedCrc = CRC.Calculate(gcodeLines[i].GCode);
                    throw new Exception($"Checksum is invalid on line {i+1}:  {gcodeLines[i].OriginalString}, Expected CRC: {expectedCrc}");
                }
            }
        }

        void CheckLineNumbers(IEnumerable<CommandBase> commands)
        {
            var lineNumCommands = commands.Where(c => c.CommandType == CommandType.N).ToArray();
            for (int i=0; i<lineNumCommands.Length-1; i++)
            {
                var lineNum1 = lineNumCommands[i].CommandSubType;
                var lineNum2 = lineNumCommands[i+1].CommandSubType;

                if (lineNum1 != lineNum2-1)
                {
                    throw new Exception("Line numbers are out of order");
                }
            }
        }

        CommandBase CreateCommandFromTokens(string[] cmdTokens, bool useMappedObjects)
        {
            if (useMappedObjects)
            {
                return CommandBase.FromTokens(cmdTokens);
            }
            else
            {
                return Command.FromTokens(cmdTokens);
            }
        }
    }
}
