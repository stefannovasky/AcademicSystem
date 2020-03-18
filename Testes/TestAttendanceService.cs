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
    public class TestAttendanceService
    {
        [SetUp]
        public void Setup() { }

        private AttendanceRepository repo = new AttendanceRepository();

        [Test]
        public async Task ShouldCreateAAttendance()
        {
            Student u = (await new StudentRepository().GetAll()).Data[0];
            Class c = (await new ClassRepository().GetAll()).Data[0];
            Attendance s = new Attendance { StudentID = u.ID, ClassID = c.ID, Value = true, Date = DateTime.Now};

            Response r = await repo.Create(s);
            Assert.IsTrue(r.Success);
        }

        [Test]
        public async Task ShouldGetAllAttendances()
        {
            DataResponse<Attendance> r = await repo.GetAll();

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldGetAAttendanceByID()
        {
            DataResponse<Attendance> r = await repo.GetByID(1);

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldReturnAttendanceNotFound()
        {
            DataResponse<Attendance> r = await repo.GetByID(287492);

            Assert.IsFalse(r.Success);
            Assert.AreEqual(expected: "User not found\r\n", actual: r.GetErrorMessage());
        }

        [Test]
        public async Task ShouldUpdateAAttendance()
        {
            Attendance u = (await repo.GetByID(1)).Data[0];
            u.Value = !u.Value;

            Response r = await repo.Update(u);
            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldDeleteAAttendance()
        {
            Attendance u = (await repo.GetByID(1)).Data[0];

            Response r = await repo.Delete(u.ID);
            Assert.IsTrue(r.Success);
        }
    }
}
