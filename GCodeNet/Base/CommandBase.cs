using System;
using System.Text;
using System.Collections.Generic;

namespace GCodeNet
{
    public abstract class CommandBase
    {
        public CommandType CommandType { get; set; }
        public int CommandSubType { get; set; }
        public abstract IEnumerable<ParameterType> GetParameters();
        public abstract bool HasParameter(ParameterType parameter);
        public abstract string GetParameterValue(ParameterType parameter);
        public abstract void SetParameterValue(ParameterType parameter, string value);
        public abstract void RemoveParameter(ParameterType parameter);

        internal void SetTokens(string[] tokens)
        {
            this.CommandType = (CommandType)Enum.Parse(typeof(CommandType), tokens[0]);
            this.CommandSubType = int.Parse(tokens[1]);

            for (int i = 2; i < tokens.Length; i++)
            {
                var paramType = (ParameterType)Enum.Parse(typeof(ParameterType), tokens[i]);
                string value = null;
                if (tokens.Length > i + 1 && !char.IsLetter(tokens[i + 1][0]))
                {
                    value = tokens[++i];
                }
                SetParameterValue(paramType, value);
            }
        }

        public static CommandBase FromTokens(params string[] tokens)
        {
            var commandLetter = (CommandType)Enum.Parse(typeof(CommandType), tokens[0]);
            int commandNumber = int.Parse(tokens[1]);
            var type = CommandReflection.GetCommandObjectType(commandLetter, commandNumber);
            if (type == null)
            {
                return Command.FromTokens(tokens);
            }
            else
            {
                return CommandMapping.FromTokens(tokens);
            }
        }

        public string ToGCode(bool addCrc = false, int lineNumber = -1)
        {
            StringBuilder sb = new StringBuilder();

            if (lineNumber > -1)
            {
                if (CommandType == CommandType.N)
                {
                    throw new Exception("Can't add a line number on a line number command type");
                }
                sb.Append("N" + lineNumber + " ");
            }
            sb.Append(this.CommandType);
            sb.Append(this.CommandSubType);
            foreach (var param in this.GetParameters())
            {
                sb.Append(" " + param);
                var val = this.GetParameterValue(param);
                if (val != null)
                {
                    sb.Append(val);
                }
            }
            if (addCrc)
            {
                sb.Append("*" + CRC.Calculate(sb.ToString()));
            }
            return sb.ToString();
        }
    }
}