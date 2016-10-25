# GCodeNet
Import, export, and manipulate GCode for 3d printing.

[GCode format](http://reprap.org/wiki/G-code)

## Create gcode using a mapped command object
```
var cmd = new RapidLinearMove();
cmd.MoveX = 10;
cmd.MoveY = 20;

Console.WriteLine(cmd.CommandType); // G
Console.WriteLine(cmd.CommandSubType); // 1
Console.WriteLine(cmd.GetParameterValue(ParameterType.X)); // 10
Console.WriteLine(cmd.GetParameterValue(ParameterType.Y)); // 20

//Convert object to GCode
Console.WriteLine(cmd.ToGCode()); // G1 X10 Y20 S0

//Convert object to GCode with CRC
Console.WriteLine(cmd.ToGCode(addCrc: true)); // G1 X10 Y20 S0*55

//Convert object to GCode with CRC and a line number
Console.WriteLine(cmd.ToGCode(addCrc: true, lineNumber: 4)); // N4 G1 X10 Y20 S0*109
```

## Create a mapped command object from GCode:
```
var cmd = CommandMapping.Parse("G1 X10 Y20") as RapidLinearMove;

Console.WriteLine(cmd.CommandType); // G
Console.WriteLine(cmd.CommandSubType); // 1
Console.WriteLine(cmd.MoveX); // 10
Console.WriteLine(cmd.MoveY); // 20
Console.WriteLine(cmd.ToGCode()); // G1 X10 Y20 S0
```

## Create a gcode command manually
```
var cmd = new Command(CommandType.G, 1);
cmd.SetParameterValue(ParameterType.X, 10);
cmd.SetParameterValue(ParameterType.Y, 20);

Console.WriteLine(cmd.ToGCode()); // G1 X10 Y20
```

## Create a command from GCode without mapping to a mapped object
```
var cmd = Command.Parse("G1 X10 Y20");

Console.WriteLine(cmd.GetParameterValue(ParameterType.X)); // 10
Console.WriteLine(cmd.GetParameterValue(ParameterType.Y)); // 20
```
