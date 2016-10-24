namespace GCodeNet.Commands
{
    [Command(CommandType.M, 106)]
    public class FanOn : CommandMapping
    {
        [ParameterType("P")]
        public int? FanNumber { get; set; }
        [ParameterType("S")]
        public double FanSpeed { get; set; }
        [ParameterType("I")]
        public int? Invert { get; set; }
        [ParameterType("F")]
        public int? FanPwmFreq { get; set; }
        [ParameterType("H")]
        public int? MonitorHeaters { get; set; }
        [ParameterType("R")]
        public int? RestoreFanSpeed { get; set; }
        [ParameterType("T")]
        public int? Temperature { get; set; }
    }
}