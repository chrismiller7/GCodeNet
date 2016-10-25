using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCodeNet;
using GCodeNet.Commands;
using System.Linq;

namespace TestProject
{
    class MissingAttributeClass : CommandMapping
    {
    }

    [TestClass]
    public class MappedCommand
    {
        [TestMethod]
        public void RapidLinearMoveTest()
        {
            var cmd = new RapidLinearMove();
            cmd.MoveX = 1;
            cmd.MoveY = 1.2m;

            Assert.IsTrue(cmd.CommandType == CommandType.G);
            Assert.IsTrue(cmd.CommandSubType == 1);

            var parameters = cmd.GetParameters().ToArray();
            Assert.IsTrue(parameters[0] == ParameterType.X);
            Assert.IsTrue(parameters[1] == ParameterType.Y);
            Assert.IsTrue(parameters[2] == ParameterType.S);

            Assert.IsTrue(cmd.GetParameterValue(ParameterType.X) == 1);
            Assert.IsTrue(cmd.GetParameterValue(ParameterType.Y) == 1.2m);
            Assert.IsTrue(cmd.GetParameterValue(ParameterType.S) == 0);

            Assert.IsTrue(cmd.ToGCode() == "G1 X1 Y1.2 S0");
        }

        [TestMethod]
        public void SetExtruderTemperatureTest()
        {
            var cmd = new SetExtruderTemperature();
            cmd.Temperature = 98;

            Assert.IsTrue(cmd.CommandType == CommandType.M);
            Assert.IsTrue(cmd.CommandSubType == 104);

            var parameters = cmd.GetParameters().ToArray();
            Assert.IsTrue(parameters[0] == ParameterType.S);
            Assert.IsTrue(cmd.GetParameterValue(ParameterType.S) == 98);

            Assert.IsTrue(cmd.ToGCode() == "M104 S98");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void MissingCommandAttribute()
        {
            MissingAttributeClass cmd = new MissingAttributeClass();
        }
    }
}
