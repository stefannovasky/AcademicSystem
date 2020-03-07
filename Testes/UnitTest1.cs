using DAL;
using Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Testes
{

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestSubject()
        {          
            try
            {
                using (AcademyContext ctx = new AcademyContext())
                {
                    ctx.Users.Add(new User() { 
                        City="bLUEMANI",
                        Cpf="111.111.111-11",
                        Name="SADIAODA",
                        Email="ASAJHDAIUDAU@SFKDFISJ.COM",
                        Number="123",
                        Password="ASIODUAIDA",
                        Rg="7.123.123",
                        Street="maria vai lavar a louca",
                        State="sa"
                    });
                    ctx.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {

                throw ex; 
            }
        }
    }
}