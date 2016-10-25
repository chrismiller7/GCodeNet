using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCodeNet
{
    public class ExportFileOptions
    {
        public bool WriteLineNumbers { get; set; } = true;
        public bool WriteCRC { get; set; } = true;
    }

    public static class ExportExtensions
    {
        public static void Export(this GCodeFile gcode, Stream outStream, ExportFileOptions options)
        {
            var writer = new StreamWriter(outStream);

            var commands = gcode.Commands.ToArray();

            if (options.WriteLineNumbers)
            {
                int lineCounter = 1;
                commands = RemoveAllLineNumbers(commands);
                foreach (var command in commands)
                {
                    writer.WriteLine(command.ToGCode(options.WriteCRC, lineCounter++));
                }
            }
            else
            {
                foreach (var command in commands)
                {
                    writer.WriteLine(command.ToGCode(options.WriteCRC));
                }
            }
        }

        static CommandBase[] RemoveAllLineNumbers(CommandBase[] commands)
        {
            return commands.Where(c => c.CommandType != CommandType.N).ToArray();
        }
    }
}
