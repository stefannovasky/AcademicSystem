using DAL;
using DAL.Impl;
using Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shared;
using System.Threading.Tasks;

namespace Tests
{

    public class TestUserRepository
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public async Task ShouldCreateAUser()
        {
            UserRepository repo = new UserRepository();

            User u = new User
            {
                City = "Blumenau",
                State = "SC",
                Street = "José Da Silva",
                Rg = "1.193.223",
                Cpf = "121.333.112-11",
                Email = "valsi3dfsdsd_email@mail.com.br",
                Name = "Stefan Novasky",
                Number = "11",
                Password = "ValidPassword123!",   
            };

            Response r = await repo.Create(u);
            Assert.IsTrue(r.Success);
        }

            [Test]
            public async Task ShouldReturnEmailAlreadyExists()
            {
                UserRepository repo = new UserRepository();

                User u = new User
                {
                    City = "Blumenau",
                    State = "SC",
                    Street = "José Da Silva",
                    Rg = "7.677.207",
                    Cpf = "262.232.242.12",
                    Email = "duplicated@mail.com.br",
                    Name = "Stefan Novasky",
                    Number = "11",
                    Password = "ValidPassword123!",
                };

                User u2 = new User
                {
                    City = "Blumenau",
                    State = "SC",
                    Street = "José Da Silva",
                    Rg = "9.998.995",
                    Cpf = "211.901.277-19",
                    Email = "duplicated@mail.com.br",
                    Name = "Stefan Novasky",
                    Number = "11",
                    Password = "ValidPassword123!",
                };

                Response r = await repo.Create(u);
                Response r2 = await repo.Create(u2);
                Assert.IsFalse(r2.Success);
                Assert.AreEqual(expected: "Email already exists\r\n", actual: r2.GetErrorMessage());
            }
            [Test]
            public async Task ShouldReturnCpfAlreadyExists()
            {
                UserRepository repo = new UserRepository();

                User u = new User
                {
                    City = "Blumenau",
                    State = "SC",
                    Street = "José Da Silva",
                    Rg = "1.111.111",
                    Cpf = "777.777.777-77",
                    Email = "mail@mail.com",
                    Name = "Stefan Novasky",
                    Number = "11",
                    Password = "ValidPassword123!",
                };

                User u2 = new User
                {
                    City = "Blumenau",
                    State = "SC",
                    Street = "José Da Silva",
                    Rg = "1.111.112",
                    Cpf = "777.777.777-77",
                    Email = "mail2@mail.com",
                    Name = "Stefan Novasky",
                    Number = "11",
                    Password = "ValidPassword123!",
                };

                Response r = await repo.Create(u);
                Response r2 = await repo.Create(u2);
                Assert.IsFalse(r2.Success);
                Assert.AreEqual(expected: "Cpf already exists\r\n", actual: r2.GetErrorMessage());
            }
            [Test]
            public async Task ShouldReturnRgAlreadyExists()
            {
                UserRepository repo = new UserRepository();

                User u = new User
                {
                    City = "Blumenau",
                    State = "SC",
                    Street = "José Da Silva",
                    Rg = "3.333.333",
                    Cpf = "717.777.776-77",
                    Email = "mail32132@mail.com",
                    Name = "Stefan Novasky",
                    Number = "11",
                    Password = "ValidPassword123!",
                };

                User u2 = new User
                {
                    City = "Blumenau",
                    State = "SC",
                    Street = "José Da Silva",
                    Rg = "3.333.333",
                    Cpf = "727.717.777-77",
                    Email = "mail1221313132@mail.com",
                    Name = "Stefan Novasky",
                    Number = "11",
                    Password = "ValidPassword123!",
                };

                Response r = await repo.Create(u);
                Response r2 = await repo.Create(u2);
                Assert.IsFalse(r2.Success);
                Assert.AreEqual(expected: "Rg already exists\r\n", actual: r2.GetErrorMessage());
            }
            [Test]
            public async Task ShouldDeleteAUser()
            {
                UserRepository repo = new UserRepository();

                Response r = await repo.Delete(1016);

                Assert.IsTrue(r.Success);
            }
            [Test]
            public async Task ShouldGetAllUsers()
            {
                UserRepository repo = new UserRepository();

                DataResponse<User> r = await repo.GetAll();

                Assert.IsTrue(r.Success);
            }
            [Test]
            public async Task ShouldGetAUserByID()
            {
                UserRepository repo = new UserRepository();

                DataResponse<User> r = await repo.GetByID(1024);

                Assert.IsTrue(r.Success);
            }
            [Test]
            public async Task ShouldReturnUserNotFound()
            {
                UserRepository repo = new UserRepository();

                DataResponse<User> r = await repo.GetByID(287492);

                Assert.IsFalse(r.Success);
                Assert.AreEqual(expected: "User not found\r\n", actual: r.GetErrorMessage()); 
            }
            [Test]
            public async Task ShouldUpdateAUser()
            {
                UserRepository repo = new UserRepository();

                DataResponse<User> r = await repo.GetByID(1024);
            
                User u = r.Data[0];
                u.Name = "Updated"; 

                DataResponse<User> r2 = await repo.Update(u);

                Assert.IsTrue(r.Success);
                Assert.AreEqual(expected: "Updated", actual: r2.Data[0].Name); 
            }
    }
}