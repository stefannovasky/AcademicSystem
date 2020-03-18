using NUnit.Framework;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class TestHashUtils
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public async Task ShouldTextEncryptedEqualHash()
        {
            string text = "stefan novasky";

            string encrypted = await new HashUtils().HashString(text);

            bool res = await new HashUtils().CompareHash(text, encrypted);

            Assert.IsTrue(res);
        }
    }
}
