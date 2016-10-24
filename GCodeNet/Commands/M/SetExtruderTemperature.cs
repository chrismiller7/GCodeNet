namespace GCodeNet.Commands
{
    [Command(CommandType.M, 104)]
    public class SetExtruderTemperature : CommandMapping
    {
        [ParameterType("S")]
        public int? Temperature { get; set; }
    }
}