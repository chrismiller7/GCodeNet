using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCodeNet;
using GCodeNet.Commands;
using System.IO;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var file1 = LoadFile(@"e:\sd_card_holder_fish-4.gcode", new GCodeFileOptions());
            file1.Export(new FileStream(@"e:\out.gcode", FileMode.CreateNew), new ExportFileOptions());

            var objs = file1.Commands.OfType<Command>().ToArray();

            GCodeFileOptions options = new GCodeFileOptions();
            options.UseMappedObjects = false;
            var file2 = LoadFile(@"e:\sd_card_holder_fish-4.gcode", options);
        }

        static GCodeFile LoadFile(string file, GCodeFileOptions options)
        {
            using (var stream = new FileStream(file, FileMode.Open))
            {
                return new GCodeFile(stream, options);
            }
        }
    }
}
