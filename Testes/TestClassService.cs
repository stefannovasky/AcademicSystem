using DAL.Impl;
using Entities;
using NUnit.Framework;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class TestClassService
    {
        [SetUp]
        public void Setup() { }

        private ClassRepository repo = new ClassRepository();

        [Test]
        public async Task ShouldCreateAClass()
        {
            Class s = (await new ClassRepository().GetAll()).Data[0];
            Instructor instructor = (await new InstructorRepository().GetAll()).Data[0];
            await repo.AddInstructor(s, instructor);
            Assert.IsTrue(true);
        }

        [Test]
        public async Task ShouldGetAllClasss()
        {
            DataResponse<Class> r = await repo.GetAll();

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldGetAClassByID()
        {
            DataResponse<Class> r = await repo.GetByID(1);

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldReturnClassNotFound()
        {
            DataResponse<Class> r = await repo.GetByID(287492);

            Assert.IsFalse(r.Success);
            Assert.AreEqual(expected: "User not found\r\n", actual: r.GetErrorMessage());
        }

        [Test]
        public async Task ShouldUpdateAClass()
        {
            Class u = (await repo.GetByID(1)).Data[0];
            Response r = await repo.Update(u);
            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldDeleteAClass()
        {
            Class u = (await repo.GetByID(1)).Data[0];

            Response r = await repo.Delete(u.ID);
            Assert.IsTrue(r.Success);
        }
    }
}
