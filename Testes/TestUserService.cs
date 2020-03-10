using BLL.Impl;
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
    class TestUserService
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public async Task ShouldReturnValidationError()
        {
            User u = new User
            {
                City = "Blumenau",
                State = "SC",
                Street = "",
                Rg = "1.193.223",
                Cpf = "444.444.423-12",
                Email = "randommail@mail.com.br",
                Name = "Stefan Novasky",
                Number = "11",
                Password = "ValidPassword123!",
            };

            Response r = await new UserService().Create(u);

            Assert.IsFalse(r.Success);
            Assert.AreEqual(expected: "Validation error\r\n", actual: r.GetErrorMessage());
        }

        [Test]
        public async Task ShouldCreateAUser()
        {
            User u = new User
            {
                City = "Blumenau",
                State = "SC",
                Street = "José Da Silva",
                Rg = "1.193.223",
                Cpf = "326.731.610-31",
                Email = "randommail@mail.com.br",
                Name = "Stefan Novasky",
                Number = "11",
                Password = "ValidPassword123!",
            };

            Response r = await new UserService().Create(u);

            Assert.IsTrue(r.Success);
        }

        [Test]
        public async Task ShouldUpdateAUser()
        {
            User u = (await (new UserRepository().GetAll())).Data[0];
            u.Email = "ipdated@mgi.com";
            
            Response r = await new UserService().Update(u);

            Assert.IsTrue(r.Success);
        }
        [Test]
        public async Task ShouldGetAllUsers()
        {
            DataResponse<User> response = await new UserService().GetAll();

            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldGetAUserById()
        {
            int id = 1;

            DataResponse<User> response = await new UserRepository().GetByID(id); 

            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldDeleteAUserById()
        {
            int id = 1;

            Response response = await new UserRepository().Delete(id);

            Assert.IsTrue(response.Success);
        }
    }
}
