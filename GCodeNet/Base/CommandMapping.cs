using System;
using System.Linq;

namespace GCodeNet
{
    public abstract class CommandMapping : CommandBase
    {
        public override ParameterType[] GetParameters()
        {
            var reflectionData = CommandReflection.GetReflectionData(this.GetType());
            return reflectionData.MappedProperties.Keys.ToArray();
        }

        public override bool HasParameter(ParameterType parameter)
        {
            var reflectionData = CommandReflection.GetReflectionData(this.GetType());
            return reflectionData.MappedProperties.ContainsKey(parameter);
        }

        public override string GetParameterValue(ParameterType parameter)
        {
            var reflectionData = CommandReflection.GetReflectionData(this.GetType());
            var propInfo = reflectionData.MappedProperties[parameter];
            return propInfo.GetValue(this, null)?.ToString();
        }

        public override void SetParameterValue(ParameterType parameter, string value)
        {
            var reflectionData = CommandReflection.GetReflectionData(this.GetType());
            var propInfo = reflectionData.MappedProperties[parameter];

            var type = propInfo.PropertyType;
            bool isNullableType = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

            if (propInfo.PropertyType.Equals(typeof(bool)))
            {
                propInfo.SetValue(this, true, null);
            }
            else if (value != null)
            {
                if (type.Equals(typeof(byte)) || type.Equals(typeof(byte?)))
                {
                    propInfo.SetValue(this, byte.Parse(value), null);
                }
                else if (type.Equals(typeof(int)) || type.Equals(typeof(int?)))
                {
                    propInfo.SetValue(this, int.Parse(value), null);
                }
                else if (propInfo.PropertyType.Equals(typeof(double)) || propInfo.PropertyType.Equals(typeof(double?)))
                {
                    propInfo.SetValue(this, double.Parse(value), null);
                }
                else if (propInfo.PropertyType.Equals(typeof(decimal)) || propInfo.PropertyType.Equals(typeof(decimal?)))
                {
                    propInfo.SetValue(this, decimal.Parse(value), null);
                }
                else if (propInfo.PropertyType.IsEnum)
                {
                    var enumVal = Enum.ToObject(propInfo.PropertyType, int.Parse(value));
                    propInfo.SetValue(this, enumVal, null);
                }
                else
                {
                    throw new Exception("Command property type not supported: " + type);
                }
            }
        }

        public override void RemoveParameter(ParameterType parameter)
        {
            SetParameterValue(parameter, null);
        }

        public static CommandMapping Parse(string gcode)
        {
            var tokenizer = new GCodeTokenizer(gcode);
            var commands = tokenizer.GetCommands().ToArray();
            if (commands.Length != 1)
            {
                throw new Exception("gcode may only contain a single command");
            }
            return FromTokens(commands[0]);
        }

        public static CommandMapping Parse(Type mappedType, string gcode)
        {
            var tokenizer = new GCodeTokenizer(gcode);
            var commands = tokenizer.GetCommands().ToArray();
            if (commands.Length != 1)
            {
                throw new Exception("gcode may only contain a single command");
            }
            return FromTokens(mappedType, commands[0]);
        }

        public static new CommandMapping FromTokens(params string[] tokens)
        {
            var commandLetter = (CommandType)Enum.Parse(typeof(CommandType), tokens[0]);
            int commandNumber = int.Parse(tokens[1]);
            var type = CommandReflection.GetCommandObjectType(commandLetter, commandNumber);
            if (type == null)
            {
                throw new Exception("No mapping defined for these tokens");
            }
            return FromTokens(type, tokens);
        }

        public static CommandMapping FromTokens(Type mappedType, params string[] tokens)
        {
            var obj = (CommandMapping)Activator.CreateInstance(mappedType);
            obj.SetTokens(tokens);
            return obj;
        }
    }
}