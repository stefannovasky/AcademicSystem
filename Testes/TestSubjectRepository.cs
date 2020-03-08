using DAL.Impl;
using NUnit.Framework;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class TestSubjectRepository
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ShouldCreateSubject()
        {
            Entities.Subject Subject = new Entities.Subject()
            {
                CourseID = (await new CourseRepository().GetAll()).Data[0].ID,
                IsActive = true
            };
            Response response = await new SubjectRepository().Create(Subject);
            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldDeleteSubject()
        {
            Response response = await new SubjectRepository().Delete((await new SubjectRepository().GetAll()).Data[0].ID);
            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldGetAllSubject()
        {
            Assert.IsTrue((await new SubjectRepository().GetAll()).Success);
        }
        [Test]
        public async Task ShouldGetByIdSubject()
        {
            Assert.IsTrue((await new SubjectRepository().GetByID((await new SubjectRepository().GetAll()).Data[0].ID)).Success);
        }
        [Test]
        public async Task ShouldUpdateSubject()
        {
            Entities.Subject Subject = (await new SubjectRepository().GetAll()).Data[0];
            Subject.CreatedAt = DateTime.Now;
            Assert.IsTrue((await new SubjectRepository().Update(Subject)).Success);
        }
    }
}
