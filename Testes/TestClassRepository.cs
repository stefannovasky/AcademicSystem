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
    public class TestClassRepository
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ShouldCreateClass()
        {
            Class Class = new Class()
            {
                CourseID = (await new CourseRepository().GetAll()).Data[0].ID,
                SubjectID = (await new SubjectRepository().GetAll()).Data[0].ID,
                IsActive = true
            };
            Response response = await new ClassRepository().Create(Class);
            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldDeleteClass()
        {
            Response response = await new ClassRepository().Delete((await new ClassRepository().GetAll()).Data[0].ID);
            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldGetAllClass()
        {
            Assert.IsTrue((await new ClassRepository().GetAll()).Success);
        }
        [Test]
        public async Task ShouldGetByIdClass()
        {
            Assert.IsTrue((await new ClassRepository().GetByID((await new ClassRepository().GetAll()).Data[0].ID)).Success);
        }
        [Test]
        public async Task ShouldUpdateClass()
        {
            Class Class = (await new ClassRepository().GetAll()).Data[0];
            Class.CreatedAt = DateTime.Now;
            Assert.IsTrue((await new ClassRepository().Update(Class)).Success);
        }
    }
}
