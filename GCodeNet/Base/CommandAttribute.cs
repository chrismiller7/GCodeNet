using System;

namespace GCodeNet
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public CommandType CommandType { get; private set; }
        public int CommandSubType { get; private set; }

        public CommandAttribute(CommandType commandType, int commandSubType)
        {
            this.CommandType = commandType;
            this.CommandSubType = commandSubType;
        }
    }
}