using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace GCodeNet
{
    public static class CommandReflection
    {
        static Dictionary<Tuple<CommandType, int>, Type> typesLookup = new Dictionary<Tuple<CommandType, int>, Type>();

        static Dictionary<Type, CommandReflectionData> propsLookup = new Dictionary<Type, CommandReflectionData>();

        static CommandReflection()
        {
            var assembly = Assembly.GetExecutingAssembly();
            AddMappedTypesFromAssembly(assembly);
        }

        public static void ClearMappings()
        {
            typesLookup.Clear();
            propsLookup.Clear();
        }
        public static void AddMappedTypesFromAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(CommandMapping)))
                {
                    AddMappedType(type);
                }
            }
        }

        public static void AddMappedType(Type type)
        {
            if (!type.IsSubclassOf(typeof(CommandMapping)))
            {
                throw new Exception("Can only map a type derived from CommandMapping");
            }

            var gcommandAttrib = (CommandAttribute)type.GetCustomAttributes(typeof(CommandAttribute), true).SingleOrDefault();
            if (gcommandAttrib != null)
            {
                var key = new Tuple<CommandType, int>(gcommandAttrib.CommandType, gcommandAttrib.CommandSubType);
                typesLookup[key] = type;
                propsLookup[type] = new CommandReflectionData(type);
            }
        }

        public static Type GetCommandObjectType(CommandType cmdType, int cmdNum)
        {
            var key = new Tuple<CommandType, int>(cmdType, cmdNum);
            if (typesLookup.ContainsKey(key))
            {
                return typesLookup[key];
            }
            return null;
        }

        /*public static CommandBase CreateCommandObject(CommandType cmdType, int cmdNum)
        {
            var type = GetCommandObjectType(cmdType, cmdNum);
            if (type != null)
            {
                return (CommandBase)Activator.CreateInstance(type);
            }
            return new Command(cmdType, cmdNum);
        }*/

        public static CommandReflectionData GetMappedProperties(CommandType cmdType, int cmdNum)
        {
            var type = GetCommandObjectType(cmdType, cmdNum);
            return GetReflectionData(type);
        }

        public static CommandReflectionData GetReflectionData(Type type)
        {
            if (!propsLookup.ContainsKey(type))
            {
                AddMappedType(type);
            }

            if (propsLookup.ContainsKey(type))
            {
                return propsLookup[type];
            }
            throw new Exception("There is no mapped command for this type");
        }
    }

}