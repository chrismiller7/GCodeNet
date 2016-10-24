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

            int currentLineNum = 0;
            bool isFirstLineNum = true;

            var commands = tokenizer.GetCommands().ToArray();
            foreach (var cmdTokens in commands)
            {
                var obj = FromTokens(cmdTokens, options.UseMappedObjects);
                if (obj.CommandType == CommandType.N)
                {
                    if (options.CheckLineNumers)
                    {
                        if (isFirstLineNum)
                        {
                            currentLineNum = obj.CommandSubType;
                            isFirstLineNum = false;
                        }
                        else
                        {
                            currentLineNum++;
                            if (obj.CommandSubType != currentLineNum )
                            {
                                throw new Exception($"Line numer out of order");
                            }
                        }
                    }
                }
                else
                {
                    Commands.Add(obj);
                }
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
                    throw new Exception($"Checksum is invalid on line {i+1}:  {gcodeLines[i].OriginalString}");
                }
            }
        }

        CommandBase FromTokens(string[] cmdTokens, bool useMappedObjects)
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
