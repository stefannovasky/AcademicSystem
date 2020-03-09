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
    class TestStudentRepository
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ShouldCreateAStudent()
        {
            UserRepository userrepo = new UserRepository();
            DataResponse<User> r = await userrepo.GetAll();

            User u = r.Data[0];

            StudentRepository repo = new StudentRepository();
            Response response = await repo.Create(new Student() { UserID = u.ID });
            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldReturnUniqueKeyError()
        {
            UserRepository userrepo = new UserRepository();
            DataResponse<User> r = await userrepo.GetAll();

            User u = r.Data[0];

            StudentRepository repo = new StudentRepository();
            Response response = await repo.Create(new Student() { UserID = u.ID });
            Assert.IsFalse(response.Success);
            Assert.AreEqual(expected: "Student already exists\r\n", actual: response.GetErrorMessage());
        }
        [Test]
        public async Task ShouldDeleteAStudent()
        {
            StudentRepository repo = new StudentRepository();
            int id = (await new StudentRepository().GetAll()).Data[0].ID;
            Response r = await repo.Delete(id);

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldGetAllStudents()
        {
            StudentRepository repo = new StudentRepository();

            DataResponse<Student> r = await repo.GetAll();

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldGetAStudentByID()
        {
            StudentRepository repo = new StudentRepository();

            DataResponse<Student> r = await repo.GetByID(1);

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldUpdateAStudent()
        {
            StudentRepository repo = new StudentRepository();

            DataResponse<Student> r = await repo.GetByID(1);

            Student u = r.Data[0];
            u.User.Name = "Updated";

            DataResponse<Student> r2 = await repo.Update(u);

            Assert.IsTrue(r.Success);
            Assert.AreEqual(expected: "Updated", actual: r2.Data[0].User.Name);
        }
    }
}
