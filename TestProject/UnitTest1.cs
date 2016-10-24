using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCodeNet;

namespace TestProject
{
    class IntClass : CommandMapping
    {
        [ParameterType(ParameterType.X)]
        public int X { get; set; }
        [ParameterType(ParameterType.Y)]
        public int? Y { get; set; }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var c1 = CommandMapping.FromTokens(typeof(IntClass), "G", "0", "X", "1");
            var c2 = CommandMapping.FromTokens(typeof(IntClass), "G", "0", "X", "1.1");
            var c3 = CommandMapping.FromTokens(typeof(IntClass), "G", "0", "X");
        }
    }
}
