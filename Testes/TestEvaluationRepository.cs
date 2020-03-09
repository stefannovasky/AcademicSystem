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
    public class TestEvaluationRepository
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ShouldCreateEvaluation()
        {
            Evaluation Evaluation = new Evaluation()
            {
                Name = "Provinha de Migrations",
                Value = 0,
                ClassID = (await new ClassRepository().GetAll()).Data[0].ID,
                StudentID = (await new StudentRepository().GetAll()).Data[0].ID,
                IsActive = true
            };
            Response response = await new EvaluationRepository().Create(Evaluation);
            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldDeleteEvaluation()
        {
            Response response = await new EvaluationRepository().Delete((await new EvaluationRepository().GetAll()).Data[0].ID);
            Assert.IsTrue(response.Success);
        }
        [Test]
        public async Task ShouldGetAllEvaluation()
        {
            Assert.IsTrue((await new EvaluationRepository().GetAll()).Success);
        }
        [Test]
        public async Task ShouldGetByIdEvaluation()
        {
            Assert.IsTrue((await new EvaluationRepository().GetByID((await new EvaluationRepository().GetAll()).Data[0].ID)).Success);
        }
        [Test]
        public async Task ShouldUpdateEvaluation()
        {
            Evaluation Evaluation = (await new EvaluationRepository().GetAll()).Data[0];
            Evaluation.Value = 0.5;
            Assert.IsTrue((await new EvaluationRepository().Update(Evaluation)).Success);
        }
    }
}
