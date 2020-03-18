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
    class TestStudentService
    {
        [SetUp]
        public void Setup() { }

        private StudentRepository repo = new StudentRepository(); 

        [Test]
        public async Task ShouldCreateAStudent()
        {
            User u = (await new UserRepository().GetAll()).Data[1];

            Student s = new Student { UserID = u.ID }; 

            Response r = await repo.Create(s);
            Assert.IsTrue(r.Success);
        }

        [Test]
        public async Task ShouldGetAllStudents()
        {
            DataResponse<Student> r = await repo.GetAll();

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldGetAStudentByID()
        {
            DataResponse<Student> r = await repo.GetByID(8);

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldReturnStudentNotFound()
        {
            DataResponse<Student> r = await repo.GetByID(287492);

            Assert.IsFalse(r.Success);
            Assert.AreEqual(expected: "User not found\r\n", actual: r.GetErrorMessage());
        }

        [Test]
        public async Task ShouldUpdateAStudent()
        {
            Student u = (await repo.GetByID(3)).Data[0];
            u.User.Name = "Updated in student"; 

            Response r = await repo.Update(u);
            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldDeleteAStudent()
        {
            Student u = (await repo.GetByID(3)).Data[0];
            u.User.IsActive = false; 

            Response r = await repo.Delete(u.ID);
            Assert.IsTrue(r.Success);
        }
    }
}
