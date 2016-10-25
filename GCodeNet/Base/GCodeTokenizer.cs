using System;
using System.Collections.Generic;
using System.Text;

namespace GCodeNet
{
    class GCodeTokenizer
    {
        string gcode;

        public GCodeTokenizer(string gcode)
        {
            this.gcode = RemoveWhitespace(gcode);
            CheckForIllegalChars(this.gcode);
            this.gcode = this.gcode.ToUpper();
        }

        void CheckForIllegalChars(string gcode)
        {
            HashSet<char> list = new HashSet<char>();
            for (char c='a'; c<='z';c++)
            {
                list.Add(c);
            }
            for (char c = 'A'; c <= 'Z'; c++)
            {
                list.Add(c);
            }
            for (char c = '0'; c <= '9'; c++)
            {
                list.Add(c);
            }
            list.Add('.');
            list.Add('+');
            list.Add('-');
            list.Add(':');

            foreach (var c in gcode)
            {
                if (!list.Contains(c))
                {
                    throw new System.Exception($"Illegal character in gcode: '{c}'");
                }
            }
        }

        string RemoveWhitespace(string str)
        {
            //this.gcode = Regex.Replace(gcode, @"\s+", ""); This is a bit slower
            StringBuilder sb = new StringBuilder();
            foreach (var c in str)
            {
                if (!char.IsWhiteSpace(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public IEnumerable<string[]> GetCommandTokens()
        {
            bool isFirstToken = true;

            List<string> tokens = new List<string>();
            foreach (var token in GetTokens())
            {
                if (isFirstToken)
                {
                    if (!IsValidCommandType(token))
                    {
                        throw new Exception("The first token must be a valid CommandType: G,M,N, etc");
                    }
                    isFirstToken = false;
                }

                if (!IsValidParameter(token) && !IsValidCommandType(token) && !IsParameterValue(token))
                {
                    throw new Exception("Invalid token: " + token);
                }

                if (IsValidCommandType(token))
                {
                    if (tokens.Count == 1)
                    {
                        throw new Exception("A command subtype must always follow a command type.");
                    }
                    else if (tokens.Count > 1)
                    {
                        yield return tokens.ToArray();
                    }
                    tokens.Clear();
                }
                tokens.Add(token);
            }

            if (tokens.Count == 1)
            {
                throw new Exception("A command subtype must always follow a command type.");
            }
            else if (tokens.Count > 1)
            {
                yield return tokens.ToArray();
            }
        }

        bool IsValidCommandType(string token)
        {
            return Enum.IsDefined(typeof(CommandType), token);
        }

        bool IsValidParameter(string token)
        {
            return Enum.IsDefined(typeof(ParameterType), token);
        }

        bool IsParameterValue(string token)
        {
            decimal tmp;
            return decimal.TryParse(token, out tmp);
        }

        public IEnumerable<string> GetTokens()
        {
            StringBuilder buff = new StringBuilder();

            for(int i=0; i< gcode.Length; i++)
            {
                if (char.IsLetter(gcode[i]))
                {
                    if (buff.Length > 0)
                    {
                        yield return buff.ToString();
                    }
                    buff.Clear();
                    yield return gcode[i].ToString();
                }
                else
                {
                    buff.Append(gcode[i]);
                }
            }

            if (buff.Length > 0)
            {
                yield return buff.ToString();
            }
        }
    }
}