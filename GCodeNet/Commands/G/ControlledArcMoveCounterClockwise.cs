namespace GCodeNet.Commands
{
    [Command(CommandType.G, 3)]
    public class ControlledArcMoveCounterClockwise : CommandMapping
    {
        [ParameterType("X")]
        public decimal? MoveX { get; set; }
        [ParameterType("Y")]
        public decimal? MoveY { get; set; }
        [ParameterType("I")]
        public decimal? CenterX { get; set; }
        [ParameterType("J")]
        public decimal? CenterY { get; set; }
        [ParameterType("E")]
        public decimal? Extrude { get; set; }
        [ParameterType("F")]
        public decimal? Feedrate { get; set; }
    }
}