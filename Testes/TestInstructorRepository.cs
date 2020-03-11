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
    class TestInstructorRepository
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ShouldCreateAInstructor()
        {
            UserRepository userrepo = new UserRepository();
            DataResponse<User> r = await userrepo.GetAll();
            User u = r.Data[0];
            InstructorRepository repo = new InstructorRepository();
            Response response = await repo.Create(new Instructor() { UserID = u.ID });
            Assert.IsTrue(response.Success);
        }

        [Test]
        public async Task ShouldReturnUniqueKeyError()
        {
            UserRepository userrepo = new UserRepository();
            DataResponse<User> r = await userrepo.GetAll();
            User u = r.Data[0];
            InstructorRepository repo = new InstructorRepository();
            Response response = await repo.Create(new Instructor() { UserID = u.ID });
            Assert.IsFalse(response.Success);
            Assert.AreEqual(expected: "Instructor already exists\r\n", actual: response.GetErrorMessage());
        }
        [Test]
        public async Task ShouldDeleteAInstructor()
        {
            InstructorRepository repo = new InstructorRepository();
            int id = (await new InstructorRepository().GetAll()).Data[0].ID;
            Response r = await repo.Delete(id);
            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldGetAllInstructors()
        {
            InstructorRepository repo = new InstructorRepository();
            DataResponse<Instructor> r = await repo.GetAll();
            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldGetAInstructorByID()
        {
            InstructorRepository repo = new InstructorRepository();
            DataResponse<Instructor> r = await repo.GetByID(1);
            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldUpdateAInstructor()
        {
            InstructorRepository repo = new InstructorRepository();
            DataResponse<Instructor> r = await repo.GetByID(1);
            Instructor u = r.Data[0];
            u.User.Name = "Updated";
            DataResponse<Instructor> r2 = await repo.Update(u);
            Assert.IsTrue(r.Success);
            Assert.AreEqual(expected: "Updated", actual: r2.Data[0].User.Name);
        }
        [Test]
        public async Task ShouldAddInstructorToClass()
        {
            InstructorRepository instructorRepository = new InstructorRepository();
            ClassRepository classRepository = new ClassRepository();
            Instructor instructor = (await instructorRepository.GetByID(1)).Data[0];
            Class @class = (await classRepository.GetByID(1002)).Data[0];
            Response response = await classRepository.AddInstructor(@class, instructor);
            Assert.IsTrue(response.Success);
        }
    }
}
