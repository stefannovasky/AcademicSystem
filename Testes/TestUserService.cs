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

        [Test]
        public async Task ShouldReturnUserNotFoundInGetByEmail()
        {
            User u = new User
            {
                City = "Blumenau",
                State = "SC",
                Street = "José Da Silva",
                Rg = "1.193.223",
                Cpf = "011.996.550-07",
                Email = "adhaasdauadyauidyad@mail.com.br",
                Name = "Stefan Novasky",
                Number = "11",
                Password = "ValidPassword123!",
            };

            Response r = await new UserService().Create(u);
            if (r.Success)
            {
                u.Email = "asdaouda@gmail.com";

                DataResponse<User> result = await new UserService().Authenticate(u);
                Assert.IsFalse(result.Success);
                Assert.AreEqual(expected: "User not found\r\n", actual: result.GetErrorMessage());
            }
            Assert.IsTrue(r.Success);
        }

        [Test]
        public async Task ShouldReturnUserInvalidPassword()
        {
            User u = new User
            {
                City = "Blumenau",
                State = "SC",
                Street = "José Da Silva",
                Rg = "1.193.223",
                Cpf = "677.215.940-30",
                Email = "aaaa@mail.com.br",
                Name = "Stefan Novasky",
                Number = "11",
                Password = "ValidPassword123!",
            };

            Response r = await new UserService().Create(u);
            if (r.Success)
            {
                u.Password = "Incorrect Password";

                DataResponse<User> result = await new UserService().Authenticate(u);
                Assert.AreEqual(expected: "Invalid password\r\n", actual: result.GetErrorMessage());
            }
        }
        [Test]
        public async Task ShouldAuthenticateUser()
        {
            User u = new User
            {
                City = "Blumenau",
                State = "SC",
                Street = "José Da Silva",
                Rg = "1.193.223",
                Cpf = "070.674.230-31",
                Email = "vrauu@mail.com.br",
                Name = "Stefan Novasky",
                Number = "11",
                Password = "ValidPassword123!",
            };

            Response r = await new UserService().Create(u);
            if (r.Success)
            {
                User u2 = new User() { Email = "vrauu@mail.com.br", Password = "ValidPassword123!" };

                DataResponse <User> result = await new UserService().Authenticate(u2);
                Assert.IsTrue(result.Success);
            }
            Assert.IsTrue(r.Success); 
        }
    }
}
