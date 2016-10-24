namespace GCodeNet.Commands
{
    [Command(CommandType.G, 92)]
    public class SetPosition : CommandMapping
    {
        [ParameterType("X")]
        public double? MoveX { get; set; }
        [ParameterType("Y")]
        public bool MoveY { get; set; }
        [ParameterType("Z")]
        public double? MoveZ { get; set; }
        [ParameterType("E")]
        public double? Extrude { get; set; }
    }
}