using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace GCodeNet
{
    public class CommandReflectionData
    {
        public Type Type { get; private set; }

        public Dictionary<ParameterType, PropertyInfo> MappedProperties { get; private set; }

        public CommandReflectionData(Type type)
        {
            this.Type = type;

            MappedProperties = new Dictionary<ParameterType, PropertyInfo>();

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var attrib = (ParameterTypeAttribute)prop.GetCustomAttributes(typeof(ParameterTypeAttribute), true).SingleOrDefault();
                if (attrib != null)
                {
                    MappedProperties[attrib.Param] = prop;
                }
            }
        }
    }
}