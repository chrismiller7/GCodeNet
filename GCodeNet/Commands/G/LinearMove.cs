namespace GCodeNet.Commands
{
    [Command(CommandType.G, 0)]
    public class LinearMove: CommandMapping
    {
        [ParameterType("X")]
        public double? MoveX { get; set; }
        [ParameterType("Y")]
        public double MoveY { get; set; }
        [ParameterType("Z")]
        public double? MoveZ { get; set; }
        [ParameterType("E")]
        public double? Extrude { get; set; }
        [ParameterType("F")]
        public double? Feedrate { get; set; }
        [ParameterType("S")]
        public CheckEndstop CheckEndstop { get; set; }
    }

    public enum CheckEndstop
    {
        Ignore = 0,
        Check = 1,
    }


}
