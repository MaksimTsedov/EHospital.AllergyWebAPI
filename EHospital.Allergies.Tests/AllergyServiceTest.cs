using EHospital.Allergies.BusinesLogic.Contracts;
using EHospital.Allergies.BusinesLogic.Services;
using EHospital.Allergies.Data;
using EHospital.Allergies.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace EHospital.Allergies.Tests
{
    [TestClass]
    public class AllergyServiceTest
    {
        private static Mock<IRepository<Allergy>> _mockRepo;
        private static Mock<IUnitOfWork> _mockData;
        private List<Allergy> allergyList;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepo = new Mock<IRepository<Allergy>>();
            _mockData = new Mock<IUnitOfWork>();
            _mockData.Setup(s => s.Allergies).Returns(_mockRepo.Object);
            allergyList = new List<Allergy>() {
           new Allergy() { Id = 1, Pathogen = "prisma" },
           new Allergy() { Id = 2, Pathogen = "pomidor" },
           new Allergy() { Id = 3, Pathogen = "abrikos" }
          };
        }

        [TestMethod]
        public void Allergies_GetAllAllergies()
        {       
            //Arrange
            _mockData.Setup(s => s.Allergies.GetAll()).Returns(allergyList.AsQueryable);

            //Act
            var actual = new AllergyService(_mockData.Object).GetAllAllergies().ToList();

            //Assert
            Assert.AreEqual(actual.Count(), 3);
            Assert.AreEqual("abrikos", actual[0].Pathogen);
            Assert.AreEqual("pomidor", actual[1].Pathogen);
            Assert.AreEqual("prisma", actual[2].Pathogen);
        }

        [TestMethod]
        public void Allergies_GetAllergy_IdIs2_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.Get(2)).Returns(allergyList[1]);

            //Act
            var actual = new AllergyService(_mockData.Object).GetAllergy(2);

            //Assert
            Assert.AreEqual(actual, allergyList[1]);
        }
    }
}
