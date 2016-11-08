using System.Text;

namespace GCodeNet.Commands.M
{
    [Command(CommandType.M, 117)]
    public class DisplayMessage : CommandMapping
    {
        [ParameterType("D")]
        public string Message { get; set; }

        public override string ToGCode(bool addCrc = false, int lineNumber = -1)
        {
            StringBuilder sb = new StringBuilder();

            if (lineNumber > -1)
            {
                sb.Append("N" + lineNumber + " ");
            }
            sb.Append(this.CommandType);
            sb.Append(this.CommandSubType);
            if (!string.IsNullOrEmpty(Message))
            {
                sb.Append(" " + Message);
            }
            if (addCrc)
            {
                sb.Append("*" + CRC.Calculate(sb.ToString()));
            }
            return sb.ToString();
        }
    }
}
