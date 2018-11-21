using EHospital.Allergies.BusinesLogic.Services;
using EHospital.Allergies.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
            var actual = new PatientAllergyService(_mockData.Object).GetAllPatientAllergies(1).ToList();

            //Assert
            Assert.AreEqual(actual.Count(), 2);
            Assert.AreEqual(_patientAllergyList[0], actual[0]);
            Assert.AreEqual(_patientAllergyList[1], actual[1]);
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(0)]
        [DataRow(-2)]
        public void PatientAllergies_GetAllPatientAllergies_NoAllergies(int id)
        {
            //Arrange
            _mockData.Setup(s => s.PatientAllergies.Include<Allergy>(It.IsAny<Expression<Func<PatientAllergy, Allergy>>>()))
                                                 .Returns(_patientAllergyList.AsQueryable().Include(a => a.Allergy));

            //Act
            var actual = new PatientAllergyService(_mockData.Object).GetAllPatientAllergies(3).ToList();

            //Assert
            Assert.AreEqual(actual.Count(), 0);
        }

        [TestMethod]
        public void PatientAllergies_GetPatientAllergy_PatientAllergyIdIs_2_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.PatientAllergies.Include<Allergy>(It.IsAny<Expression<Func<PatientAllergy, Allergy>>>()))
                                                  .Returns(_patientAllergyList.AsQueryable().Include(a => a.Allergy));

            //Act
            var actual = new PatientAllergyService(_mockData.Object).GetPatientAllergy(2);

            //Assert
            Assert.AreEqual(actual, _patientAllergyList[1]);
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(0)]
        [DataRow(-2)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatientAllergies_GetAllPatientAllergies_ThrowArgumentNullException(int id)
        {
            //Arrange
            _mockData.Setup(s => s.PatientAllergies.Include<Allergy>(It.IsAny<Expression<Func<PatientAllergy, Allergy>>>()))
                                                 .Returns(_patientAllergyList.AsQueryable().Include(a => a.Allergy));

            //Act
            var actual = new PatientAllergyService(_mockData.Object).GetPatientAllergy(id);
        }

        [TestMethod]
        public void PatientAllergies_CreatePatientAllergyAsync_Correct()
        {
            //Arrange
            int id = 4;
            PatientAllergy testPatientAllergy = new PatientAllergy { PatientId = 1, AllergyId = 3 };

            _mockData.Setup(s => s.Allergies.Get(testPatientAllergy.AllergyId)).Returns(_allergyList[2]);
            _mockData.Setup(s => s.PatientInfo.Get(testPatientAllergy.PatientId)).Returns(new PatientInfo { Id = 1 });
            _mockData.Setup(s => s.PatientAllergies.GetAll())
                                            .Returns(_patientAllergyList.AsQueryable);
            _mockData.Setup(s => s.PatientAllergies.Insert(testPatientAllergy)).Returns((PatientAllergy a) =>
            {
                a.Id = id;
                return a;
            });


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
        public void PatientAllergies_CreatePatientAllergyAsync_ThrowArgumentNullException(int patientId, int allergyId)
        {
            //Arrange
            PatientAllergy testPatientAllergy = new PatientAllergy { PatientId = patientId, AllergyId = allergyId };

            _mockData.Setup(s => s.Allergies.Get(testPatientAllergy.AllergyId)).Returns(default(Allergy));
            _mockData.Setup(s => s.PatientInfo.Get(testPatientAllergy.PatientId)).Returns(default(PatientInfo));

            //Assert
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                                    new PatientAllergyService(_mockData.Object)
                                    .CreatePatientAllergyAsync(testPatientAllergy));
        }

        [TestMethod]
        public void PatientAllergies_CreatePatientAllergyAsync_ThrowArgumentException()
        {
            //Arrange
            int id = 4;
            PatientAllergy testPatientAllergy = new PatientAllergy { PatientId = 1, AllergyId = 2 };

            _mockData.Setup(s => s.Allergies.Get(testPatientAllergy.AllergyId)).Returns(_allergyList[1]);
            _mockData.Setup(s => s.PatientInfo.Get(testPatientAllergy.PatientId)).Returns(new PatientInfo { Id = 1 });
            _mockData.Setup(s => s.PatientAllergies.GetAll())
                                            .Returns(_patientAllergyList.AsQueryable);
            _mockData.Setup(s => s.PatientAllergies.Insert(testPatientAllergy)).Returns((PatientAllergy a) =>
            {
                a.Id = id;
                return a;
            });

            //Assert
            Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                                    new PatientAllergyService(_mockData.Object)
                                    .CreatePatientAllergyAsync(testPatientAllergy));
        }

        [TestMethod]
        public void PatientAllergies_UpdatePatientAllergyAsync_PatientAllergyIdIs_2_Correct()
        {
            //Arrange
            PatientAllergy testPatientAllergy = new PatientAllergy { Id = 4, AllergyId = 3 };
            _mockData.Setup(s => s.PatientAllergies.Get(2)).Returns(_patientAllergyList[1]);
            _mockData.Setup(s => s.Allergies.Get(3)).Returns(_allergyList[2]);
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
        public void PatientAllergies_UpdatePatientAllergyAsync_ThrowArgumentNullException(int patientAllergyId, int allergyId)
        {
            //Arrange
            PatientAllergy testPatientAllergy = new PatientAllergy { Id = 4,  AllergyId = allergyId };
            _mockData.Setup(s => s.PatientAllergies.Get(patientAllergyId)).Returns(default(PatientAllergy));
            _mockData.Setup(s => s.Allergies.Get(allergyId)).Returns(default(Allergy));

            //Assert
            Assert.ThrowsExceptionAsync<ArgumentNullException>(()
                                 => new PatientAllergyService(_mockData.Object)
                                        .UpdatePatientAllergyAsync(patientAllergyId, testPatientAllergy));
                                                                    
        }

        [TestMethod]
        public void PatientAllergies_UpdatePatientAllergyAsync_ThrowArgumentException()
        {
            //Arrange
            PatientAllergy testPatientAllergy = new PatientAllergy { Id = 4, AllergyId = 2 };
            _mockData.Setup(s => s.PatientAllergies.Get(2)).Returns(_patientAllergyList[1]);
            _mockData.Setup(s => s.Allergies.Get(2)).Returns(_allergyList[1]);
            _mockData.Setup(s => s.PatientAllergies.GetAll()).Returns(_patientAllergyList.AsQueryable);
            _mockData.Setup(s => s.PatientAllergies.Update(It.IsAny<PatientAllergy>())).Returns(testPatientAllergy);

            //Assert
            Assert.ThrowsExceptionAsync<ArgumentException>(()
                                => new PatientAllergyService(_mockData.Object)
                                       .UpdatePatientAllergyAsync(2, testPatientAllergy));
        }
        [TestMethod]
        public void PatientAllergies_UpdateNotesAsync_PatientAllergyIdIs_2_Correct()
        {
            //Arrange
            PatientAllergy testPatientAllergy = new PatientAllergy { Id = 4, Notes = "Test" };
            _mockData.Setup(s => s.PatientAllergies.Get(2)).Returns(_patientAllergyList[1]);
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
        public void PatientAllergies_UpdateNotesAsync_ThrowArgumentNullException(int patientAllergyId)
        {
            //Arrange
            PatientAllergy testPatientAllergy = new PatientAllergy { Id = 4, Notes = "Test" };
            _mockData.Setup(s => s.PatientAllergies.Get(patientAllergyId)).Returns(default(PatientAllergy));

            //Assert
            Assert.ThrowsExceptionAsync<ArgumentNullException>(()
                                 => new PatientAllergyService(_mockData.Object)
                                        .UpdateNotesAsync(patientAllergyId, testPatientAllergy.Notes));
        }


        [TestMethod]
        public void PatientAllergies_DeletePatientAllergyAsync_IdIs_1_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.PatientAllergies.Get(1)).Returns(_patientAllergyList[0]);

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
        public void PatientAllergies_DeletePatientAllergyAsync_ThrowArgumentNullException(int id)
        {
            //Arrange
            _mockData.Setup(s => s.PatientAllergies.Get(1)).Returns(default(PatientAllergy));

            //Assert
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                                    new PatientAllergyService(_mockData.Object).DeletePatientAllergyAsync(id));
        }
    }
}
