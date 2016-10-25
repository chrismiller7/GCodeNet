namespace GCodeNet.Commands
{
    [Command(CommandType.G, 92)]
    public class SetPosition : CommandMapping
    {
        [ParameterType("X")]
        public decimal? MoveX { get; set; }
        [ParameterType("Y")]
        public decimal? MoveY { get; set; }
        [ParameterType("Z")]
        public decimal? MoveZ { get; set; }
        [ParameterType("E")]
        public decimal? Extrude { get; set; }
    }
}