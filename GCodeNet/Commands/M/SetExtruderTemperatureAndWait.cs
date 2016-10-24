namespace GCodeNet.Commands
{
    [Command(CommandType.M, 109)]
    public class SetExtruderTemperatureAndWait : CommandMapping
    {
        [ParameterType("S")]
        public int? MinTemperature { get; set; }
        [ParameterType("R")]
        public int? AccurateTargetTemperature { get; set; }
    }
}