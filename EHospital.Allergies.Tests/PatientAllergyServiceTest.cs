using EHospital.Allergies.Model;
using Microsoft.EntityFrameworkCore;
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
    public class PatientAllergyServiceTest
    {
        private static Mock<IRepository<PatientAllergy>> _mockRepo;
        private static Mock<IUnitOfWork> _mockData;
        private List<Allergy> _allergyList;
        private List<PatientAllergy> _patientAllergyList;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepo = new Mock<IRepository<PatientAllergy>>();
            _mockData = new Mock<IUnitOfWork>();
            _mockData.Setup(s => s.PatientAllergies).Returns(_mockRepo.Object);
            _allergyList = new List<Allergy>() {
           new Allergy { Id = 1, Pathogen = "prisma" },
           new Allergy { Id = 2, Pathogen = "pomidor" },
           new Allergy { Id = 3, Pathogen = "abrikos" }
          };
            _patientAllergyList = new List<PatientAllergy>() {
           new PatientAllergy { Id = 1, PatientId = 1, AllergyId = 1 },
           new PatientAllergy { Id = 2, PatientId = 1, AllergyId = 2 },
           new PatientAllergy { Id = 3, PatientId = 2, AllergyId = 3 }
          };
        }

        [TestMethod]
        public void PatientAllergies_GetAllPatientAllergies_PatientIdIs_1_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.PatientAllergies.Include<Allergy>(It.IsAny<Expression<Func<PatientAllergy, Allergy>>>()))
                     .Returns(_patientAllergyList.AsQueryable().Include(a => a.Allergy));

            //Act
            var actual = new PatientAllergyService(_mockData.Object).GetAllPatientAllergies(1).Result.ToList();

            //Assert
            Assert.AreEqual(actual.Count(), 2);
            Assert.AreEqual(_patientAllergyList[0], actual[0]);
            Assert.AreEqual(_patientAllergyList[1], actual[1]);
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(0)]
        [DataRow(-2)]
        public async Task PatientAllergies_GetAllPatientAllergies_NoAllergies(int id)
        {
            //Arrange
            _mockData.Setup(s => s.PatientAllergies.Include<Allergy>(It.IsAny<Expression<Func<PatientAllergy, Allergy>>>()))
                                                   .Returns(_patientAllergyList.AsQueryable().Include(a => a.Allergy));

            //Act
            var actual = await new PatientAllergyService(_mockData.Object).GetAllPatientAllergies(3);

            //Assert
            Assert.AreEqual(actual.Count(), 0);
        }

        [TestMethod]
        public async Task PatientAllergies_GetPatientAllergy_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.PatientAllergies.Include<Allergy>(It.IsAny<Expression<Func<PatientAllergy, Allergy>>>()))
                     .Returns(_patientAllergyList.AsQueryable().Include(a => a.Allergy));

            //Act
            var actual = await new PatientAllergyService(_mockData.Object).GetPatientAllergy(2);

            //Assert
            Assert.AreEqual(actual, _patientAllergyList[1]);
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(0)]
        [DataRow(-2)]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task PatientAllergies_GetAllPatientAllergies_ThrowArgumentNullException(int id)
        {
            //Arrange
            _mockData.Setup(s => s.PatientAllergies.Include<Allergy>(It.IsAny<Expression<Func<PatientAllergy, Allergy>>>()))
                     .Returns(_patientAllergyList.AsQueryable().Include(a => a.Allergy));

            //Act
            var actual = await new PatientAllergyService(_mockData.Object).GetPatientAllergy(id); //Patient-allergy pair doesn`t exist.
        }

        [TestMethod]
        public void PatientAllergies_CreatePatientAllergyAsync_Correct()
        {
            //Arrange
            int id = 4;
            PatientAllergy testPatientAllergy = new PatientAllergy { Id = id, PatientId = 1, AllergyId = 3 };

            _mockData.Setup(s => s.Allergies.Get(testPatientAllergy.AllergyId))
                     .ReturnsAsync(_allergyList[2]);
            _mockData.Setup(s => s.PatientInfo.Get(testPatientAllergy.PatientId))
                     .ReturnsAsync(new PatientInfo { Id = 1 });
            _mockData.Setup(s => s.PatientAllergies.GetAll())
                     .Returns(_patientAllergyList.AsQueryable);
            _mockData.Setup(s => s.PatientAllergies.Insert(testPatientAllergy)).Returns(testPatientAllergy);


            //Act
            var actual = new PatientAllergyService(_mockData.Object).CreatePatientAllergyAsync(testPatientAllergy)
                                                                    .Result;

            //Assert
            Assert.AreEqual(testPatientAllergy, actual);
            _mockData.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        [DataRow(1, 4)]
        [DataRow(3, 2)]
        [DataRow(0, 0)]
        [DataRow(-1, 1)]
        [DataRow(1, -1)]
        [DataRow(-1, -1)]
        public async void PatientAllergies_CreatePatientAllergyAsync_ThrowArgumentNullException(int patientId, int allergyId)
        {
            //Arrange
            PatientAllergy testPatientAllergy = new PatientAllergy { PatientId = patientId, AllergyId = allergyId };

            _mockData.Setup(s => s.Allergies.Get(testPatientAllergy.AllergyId))
                     .ReturnsAsync(default(Allergy));
            _mockData.Setup(s => s.PatientInfo.Get(testPatientAllergy.PatientId))
                     .ReturnsAsync(default(PatientInfo));

            //Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                                    new PatientAllergyService(_mockData.Object).CreatePatientAllergyAsync(testPatientAllergy));
        }

        [TestMethod]
        public async void PatientAllergies_CreatePatientAllergyAsync_ThrowArgumentExceptionDueToDuplicatePair()
        {
            //Arrange
            int id = 4;
            PatientAllergy testPatientAllergy = new PatientAllergy { PatientId = 1, AllergyId = 2 };

            _mockData.Setup(s => s.Allergies.Get(testPatientAllergy.AllergyId))
                     .ReturnsAsync(_allergyList[1]);
            _mockData.Setup(s => s.PatientInfo.Get(testPatientAllergy.PatientId))
                     .ReturnsAsync(new PatientInfo { Id = 1 });
            _mockData.Setup(s => s.PatientAllergies.GetAll())
                                            .Returns(_patientAllergyList.AsQueryable);
            _mockData.Setup(s => s.PatientAllergies.Insert(testPatientAllergy)).Returns((PatientAllergy a) =>
            {
                a.Id = id;
                return a;
            });

            //Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
                                    await new PatientAllergyService(_mockData.Object)
                                    .CreatePatientAllergyAsync(testPatientAllergy));
        }

        [TestMethod]
        public void PatientAllergies_UpdatePatientAllergyAsync_Correct()
        {
            //Arrange
            PatientAllergy testPatientAllergy = new PatientAllergy { Id = 4, AllergyId = 3 };
            _mockData.Setup(s => s.PatientAllergies.Get(2)).ReturnsAsync(_patientAllergyList[1]);
            _mockData.Setup(s => s.Allergies.Get(3)).ReturnsAsync(_allergyList[2]);
            _mockData.Setup(s => s.PatientAllergies.GetAll()).Returns(_patientAllergyList.AsQueryable);
            _mockData.Setup(s => s.PatientAllergies.Update(It.IsAny<PatientAllergy>())).Returns(testPatientAllergy);

            //Act
            var actual = new PatientAllergyService(_mockData.Object).UpdatePatientAllergyAsync(2, testPatientAllergy)
                                                                    .Result;

            //Assert
            Assert.AreEqual(testPatientAllergy.AllergyId, actual.AllergyId);
            Assert.AreNotEqual(testPatientAllergy.Id, actual.Id);
            _mockData.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        [DataRow(4, 1)]
        [DataRow(2, 4)]
        [DataRow(0, 0)]
        [DataRow(-2, -4)]
        public async void PatientAllergies_UpdatePatientAllergyAsync_ThrowArgumentNullException(int patientAllergyId, int allergyId)
        {
            //Arrange
            PatientAllergy testPatientAllergy = new PatientAllergy { Id = 4,  AllergyId = allergyId };
            _mockData.Setup(s => s.PatientAllergies.Get(patientAllergyId)).ReturnsAsync(default(PatientAllergy));
            _mockData.Setup(s => s.Allergies.Get(allergyId)).ReturnsAsync(default(Allergy));

            //Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(()
                                 => new PatientAllergyService(_mockData.Object)
                                        .UpdatePatientAllergyAsync(patientAllergyId, testPatientAllergy)); //Can`t update due to nonexistence such patient or allergy

        }

        [TestMethod]
        public async void PatientAllergies_UpdatePatientAllergyAsync_ThrowArgumentExceptionDueToDuplicatePair()
        {
            //Arrange
            PatientAllergy testPatientAllergy = new PatientAllergy { Id = 4, AllergyId = 2 };
            _mockData.Setup(s => s.PatientAllergies.Get(2)).ReturnsAsync(_patientAllergyList[1]);
            _mockData.Setup(s => s.Allergies.Get(2)).ReturnsAsync(_allergyList[1]);
            _mockData.Setup(s => s.PatientAllergies.GetAll()).Returns(_patientAllergyList.AsQueryable);
            _mockData.Setup(s => s.PatientAllergies.Update(It.IsAny<PatientAllergy>())).Returns(testPatientAllergy);

            //Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(()
                                => new PatientAllergyService(_mockData.Object)
                                       .UpdatePatientAllergyAsync(2, testPatientAllergy));
        }
        [TestMethod]
        public void PatientAllergies_UpdateNotesAsync_Correct()
        {
            //Arrange
            PatientAllergy testPatientAllergy = new PatientAllergy { Id = 4, Notes = "Test" };
            _mockData.Setup(s => s.PatientAllergies.Get(2)).ReturnsAsync(_patientAllergyList[1]);
            _mockData.Setup(s => s.PatientAllergies.Update(It.IsAny<PatientAllergy>())).Returns(testPatientAllergy);

            //Act
            var actual = new PatientAllergyService(_mockData.Object).UpdateNotesAsync(2, testPatientAllergy.Notes)
                                                                    .Result;

            //Assert
            Assert.AreEqual(testPatientAllergy.Notes, actual.Notes);
            Assert.AreNotEqual(testPatientAllergy.Id, actual.Id);
            _mockData.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(0)]
        [DataRow(-2)]
        public async void PatientAllergies_UpdateNotesAsync_ThrowArgumentNullException(int patientAllergyId)
        {
            //Arrange
            PatientAllergy testPatientAllergy = new PatientAllergy { Id = 4, Notes = "Test" };
            _mockData.Setup(s => s.PatientAllergies.Get(patientAllergyId)).ReturnsAsync(default(PatientAllergy));

            //Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(()
                                 => new PatientAllergyService(_mockData.Object)
                                        .UpdateNotesAsync(patientAllergyId, testPatientAllergy.Notes)); //Not found such pair
        }


        [TestMethod]
        public void PatientAllergies_DeletePatientAllergyAsync_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.PatientAllergies.Get(1)).ReturnsAsync(_patientAllergyList[0]);

            //Act
            var actual = new PatientAllergyService(_mockData.Object).DeletePatientAllergyAsync(1);

            //Assert
            _mockData.Verify(m => m.CascadeDeletePatientAllergy(1), Times.Once);
            _mockData.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(0)]
        [DataRow(-2)]
        public async void PatientAllergies_DeletePatientAllergyAsync_ThrowArgumentNullException(int id)
        {
            //Arrange
            _mockData.Setup(s => s.PatientAllergies.Get(1)).ReturnsAsync(default(PatientAllergy));

            //Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                                    new PatientAllergyService(_mockData.Object).DeletePatientAllergyAsync(id));
        }
    }
}
