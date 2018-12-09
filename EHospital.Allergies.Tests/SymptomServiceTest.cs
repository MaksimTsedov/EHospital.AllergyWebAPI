using EHospital.Allergies.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EHospital.Allergies.BusinessLogic.Services;

namespace EHospital.Allergies.Tests
{
    [TestClass]
    public class SymptomServiceTest
    {
        private static Mock<IRepository<Symptom>> _mockRepo;
        private static Mock<IUnitOfWork> _mockData;
        private List<Symptom> _symptomList;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepo = new Mock<IRepository<Symptom>>();
            _mockData = new Mock<IUnitOfWork>();
            _mockData.Setup(s => s.Symptoms).Returns(_mockRepo.Object);
            _symptomList = new List<Symptom>() {
           new Symptom { Id = 1, Naming = "prisma" },
           new Symptom { Id = 2, Naming = "pomidor" },
           new Symptom { Id = 3, Naming = "abrikos" }
          };
        }

        [TestMethod]
        public void Symptoms_GetAllSymptoms()
        {
            //Arrange
            _mockData.Setup(s => s.Symptoms.GetAll()).Returns(_symptomList.AsQueryable);

            //Act
            var actual = new SymptomService(_mockData.Object).GetAllSymptoms().Result.ToList();

            //Assert
            Assert.AreEqual(actual.Count(), 3);
            Assert.AreEqual("abrikos", actual[0].Naming);
            Assert.AreEqual("pomidor", actual[1].Naming);
            Assert.AreEqual("prisma", actual[2].Naming);
        }

        [TestMethod]
        public async Task Symptoms_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.Symptoms.Get(2)).ReturnsAsync(_symptomList[1]);

            //Act
            var actual = await new SymptomService(_mockData.Object).GetSymptom(2);

            //Assert
            Assert.AreEqual(actual, _symptomList[1]);
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(0)]
        [DataRow(-2)]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Symptoms_GetSymptom_ThrowArgumentNullException(int id)
        {
            //Arrange
            _mockData.Setup(s => s.Symptoms.Get(id)).ReturnsAsync(default(Symptom));

            //Act
            var actual = await new SymptomService(_mockData.Object).GetSymptom(id);//Not found such symptom 
        }

        [TestMethod]
        public void Symptoms_SearchSymptomsByName_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.Symptoms.GetAll(It.IsAny<Expression<Func<Symptom, bool>>>()))
                                            .Returns(_symptomList.Where(a => a.Naming.StartsWith("p")).AsQueryable);

            //Act
            var actual = new SymptomService(_mockData.Object).SearchSymptomsByName("p").Result.ToList();

            //Assert
            Assert.AreEqual(actual.Count(), 2);
            Assert.AreEqual("pomidor", actual[0].Naming);
            Assert.AreEqual("prisma", actual[1].Naming);
        }

        [TestMethod]
        public void Symptoms_SearchSymptomsByName_NotFoundAnySymptom()
        {
            //Arrange
            _mockData.Setup(s => s.Symptoms.GetAll(It.IsAny<Expression<Func<Symptom, bool>>>()))
                                            .Returns(_symptomList.Where(a => a.Naming.StartsWith("s")).AsQueryable);

            //Act
            var actual = new SymptomService(_mockData.Object).SearchSymptomsByName("s").Result.ToList();

            //Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void Symptoms_CreateSymptomAsync_Correct()
        {
            //Arrange
            int id = 4;
            Symptom testSymptom = new Symptom { Id = id, Naming = "Test" };
            _mockData.Setup(s => s.Symptoms.Insert(testSymptom)).Returns(testSymptom);

            //Act
            var actual = new SymptomService(_mockData.Object).CreateSymptomAsync(testSymptom).Result;

            //Assert
            Assert.AreEqual(testSymptom, actual);
            _mockData.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        public void Symptoms_CreateSymptomAsync_ThrowAtgumentExceptionDueToDuplicate()
        {
            //Arrange
            Symptom testSymptom = new Symptom { Id = 1, Naming = "abrikos" };
            _mockData.Setup(s => s.Symptoms.GetAll()).Returns(_symptomList.AsQueryable);

            //Assert
            Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                                    new SymptomService(_mockData.Object).CreateSymptomAsync(testSymptom));
        }

        [TestMethod]
        public void Symptoms_DeleteSymptomAsync_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.Symptoms.Get(1)).ReturnsAsync(_symptomList[0]);
            _mockData.Setup(s => s.AllergySymptoms.GetAll()).Returns(new List<AllergySymptom>().AsQueryable);

            //Act
            var actual = new SymptomService(_mockData.Object).DeleteSymptomAsync(1).Result;

            //Assert
            Assert.IsTrue(actual.IsDeleted);
            _mockData.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(0)]
        [DataRow(-2)]
        public void Symptoms_DeleteSymptomAsync_ThrowArgumentNullException(int id)
        {
            //Arrange
            _mockData.Setup(s => s.Symptoms.Get(id)).ReturnsAsync(default(Symptom));
            _mockData.Setup(s => s.AllergySymptoms.GetAll()).Returns(new List<AllergySymptom>().AsQueryable);

            //Act
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                                    new SymptomService(_mockData.Object).DeleteSymptomAsync(id));
        }

        [TestMethod]
        public void Symptoms_DeleteSymptomAsync_LinkedToPatientAllergy_HasInvalidOperationException()
        {
            //Arrange
            _mockData.Setup(s => s.Symptoms.Get(1)).ReturnsAsync(_symptomList[0]);
            _mockData.Setup(s => s.AllergySymptoms.GetAll()).Returns(new List<AllergySymptom>
            {
                new AllergySymptom{ PatientAllergyId = 1, SymptomId = 1 }
            }.AsQueryable);

            //Act
            Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                                    new SymptomService(_mockData.Object).DeleteSymptomAsync(1)); //Deleting symptom while we have records about this allergy in other tables
        }
    }
}
