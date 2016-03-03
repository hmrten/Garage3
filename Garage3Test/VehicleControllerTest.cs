using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Garage3.Controllers;
using System.Collections.Generic;
using System.Diagnostics;

namespace Garage3Test
{
    [TestClass]
    public class VehicleControllerTest
    {
        [TestMethod]
        public void TestVehicleTypes()
        {
            using (var vc = new VehicleController())
            {
                var actual = vc.Types(null).Data as List<dynamic>;
                Assert.AreEqual(4, actual.Count);
                Assert.AreEqual("Car", actual[0].name);
                Assert.AreEqual("Motorcycle", actual[1].name);
                Assert.AreEqual("Truck", actual[2].name);
                Assert.AreEqual("Bus", actual[3].name);
            }
        }

        [TestMethod]
        public void TestVehicleOwners()
        {
            using (var vc = new VehicleController())
            {
                var actual = vc.Owners(null).Data as List<dynamic>;
                Assert.AreEqual("Bob", actual[0].name);
                Assert.AreEqual("John", actual[1].name);
                Assert.AreEqual("Eve", actual[2].name);
            }
        }

        [TestMethod]
        public void TestvehicleList()
        {
            using (var vc = new VehicleController())
            {
                var actual = vc.List(null).Data as List<dynamic>;
                Assert.AreEqual("AAA111", actual[0].reg);
                Assert.AreEqual("AAA222", actual[1].reg);
                Assert.AreEqual("AAA333", actual[2].reg);
                Assert.AreEqual("BBB222", actual[3].reg);
                Assert.AreEqual("BBB333", actual[4].reg);
                Assert.AreEqual("CCC333", actual[5].reg);
                Assert.AreEqual("CCC444", actual[6].reg);
                Assert.AreEqual("DDD444", actual[7].reg);
                Assert.AreEqual("DDD555", actual[8].reg);
            }
        }
    }
}
