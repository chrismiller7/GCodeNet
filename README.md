# GCodeNet
Import, export, and manipulate GCode for 3d printing.

[GCode format](http://reprap.org/wiki/G-code)

## Create a GCode command
```
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
```

## Create a command from GCode
```
var cmd = Command.Parse("G1 X10 Y20");

Console.WriteLine(cmd.CommandType); //Output: "G"
Console.WriteLine(cmd.CommandSubType); //Output: "1"
Console.WriteLine(cmd.GetParameterValue(ParameterType.X)); //Output: "10"
Console.WriteLine(cmd.GetParameterValue(ParameterType.Y)); //Output: "20"
```

## Create GCode using a mapped command object
```
var cmd = new RapidLinearMove();
cmd.MoveX = 10;
cmd.MoveY = 20;

Console.WriteLine(cmd.CommandType); //Output: "G"
Console.WriteLine(cmd.CommandSubType); //Output: "1"
Console.WriteLine(cmd.GetParameterValue(ParameterType.X)); //Output: "10"
Console.WriteLine(cmd.GetParameterValue(ParameterType.Y)); //Output: "20"

Console.WriteLine(cmd.ToGCode()); //Output: "G1 X10 Y20 S0"
```

## Create a mapped command object from GCode
```
var cmd = CommandMapping.Parse("G1 X10 Y20") as RapidLinearMove;

Console.WriteLine(cmd.CommandType); //Output: "G"
Console.WriteLine(cmd.CommandSubType); //Output: "1"
Console.WriteLine(cmd.MoveX); //Output: "10"
Console.WriteLine(cmd.MoveY); //Output: "20"
Console.WriteLine(cmd.ToGCode()); //Output: "G1 X10 Y20 S0"
```
