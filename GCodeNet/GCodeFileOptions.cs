namespace GCodeNet
{
    public class GCodeFileOptions
    {
        public bool CheckCRC { get; set; } = true;
        public bool CheckLineNumers { get; set; } = true;
        public bool UseMappedObjects { get; set; } = true;
    }
}
