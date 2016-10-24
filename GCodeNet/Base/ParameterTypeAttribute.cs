using System;

namespace GCodeNet
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ParameterTypeAttribute : Attribute
    {
        public ParameterType Param { get; private set; }
        public ParameterTypeAttribute(ParameterType param)
        {
            this.Param = param;
        }

        public ParameterTypeAttribute(string param)
        {
            this.Param = (ParameterType)Enum.Parse(typeof(ParameterType), param);
        }
    }
}