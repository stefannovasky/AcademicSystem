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
    class TestCoordinatorService
    {
        [SetUp]
        public void Setup() { }

        private CoordinatorRepository repo = new CoordinatorRepository();

        [Test]
        public async Task ShouldCreateACoordinator()
        {
            User u = (await new UserRepository().GetAll()).Data[0];

            Coordinator s = new Coordinator { UserID = u.ID };

            Response r = await repo.Create(s);
            Assert.IsTrue(r.Success);
        }

        [Test]
        public async Task ShouldGetAllCoordinators()
        {
            DataResponse<Coordinator> r = await repo.GetAll();

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldGetACoordinatorByID()
        {
            DataResponse<Coordinator> r = await repo.GetByID(1);

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldReturnCoordinatorNotFound()
        {
            DataResponse<Coordinator> r = await repo.GetByID(287492);

            Assert.IsFalse(r.Success);
            Assert.AreEqual(expected: "User not found\r\n", actual: r.GetErrorMessage());
        }

        [Test]
        public async Task ShouldUpdateACoordinator()
        {
            Coordinator u = (await repo.GetByID(1)).Data[0];
            u.User.Name = "Updated in Coordinator";

            Response r = await repo.Update(u);
            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldDeleteACoordinator()
        {
            Coordinator u = (await repo.GetByID(1)).Data[0];
            u.User.IsActive = false;

            Response r = await repo.Delete(u.ID);
            Assert.IsTrue(r.Success);
        }
    }
}
