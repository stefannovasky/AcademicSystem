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
    public class TestCoordinatorRepository
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ShouldCreateCoordinator()
        {
            Coordinator Coordinator = new Coordinator()
            {
                UserID = (await new UserRepository().GetAll()).Data[0].ID,
                IsActive = true
            };
            Response response = await new CoordinatorRepository().Create(Coordinator);
            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldDeleteCoordinator()
        {
            Response response = await new CoordinatorRepository().Delete((await new CoordinatorRepository().GetAll()).Data[0].ID);
            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldGetAllCoordinator()
        {
            Assert.IsTrue((await new CoordinatorRepository().GetAll()).Success);
        }
        [Test]
        public async Task ShouldGetByIdCoordinator()
        {
            Assert.IsTrue((await new CoordinatorRepository().GetByID((await new CoordinatorRepository().GetAll()).Data[0].ID)).Success);
        }
        [Test]
        public async Task ShouldUpdateCoordinator()
        {
            Coordinator Coordinator = (await new CoordinatorRepository().GetAll()).Data[0];
            Coordinator.CreatedAt = DateTime.MinValue;
            Assert.IsTrue((await new CoordinatorRepository().Update(Coordinator)).Success);
        }
    }
}
