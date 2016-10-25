namespace GCodeNet.Commands
{
    [Command(CommandType.G, 0)]
    public class LinearMove: CommandMapping
    {
        [ParameterType("X")]
        public decimal? MoveX { get; set; }
        [ParameterType("Y")]
        public decimal? MoveY { get; set; }
        [ParameterType("Z")]
        public decimal? MoveZ { get; set; }
        [ParameterType("E")]
        public decimal? Extrude { get; set; }
        [ParameterType("F")]
        public decimal? Feedrate { get; set; }
        [ParameterType("S")]
        public CheckEndstop CheckEndstop { get; set; }
    }

    public enum CheckEndstop
    {
        Ignore = 0,
        Check = 1,
    }


}
