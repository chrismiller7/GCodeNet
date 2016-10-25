namespace GCodeNet.Commands
{
    [Command(CommandType.G, 4)]
    public class Dwell : CommandMapping
    {
        [ParameterType("P")]
        public decimal? WaitInMSecs { get; set; }
        [ParameterType("S")]
        public decimal? WaitInSecs { get; set; }
    }
}