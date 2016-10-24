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
            CheckForIllegalChars(gcode);
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

        public IEnumerable<string[]> GetCommands()
        {
            List<string> tokens = new List<string>();
            foreach (var token in GetTokens())
            {
                if (token == "N" || token == "G" || token == "M")
                {
                    if (tokens.Count > 0)
                    {
                        yield return tokens.ToArray();
                    }
                    tokens.Clear();
                }
                tokens.Add(token);
            }

            if (tokens.Count > 0)
            {
                yield return tokens.ToArray();
            }
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