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
    public class TestCourseRepository
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ShouldCreateCourse()
        {
            Course Course = new Course()
            {
                Name = "Excel Completo",
                Period = "Noturno",
                IsActive = true
            };
            Response response = await new CourseRepository().Create(Course);
            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldDeleteCourse()
        {
            Response response = await new CourseRepository().Delete((await new CourseRepository().GetAll()).Data[0].ID);
            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldGetAllCourse()
        {
            Assert.IsTrue((await new CourseRepository().GetAll()).Success);
        }
        [Test]
        public async Task ShouldGetByIdCourse()
        {
            Assert.IsTrue((await new CourseRepository().GetByID((await new CourseRepository().GetAll()).Data[0].ID)).Success);
        }
        [Test]
        public async Task ShouldUpdateCourse()
        {
            Course Course = (await new CourseRepository().GetAll()).Data[0];
            Course.Name = "Testo";
            Assert.IsTrue((await new CourseRepository().Update(Course)).Success);
        }
    }
}
