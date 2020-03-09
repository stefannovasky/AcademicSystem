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
    public class TestAttendanceRepository
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ShouldCreateAttendance()
        {
            Attendance attendance = new Attendance()
            {
                ClassID = (await new ClassRepository().GetAll()).Data[0].ID,
                Date = DateTime.Now,
                StudentID = (await new StudentRepository().GetAll()).Data[0].ID,
                Value = true,
                IsActive = true
            };
            Response response = await new AttendanceRepository().Create(attendance);
            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldDeleteAttendance()
        {
            Response response = await new AttendanceRepository().Delete((await new AttendanceRepository().GetAll()).Data[0].ID);
            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldGetAllAttendance()
        {
            Assert.IsTrue((await new AttendanceRepository().GetAll()).Success);
        }
        [Test]
        public async Task ShouldGetByIdAttendance()
        {
            Assert.IsTrue((await new AttendanceRepository().GetByID((await new AttendanceRepository().GetAll()).Data[0].ID)).Success);
        }
        [Test]
        public async Task ShouldUpdateAttendance()
        {
            Attendance attendance = (await new AttendanceRepository().GetAll()).Data[0];
            attendance.Date = DateTime.MinValue;
            Assert.IsTrue((await new AttendanceRepository().Update(attendance)).Success);
        }
    }
}
