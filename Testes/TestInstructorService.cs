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
    class TestInstructorService
    {
        [SetUp]
        public void Setup() { }

        private InstructorRepository repo = new InstructorRepository();

        [Test]
        public async Task ShouldCreateAInstructor()
        {
            User u = (await new UserRepository().GetAll()).Data[0];

            Instructor s = new Instructor { UserID = u.ID };

            Response r = await repo.Create(s);
            Assert.IsTrue(r.Success);
        }

        [Test]
        public async Task ShouldGetAllInstructors()
        {
            DataResponse<Instructor> r = await repo.GetAll();

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldGetAInstructorByID()
        {
            DataResponse<Instructor> r = await repo.GetByID(4);

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldReturnInstructorNotFound()
        {
            DataResponse<Instructor> r = await repo.GetByID(287492);

            Assert.IsFalse(r.Success);
            Assert.AreEqual(expected: "User not found\r\n", actual: r.GetErrorMessage());
        }

        [Test]
        public async Task ShouldUpdateAInstructor()
        {
            Instructor u = (await repo.GetByID(4)).Data[0];
            u.User.Name = "Updated in Instructor";

            Response r = await repo.Update(u);
            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldDeleteAInstructor()
        {
            Instructor u = (await repo.GetByID(6)).Data[0];
            u.User.IsActive = false;

            Response r = await repo.Delete(u.ID);
            Assert.IsTrue(r.Success);
        }
    }
}
