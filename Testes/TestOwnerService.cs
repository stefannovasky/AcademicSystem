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
    class TestOwnerService
    {
        [SetUp]
        public void Setup() { }

        private OwnerRepository repo = new OwnerRepository();

        [Test]
        public async Task ShouldCreateAOwner()
        {
            User u = (await new UserRepository().GetAll()).Data[2];

            Owner s = new Owner { UserID = u.ID };

            Response r = await repo.Create(s);
            Assert.IsTrue(r.Success);
        }

        [Test]
        public async Task ShouldGetAllOwners()
        {
            DataResponse<Owner> r = await repo.GetAll();

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldGetAOwnerByID()
        {
            DataResponse<Owner> r = await repo.GetByID(10);

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldReturnOwnerNotFound()
        {
            DataResponse<Owner> r = await repo.GetByID(287492);

            Assert.IsFalse(r.Success);
            Assert.AreEqual(expected: "User not found\r\n", actual: r.GetErrorMessage());
        }

        [Test]
        public async Task ShouldUpdateAOwner()
        {
            Owner u = (await repo.GetByID(10)).Data[0];
            u.User.Name = "Updated in Owner";

            Response r = await repo.Update(u);
            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldDeleteAOwner()
        {
            Owner u = (await repo.GetByID(4)).Data[0];
            u.User.IsActive = false;

            Response r = await repo.Delete(u.ID);
            Assert.IsTrue(r.Success);
        }
    }
}
