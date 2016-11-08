using System;
using System.Linq;
using System.Collections.Generic;

namespace GCodeNet
{
    public abstract class CommandMapping : CommandBase
    {
        protected CommandMapping()
        {
            var attrib = this.GetType().GetCustomAttributes(typeof(CommandAttribute), true).FirstOrDefault() as CommandAttribute;
            if (attrib == null)
            {
                throw new Exception("A mapped command must have the CommandAttribute attribute");
            }
            this.CommandType = attrib.CommandType;
            this.CommandSubType = attrib.CommandSubType;
        }

        public override IEnumerable<ParameterType> GetParameters()
        {
            var reflectionData = CommandReflection.GetReflectionData(this.GetType());
            foreach (var kv in reflectionData.MappedProperties)
            {
                var paramType = kv.Key;
                var propInfo = kv.Value;

                if (propInfo.PropertyType.Equals(typeof(bool)) && (bool)propInfo.GetValue(this, null) == true)
                {
                    yield return paramType;
                }
                else if (GetParameterValue(paramType) != null)
                {
                    yield return paramType;
                }
            }
        }

        public override bool HasParameter(ParameterType parameter)
        {
            var reflectionData = CommandReflection.GetReflectionData(this.GetType());
            return reflectionData.MappedProperties.ContainsKey(parameter);
        }

        public override object GetParameterValue(ParameterType parameter)
        {
            var reflectionData = CommandReflection.GetReflectionData(this.GetType());
            var propInfo = reflectionData.MappedProperties[parameter];
            var type = propInfo.PropertyType;

            if (type.Equals(typeof(bool)))
            {
                return null;
            }
            else if (type.IsEnum)
            {
                return (decimal)(int)propInfo.GetValue(this, null);
            }
            else if (type.IsGenericType && type.GetGenericArguments()[0].IsEnum)
            {
                var val = propInfo.GetValue(this, null);
                if (val == null) return null;
                return (decimal)(int)val;
            }
            else
            {
                var val = propInfo.GetValue(this, null);
                if (val == null) return null;
                var dec = (decimal)Convert.ChangeType(val, typeof(decimal));
                return dec;
            }
        }

        public override void SetParameterValue(ParameterType parameter, object value)
        {
            var reflectionData = CommandReflection.GetReflectionData(this.GetType());
            var propInfo = reflectionData.MappedProperties[parameter];

            var type = propInfo.PropertyType;
            bool isNullableType = type.Equals(typeof(string)) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));

            if (type.Equals(typeof(bool)) || type.Equals(typeof(bool?)))
            {
                propInfo.SetValue(this, true, null);
            }
            else if (value == null)
            {
                if (isNullableType)
                {
                    propInfo.SetValue(this, null, null);
                }
                else if (type.IsEnum)
                {
                    propInfo.SetValue(this,0, null);
                }
                else
                {
                    propInfo.SetValue(this, Convert.ChangeType(0, type), null);
                }
            }
            else if (value != null)
            {
                if (type.Equals(typeof(string)))
                {
                    propInfo.SetValue(this, value, null);
                }
                else if (type.Equals(typeof(byte)) || type.Equals(typeof(byte?)))
                {
                    propInfo.SetValue(this, Convert.ChangeType(value, typeof(byte)), null);
                }
                else if (type.Equals(typeof(int)) || type.Equals(typeof(int?)))
                {
                    propInfo.SetValue(this, Convert.ChangeType(value, typeof(int)), null);
                }
                else if (propInfo.PropertyType.Equals(typeof(double)) || propInfo.PropertyType.Equals(typeof(double?)))
                {
                    propInfo.SetValue(this, Convert.ChangeType(value, typeof(double)), null);
                }
                else if (propInfo.PropertyType.Equals(typeof(decimal)) || propInfo.PropertyType.Equals(typeof(decimal?)))
                {
                    propInfo.SetValue(this, Convert.ChangeType(value, typeof(decimal)), null);
                }
                else if (type.IsEnum)
                {
                    var enumVal = Enum.ToObject(type, Convert.ChangeType(value, typeof(int)));
                    propInfo.SetValue(this, enumVal, null);
                }
                else if (type.IsGenericType && type.GetGenericArguments()[0].IsEnum)
                {
                    var enumVal = Enum.ToObject(type.GetGenericArguments()[0], Convert.ChangeType(value, typeof(int)));
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

        public override void ClearAllParameters()
        {
            foreach (var key in GetParameters())
            {
                SetParameterValue(key, null);
            }
        }

        public static CommandMapping Parse(string gcode)
        {
            var tokenizer = new GCodeTokenizer(gcode);
            var commands = tokenizer.GetCommandTokens().ToArray();
            if (commands.Length != 1)
            {
                throw new Exception("gcode may only contain a single command");
            }
            return FromTokens(commands[0]);
        }

        public static CommandMapping Parse(Type mappedType, string gcode)
        {
            var tokenizer = new GCodeTokenizer(gcode);
            var commands = tokenizer.GetCommandTokens().ToArray();
            if (commands.Length != 1)
            {
                throw new Exception("gcode may only contain a single command");
            }
            return FromTokens(mappedType, commands[0]);
        }

        public static CommandMapping FromTokens(string[] tokens)
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

        public static CommandMapping FromTokens(Type mappedType, string[] tokens)
        {
            var obj = (CommandMapping)Activator.CreateInstance(mappedType);
            obj.SetTokens(tokens);
            return obj;
        }
    }
}