using BLL.Impl;
using DAL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class AssemblyIntegrationTest
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void TesteAssembly()
        {
            Type[] InterfacesTypes = Assembly.GetAssembly(typeof(AttendanceService)).GetTypes().Where(c => c.IsInterface && c.Name.Contains("Service") && !c.Name.Contains("IService")).ToArray();
            Type[] ClassesTypes = Assembly.GetAssembly(typeof(AttendanceService)).GetTypes().Where(c => c.IsClass && c.Name.Contains("Service")).ToArray();
            Assert.IsTrue(true);
        }
    }
}
