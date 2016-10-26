using System.Text;

namespace GCodeNet
{
    internal class GCodeFileLine
    {
        public string OriginalString { get; private set; }
        public string Comment { get; private set; }
        public int? Checksum { get; private set; }
        public string GCode { get; private set; }

        public bool IsChecksumValid { get; private set; }

        public GCodeFileLine(string line)
        {
            OriginalString = line;
            line = GetComment(line);
            line = GetChecksum(line);
            ValidateChecksum(line);
            GCode = line;
        }

        string GetComment(string str)
        {
            var idx = str.IndexOf(';');
            if (idx >= 0)
            {
                Comment = str.Substring(idx + 1);
                return str.Substring(0, idx);
            }
            Comment = "";
            return str;
        }

        void ValidateChecksum(string line)
        {
            if (Checksum == null)
            {
                IsChecksumValid = true;
                return;
            }

            IsChecksumValid = Checksum == CRC.Calculate(line);
        }

        string GetChecksum(string str)
        {
            var idx = str.LastIndexOf('*');
            if (idx >= 0)
            {
                Checksum = int.Parse(str.Substring(idx+1));
                return str.Substring(0, idx);
            }
            Checksum = null;
            return str;
        }
    }
}