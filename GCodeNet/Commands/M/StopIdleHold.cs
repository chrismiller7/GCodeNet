namespace GCodeNet.Commands
{
    [Command(CommandType.M, 84)]
    public class StopIdleHold : CommandMapping
    {
        [ParameterType("I")]
        public int? ResetFlags { get; set; }
    }
}