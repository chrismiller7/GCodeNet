using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace GCodeNet
{
    public abstract class CommandBase : IDictionary<ParameterType, object>
    {
        public CommandType CommandType { get; set; }
        public int CommandSubType { get; set; }

        public abstract IEnumerable<ParameterType> GetParameters();
        public abstract bool HasParameter(ParameterType parameter);
        public abstract object GetParameterValue(ParameterType parameter);
        public abstract void SetParameterValue(ParameterType parameter, object value);
        public abstract void RemoveParameter(ParameterType parameter);
        public abstract void ClearAllParameters();

        internal void SetTokens(string[] tokens)
        {
            this.CommandType = (CommandType)Enum.Parse(typeof(CommandType), tokens[0]);
            this.CommandSubType = int.Parse(tokens[1]);

            int i = 2;
            while (i < tokens.Length)
            {
                var paramType = (ParameterType)Enum.Parse(typeof(ParameterType), tokens[i++]);
                object value = null;
                if (tokens.Length > i && !char.IsLetter(tokens[i][0]))
                {
                    if (tokens[i][0] == '"')
                    {
                        value = tokens[i++].Trim('"');
                    }
                    else
                    {
                        value = decimal.Parse(tokens[i++]);
                    }
                }
                this.SetParameterValue(paramType, value);
            }
        }

        public static CommandBase Parse(string gcode, bool useMappedCommands = true)
        {
            var tokenizer = new GCodeTokenizer(gcode);
            var commands = tokenizer.GetCommandTokens().ToArray();
            if (commands.Length != 1)
            {
                throw new Exception("gcode may only contain a single command");
            }
            return FromTokens(commands[0], useMappedCommands);
        }

        public static CommandBase FromTokens(string[] tokens, bool useMappedCommands = true)
        {
            if (useMappedCommands)
            {
                var commandLetter = (CommandType)Enum.Parse(typeof(CommandType), tokens[0]);
                int commandNumber = int.Parse(tokens[1]);
                var type = CommandReflection.GetCommandObjectType(commandLetter, commandNumber);
                if (type != null)
                {
                     return CommandMapping.FromTokens(tokens);
                }
            }
            return Command.FromTokens(tokens);
        }

        public virtual string ToGCode(bool addCrc = false, int lineNumber = -1)
        {
            StringBuilder sb = new StringBuilder();

            if (lineNumber > -1)
            {
                if (this.CommandType == CommandType.N)
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

        public ICollection<ParameterType> Keys
        {
            get
            {
                return (ICollection<ParameterType>)GetParameters();
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return (ICollection<object>)GetParameters().Select(k => GetParameterValue(k));
            }
        }

        public int Count
        {
            get
            {
                return GetParameters().Count();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public object this[ParameterType key]
        {
            get
            {
                return GetParameterValue(key);
            }

            set
            {
                SetParameterValue(key, value);
            }
        }
        public void Add(ParameterType key, object value)
        {
            if (HasParameter(key))
            {
                throw new Exception("Command already this ParameterType");
            }
            SetParameterValue(key, value);
        }

        public bool ContainsKey(ParameterType key)
        {
            return HasParameter(key);
        }

        public bool Remove(ParameterType key)
        {
            if (HasParameter(key))
            {
                RemoveParameter(key);
                return true;
            }
            return false;
        }

        public bool TryGetValue(ParameterType key, out object value)
        {
            if (HasParameter(key))
            {
                value = GetParameterValue(key);
                return true;
            }
            value = null;
            return false;
        }

        public void Add(KeyValuePair<ParameterType, object> item)
        {
            SetParameterValue(item.Key, item.Value);
        }

        public void Clear()
        {
            ClearAllParameters();
        }

        public bool Contains(KeyValuePair<ParameterType, object> item)
        {
            foreach (var key in GetParameters())
            {
                var val = GetParameterValue(key);
                if (key == item.Key && val.Equals(item.Value))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(KeyValuePair<ParameterType, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<ParameterType, object> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<ParameterType, object>> GetEnumerator()
        {
            foreach (var key in GetParameters())
            {
                var val = GetParameterValue(key);
                yield return new KeyValuePair<ParameterType, object>(key, val);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}