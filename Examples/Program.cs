using System;
using GCodeNet;
using GCodeNet.Commands;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateGCodeCommand();
            CreateCommandFromGCode();
            GCodeFromMappedCommand();
            MappedCommandFromGCode();
        }

        static void CreateGCodeCommand()
        {
            //Create a G1 command (Rapid Linear Movement)
            var cmd = new Command(CommandType.G, 1);
            cmd.SetParameterValue(ParameterType.X, 10);
            cmd.SetParameterValue(ParameterType.Y, 20);

            //Convert to GCode
            Console.WriteLine(cmd.ToGCode()); //Output: "G1 X10 Y20"

            //Convert to GCode with the CRC
            Console.WriteLine(cmd.ToGCode(addCrc: true)); //Output: "G1 X10 Y20*116"

            //Convert to GCode with the CRC and a line number
            Console.WriteLine(cmd.ToGCode(addCrc: true, lineNumber: 4)); //Output: "N4 G1 X10 Y20*46"
        }

        static void CreateCommandFromGCode()
        {
            var cmd = Command.Parse("G1 X10 Y20");

            Console.WriteLine(cmd.CommandType); //Output: "G"
            Console.WriteLine(cmd.CommandSubType); //Output: "1"
            Console.WriteLine(cmd.GetParameterValue(ParameterType.X)); //Output: "10"
            Console.WriteLine(cmd.GetParameterValue(ParameterType.Y)); //Output: "20"
        }

        static void GCodeFromMappedCommand()
        {
            var cmd = new RapidLinearMove();
            cmd.MoveX = 10;
            cmd.MoveY = 20;

            Console.WriteLine(cmd.CommandType); //Output: "G"
            Console.WriteLine(cmd.CommandSubType); //Output: "1"
            Console.WriteLine(cmd.GetParameterValue(ParameterType.X)); //Output: "10"
            Console.WriteLine(cmd.GetParameterValue(ParameterType.Y)); //Output: "20"

            Console.WriteLine(cmd.ToGCode()); //Output: "G1 X10 Y20 S0"
        }

        static void MappedCommandFromGCode()
        {
            var cmd = CommandMapping.Parse("G1 X10 Y20") as RapidLinearMove;

            Console.WriteLine(cmd.CommandType); //Output: "G"
            Console.WriteLine(cmd.CommandSubType); //Output: "1"
            Console.WriteLine(cmd.MoveX); //Output: "10"
            Console.WriteLine(cmd.MoveY); //Output: "20"
            Console.WriteLine(cmd.ToGCode()); //Output: "G1 X10 Y20 S0"
        }
    }
}
