using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCodeNet
{
    public class Command : CommandBase
    {
        Dictionary<ParameterType, object> parameters = new Dictionary<ParameterType, object>();

        private Command() { }

        public Command(CommandType type, int subType)
        {
            this.CommandType = type;
            this.CommandSubType = subType;
        }

        public override IEnumerable<ParameterType> GetParameters()
        {
            return parameters.Keys.ToArray();
        }

        public override object GetParameterValue(ParameterType parameter)
        {
            if (parameters.ContainsKey(parameter))
            {
                return parameters[parameter];
            }
            return null;
        }

        public override bool HasParameter(ParameterType parameter)
        {
            return parameters.ContainsKey(parameter);
        }

        public override void RemoveParameter(ParameterType parameter)
        {
            parameters.Remove(parameter);
        }

        public override void SetParameterValue(ParameterType parameter, object value)
        {
            parameters[parameter] = value;
        }

        public static Command Parse(string gcode)
        {
            var tokenizer = new GCodeTokenizer(gcode);
            var commands = tokenizer.GetCommandTokens().ToArray();
            if (commands.Length != 1)
            {
                throw new Exception("gcode may only contain a single command");
            }
            return FromTokens(commands[0]);
        }

        public static Command FromTokens(params string[] tokens)
        {
            var obj = new Command();
            obj.SetTokens(tokens);
            return obj;
        }

        public override string ToString()
        {
            return this.ToGCode();
        }

        public override void ClearAllParameters()
        {
            parameters.Clear();
        }
    }
}
