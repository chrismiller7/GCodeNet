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
                //remove all existing Line number commands
                commands = commands.Where(c => c.CommandType != CommandType.N).ToArray();
            }

            int lineCounter = 1;
            foreach (var command in commands)
            {
                StringBuilder lineBuilder = new StringBuilder();
                if (options.WriteLineNumbers)
                {
                    lineBuilder.Append("N" + (lineCounter++) + " ");
                }
                lineBuilder.Append(command.ToGCode());
                if (options.WriteCRC)
                {
                    lineBuilder.Append("*" + CRC.Calculate(lineBuilder.ToString()));
                }
                writer.WriteLine(lineBuilder);
            }
        }
    }
}
