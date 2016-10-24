namespace GCodeNet.Commands
{
    [Command(CommandType.G, 1)]
    public class RapidLinearMove : CommandMapping
    {
        [ParameterType("X")]
        public double? MoveX { get; set; }
        [ParameterType("Y")]
        public double? MoveY { get; set; }
        [ParameterType("Z")]
        public double? MoveZ { get; set; }
        [ParameterType("E")]
        public double? Extrude { get; set; }
        [ParameterType("F")]
        public double? Feedrate { get; set; }
        [ParameterType("S")]
        public CheckEndstop CheckEndstop { get; set; }
    }
}