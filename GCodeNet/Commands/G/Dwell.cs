namespace GCodeNet.Commands
{
    [Command(CommandType.G, 4)]
    public class Dwell : CommandMapping
    {
        [ParameterType("P")]
        public double? WaitInMSecs { get; set; }
        [ParameterType("S")]
        public double? WaitInSecs { get; set; }
    }
}