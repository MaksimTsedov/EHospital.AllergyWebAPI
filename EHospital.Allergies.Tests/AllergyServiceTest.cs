using EHospital.Allergies.BusinesLogic.Contracts;
using EHospital.Allergies.BusinesLogic.Services;
using EHospital.Allergies.Data;
using EHospital.Allergies.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
        public void Allergies_GetAllergy_IdIs_2_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.Get(2)).Returns(allergyList[1]);

            //Act
            var actual = new AllergyService(_mockData.Object).GetAllergy(2);

            //Assert
            Assert.AreEqual(actual, allergyList[1]);
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(0)]
        [DataRow(-2)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Allergies_GetAllergy_ThrowArgumentNullException(int id)
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.Get(id)).Returns(default(Allergy));

            //Act
            var actual = new AllergyService(_mockData.Object).GetAllergy(id);
        }

        [TestMethod]
        public void Allergies_SearchAllergiesByName_stringKeyIs_p_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.GetAll(It.IsAny<Expression<Func<Allergy, bool>>>()))
                                            .Returns(allergyList.Where(a => a.Pathogen.StartsWith("p")).AsQueryable);

            //Act
            var actual = new AllergyService(_mockData.Object).SearchAllergiesByName("p").ToList();

            //Assert
            Assert.AreEqual(actual.Count(), 2);
            Assert.AreEqual("pomidor", actual[0].Pathogen);
            Assert.AreEqual("prisma", actual[1].Pathogen);
        }

        [TestMethod]
        public void Allergies_SearchAllergiesByName_stringKeyIs_s_InCorrect()
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.GetAll(It.IsAny<Expression<Func<Allergy, bool>>>()))
                                            .Returns(allergyList.Where(a => a.Pathogen.StartsWith("s")).AsQueryable);

            //Act
            var actual = new AllergyService(_mockData.Object).SearchAllergiesByName("s").ToList();

            //Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void Allergies_CreateAllergyAsync_Correct()
        {
            //Arrange
            int id = 4;
            Allergy testAllergy = new Allergy { Id = id, Pathogen = "Test" };
            _mockData.Setup(s => s.Allergies.Insert(testAllergy)).Returns((Allergy a) =>
            {
                a.Id = id;
                return a;
            });

            //Act
            var actual = new AllergyService(_mockData.Object).CreateAllergyAsync(testAllergy).Result;

            //Assert
            Assert.AreEqual(testAllergy.Id, actual.Id);
            _mockData.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        public void Allergies_CreateAllergyAsync_Incorrect()
        {
            //Arrange
            Allergy testAllergy = new Allergy { Id = 1, Pathogen = "abrikos" };
            _mockData.Setup(s => s.Allergies.GetAll()).Returns(allergyList.AsQueryable);

            //Assert
            Assert.ThrowsExceptionAsync<ArgumentException>(() => 
                                    new AllergyService(_mockData.Object).CreateAllergyAsync(testAllergy));
        }

        [TestMethod]
        public void Allergies_DeleteAllergyAsync_IdIs_1_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.Get(1)).Returns(allergyList[0]);
            _mockData.Setup(s => s.PatientAllergies.GetAll()).Returns(new List<PatientAllergy>().AsQueryable);

            //Act
            var actual = new AllergyService(_mockData.Object).DeleteAllergyAsync(1).Result;

            //Assert
            Assert.IsTrue(actual.IsDeleted);
            _mockData.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(0)]
        [DataRow(-2)]
        public void Allergies_DeleteAllergyAsync_ThrowArgumentNullException(int id)
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.Get(id)).Returns(default(Allergy));
            _mockData.Setup(s => s.PatientAllergies.GetAll()).Returns(new List<PatientAllergy>().AsQueryable);

            //Act
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => 
                                    new AllergyService(_mockData.Object).DeleteAllergyAsync(id));
        }

        [TestMethod]
        public void Allergies_DeleteAllergyAsync_LinkedToPatient_HasInvalidOperationException()
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.Get(1)).Returns(allergyList[0]);
            _mockData.Setup(s => s.PatientAllergies.GetAll()).Returns(new List<PatientAllergy>
            {
                new PatientAllergy{ AllergyId = 1, PatientId = 1 }
            }.AsQueryable);

            //Act
            Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                                    new AllergyService(_mockData.Object).DeleteAllergyAsync(1));
        }
    }
}
