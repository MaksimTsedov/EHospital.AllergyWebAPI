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
    public class AllergyServiceTest
    {
        private static Mock<IRepository<Allergy>> _mockRepo;
        private static Mock<IUnitOfWork> _mockData;
        private List<Allergy> _allergyList;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepo = new Mock<IRepository<Allergy>>();
            _mockData = new Mock<IUnitOfWork>();
            _mockData.Setup(s => s.Allergies).Returns(_mockRepo.Object);
            _allergyList = new List<Allergy>() {
           new Allergy { Id = 1, Pathogen = "prisma" },
           new Allergy { Id = 2, Pathogen = "pomidor" },
           new Allergy { Id = 3, Pathogen = "abrikos" }
          };
        }

        [TestMethod]
        public void Allergies_GetAllAllergies()
        {       
            //Arrange
            _mockData.Setup(s => s.Allergies.GetAll()).Returns(_allergyList.AsQueryable);

            //Act
            var actual = new AllergyService(_mockData.Object).GetAllAllergies().Result.ToList();

            //Assert
            Assert.AreEqual(actual.Count(), 3);
            //GetAll also descending sorting 
            Assert.AreEqual("abrikos", actual[0].Pathogen);
            Assert.AreEqual("pomidor", actual[1].Pathogen);
            Assert.AreEqual("prisma", actual[2].Pathogen);
        }

        [TestMethod]
        public void Allergies_GetAllergy_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.Get(2)).ReturnsAsync(_allergyList[1]);

            //Act
            var actual = new AllergyService(_mockData.Object).GetAllergy(2).Result;

            //Assert
            Assert.AreEqual(actual, _allergyList[1]);
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(0)]
        [DataRow(-2)]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Allergies_GetAllergy_ThrowArgumentNullException(int id)
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.Get(id)).ReturnsAsync(default(Allergy));

            //Act
            var actual = await new AllergyService(_mockData.Object).GetAllergy(id);
        }

        [TestMethod]
        public async Task Allergies_SearchAllergiesByName_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.GetAll(It.IsAny<Expression<Func<Allergy, bool>>>()))
                                            .Returns(_allergyList.Where(a => a.Pathogen.StartsWith("p")).AsQueryable);

            //Act
            var actual = await new AllergyService(_mockData.Object).SearchAllergiesByName("p");

            //Assert
            Assert.AreEqual(actual.Count(), 2);
            Assert.AreEqual("pomidor", actual.ToList()[0].Pathogen);
            Assert.AreEqual("prisma", actual.ToList()[1].Pathogen);
        }

        [TestMethod]
        public async Task Allergies_SearchAllergiesByName_NotFoundAnyAllergy()
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.GetAll(It.IsAny<Expression<Func<Allergy, bool>>>()))
                                            .Returns(_allergyList.Where(a => a.Pathogen.StartsWith("s")).AsQueryable);

            //Act
            var actual = await new AllergyService(_mockData.Object).SearchAllergiesByName("s");

            //Assert
            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void Allergies_CreateAllergyAsync_Correct()
        {
            //Arrange
            int id = 4;
            Allergy testAllergy = new Allergy { Id = id, Pathogen = "Test" };
            _mockData.Setup(s => s.Allergies.Insert(testAllergy)).Returns(testAllergy);

            //Act
            var actual = new AllergyService(_mockData.Object).CreateAllergyAsync(testAllergy).Result;

            //Assert
            Assert.AreEqual(testAllergy, actual);
            _mockData.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        public async void Allergies_CreateAllergyAsync_IncorrectDueToDuplicateName()
        {
            //Arrange
            Allergy testAllergy = new Allergy { Id = 1, Pathogen = "abrikos" };
            _mockData.Setup(s => s.Allergies.GetAll()).Returns(_allergyList.AsQueryable);

            //Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => 
                                         new AllergyService(_mockData.Object).CreateAllergyAsync(testAllergy));
        }

        [TestMethod]
        public void Allergies_DeleteAllergyAsync_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.Get(1)).ReturnsAsync(_allergyList[0]);
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
        public async void Allergies_DeleteAllergyAsync_ThrowArgumentNullException(int id)
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.Get(id)).ReturnsAsync(default(Allergy));
            _mockData.Setup(s => s.PatientAllergies.GetAll()).Returns(new List<PatientAllergy>().AsQueryable);

            //Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => 
                                     new AllergyService(_mockData.Object).DeleteAllergyAsync(id)); //Not found such allergy
        }

        [TestMethod]
        public async void Allergies_DeleteAllergyAsync_LinkedToPatient_HasInvalidOperationException()
        {
            //Arrange
            _mockData.Setup(s => s.Allergies.Get(1)).ReturnsAsync(_allergyList[0]);
            _mockData.Setup(s => s.PatientAllergies.GetAll()).Returns(new List<PatientAllergy>
            {
                new PatientAllergy{ AllergyId = 1, PatientId = 1 }
            }.AsQueryable);

            //Act
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                                    new AllergyService(_mockData.Object).DeleteAllergyAsync(1)); //Deleting allergy while we have records about this allergy in other tables
        }
    }
}
