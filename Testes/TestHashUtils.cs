using NUnit.Framework;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    class TestHashUtils
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void ShouldTextEncryptedEqualHash()
        {
            string text = "stefan novasky";

            string encrypted = new HashUtils().HashString(text);

            bool res = new HashUtils().CompareHash(text, encrypted);

            Assert.IsTrue(res);
        }
    }
}
