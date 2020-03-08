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
    class TestOwnerRepository
    {
        [SetUp]
        public void Setup()
        {
        }

        /*
        [Test]
        public async Task ShouldCreateAOwner()
        {
            OwnerRepository repo = new OwnerRepository();

            int id = (await new UserRepository().GetAll()).Data[2].ID;

            Owner u = new Owner
            {
                UserID = id
            };

            Response r = await repo.Create(u);
            Assert.IsTrue(r.Success);
        }
        */
        /*
        [Test]
        public async Task ShouldReturnUniqueKeyError()
        {
            UserRepository userrepo = new UserRepository();
            DataResponse<User> r = await userrepo.GetAll();
            User u = r.Data[0];
            OwnerRepository repo = new OwnerRepository();
            Response response = await repo.Create(new Owner() { UserID = u.ID });
            Assert.IsFalse(response.Success);
            Assert.AreEqual(expected: "Owner already exists\r\n", actual: response.GetErrorMessage());
        }
        */
        
                
        /*
        [Test]
        public async Task ShouldDeleteAOwner()
        {
            OwnerRepository repo = new OwnerRepository();
            int id = (await new UserRepository().GetAll()).Data[0].ID;
            Response r = await repo.Delete(id);
            Assert.IsTrue(r.Success);
        }
        */

        /*
        [Test]
        public async Task ShouldGetAllOwners()
        {
            OwnerRepository repo = new OwnerRepository();
            DataResponse<Owner> r = await repo.GetAll();
            Assert.IsTrue(r.Success);
        }
        */
        /*
        [Test]
        public async Task ShouldGetAOwnerByID()
        {
            OwnerRepository repo = new OwnerRepository();
            DataResponse<Owner> r = await repo.GetByID(1);
            Assert.IsTrue(r.Success);
        }
        */
        /*
        [Test]
        public async Task ShouldUpdateAOwner()
        {
            OwnerRepository repo = new OwnerRepository();
            DataResponse<Owner> r = await repo.GetByID(4);
            Owner u = r.Data[0];
            u.User.Name = "Updated";
            DataResponse<Owner> r2 = await repo.Update(u);
            Assert.IsTrue(r.Success);
            Assert.AreEqual(expected: "Updated", actual: r2.Data[0].User.Name);
        }
        */
    }
}
