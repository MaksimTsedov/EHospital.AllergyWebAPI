using EHospital.Allergies.BusinesLogic.Services;
using EHospital.Allergies.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EHospital.Allergies.Tests
{
    [TestClass]
    public class AllergySymptomServiceTest
    {
        private static Mock<IRepository<AllergySymptom>> _mockRepo;
        private static Mock<IUnitOfWork> _mockData;
        private List<Symptom> _symptomList;
        private List<AllergySymptom> _allergySymptomList;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepo = new Mock<IRepository<AllergySymptom>>();
            _mockData = new Mock<IUnitOfWork>();
            _mockData.Setup(s => s.AllergySymptoms).Returns(_mockRepo.Object);
            _symptomList = new List<Symptom>() {
           new Symptom { Id = 1, Naming = "prisma" },
           new Symptom { Id = 2, Naming = "pomidor" },
           new Symptom { Id = 3, Naming = "abrikos" }
            };
            _allergySymptomList = new List<AllergySymptom>() {
           new AllergySymptom { Id = 1, PatientAllergyId = 1, SymptomId = 1 },
           new AllergySymptom { Id = 2, PatientAllergyId = 1, SymptomId = 2 },
           new AllergySymptom { Id = 3, PatientAllergyId = 2, SymptomId = 3 }
          };
        }

        [TestMethod]
        public void AllergySymptoms_GetAllAllergySymptoms_PatientAllergyIs_1_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.AllergySymptoms.GetAll(It.IsAny<Expression<Func<AllergySymptom, bool>>>()))
                                            .Returns(_allergySymptomList.Where(a => a.PatientAllergyId == 1).AsQueryable);
            _mockData.Setup(s => s.Symptoms.GetAll()).Returns(_symptomList.AsQueryable);


            //Act
            var actual = new AllergySymptomService(_mockData.Object).GetAllAllergySymptoms(1).ToList();

            //Assert
            Assert.AreEqual(actual.Count(), 2);
            Assert.AreEqual("pomidor", actual[0].Naming);
            Assert.AreEqual("prisma", actual[1].Naming);
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(0)]
        [DataRow(-2)]
        [ExpectedException(typeof(ArgumentException))]
        public void AllergySymptoms_GetAllAllergySymptoms_ThrowArgumentException(int id)
        {
            //Arrange
            _mockData.Setup(s => s.AllergySymptoms.GetAll(It.IsAny<Expression<Func<AllergySymptom, bool>>>()))
                                            .Returns(_allergySymptomList.Where(a => a.PatientAllergyId == id).AsQueryable);
            _mockData.Setup(s => s.Symptoms.GetAll()).Returns(_symptomList.AsQueryable);

            //Act
            var actual = new AllergySymptomService(_mockData.Object).GetAllAllergySymptoms(id).ToList();
        }

        [TestMethod]
        public void AllergySymptoms_CreateAllergySymptomAsync_Correct()
        {
            //Arrange
            int id = 4;
            AllergySymptom testAllergySymptom = new AllergySymptom { PatientAllergyId = 1, SymptomId = 3 };

            _mockData.Setup(s => s.Symptoms.Get(testAllergySymptom.SymptomId))
                                           .Returns(Task.FromResult(_symptomList[2]));
            _mockData.Setup(s => s.PatientAllergies.Get(testAllergySymptom.PatientAllergyId))
                                                   .Returns(Task.FromResult(new PatientAllergy { Id = 1 }));
            _mockData.Setup(s => s.AllergySymptoms.GetAll())
                                            .Returns(_allergySymptomList.AsQueryable);
            _mockData.Setup(s => s.AllergySymptoms.Insert(testAllergySymptom)).Returns((AllergySymptom a) =>
            {
                a.Id = id;
                return a;
            });


            //Act
            var actual = new AllergySymptomService(_mockData.Object).CreateAllergySymptomAsync(testAllergySymptom).Result;

            //Assert
            Assert.AreEqual(testAllergySymptom, actual);
            _mockData.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        [DataRow(1, 4)]
        [DataRow(3, 2)]
        [DataRow(0, 0)]
        [DataRow(-1, 1)]
        [DataRow(1, -1)]
        [DataRow(-1, -1)]       
        public void AllergySymptoms_CreateAllergySymptomAsync_ThrowArgumentNullException(int patientAllergyId, int symptomId)
        {
            //Arrange
            AllergySymptom testAllergySymptom = new AllergySymptom { PatientAllergyId = patientAllergyId, SymptomId = symptomId };

            _mockData.Setup(s => s.Symptoms.Get(testAllergySymptom.SymptomId))
                                           .Returns(Task.FromResult(default(Symptom)));
            _mockData.Setup(s => s.PatientAllergies.Get(testAllergySymptom.PatientAllergyId))
                                                   .Returns(Task.FromResult(default(PatientAllergy)));

            //Assert
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => 
                                    new AllergySymptomService(_mockData.Object)
                                    .CreateAllergySymptomAsync(testAllergySymptom));
        }

        [TestMethod]
        public void AllergySymptoms_CreateAllergySymptomAsync_ThrowArgumentException()
        {
            //Arrange
            int id = 4;
            AllergySymptom testAllergySymptom = new AllergySymptom { PatientAllergyId = 1, SymptomId = 2 };

            _mockData.Setup(s => s.Symptoms.Get(testAllergySymptom.SymptomId))
                                           .Returns(Task.FromResult(_symptomList[1]));
            _mockData.Setup(s => s.PatientAllergies.Get(testAllergySymptom.PatientAllergyId))
                                                   .Returns(Task.FromResult(new PatientAllergy { Id = 1 }));
            _mockData.Setup(s => s.AllergySymptoms.GetAll())
                                            .Returns(_allergySymptomList.AsQueryable);
            _mockData.Setup(s => s.AllergySymptoms.Insert(testAllergySymptom)).Returns((AllergySymptom a) =>
            {
                a.Id = id;
                return a;
            });

            //Assert
            Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                                    new AllergySymptomService(_mockData.Object)
                                    .CreateAllergySymptomAsync(testAllergySymptom));
        }

        [TestMethod]
        public async Task AllergySymptoms_GetAllergySymptom_IdIs_2_Correct()
        {
            //Arrange

            _mockData.Setup(s => s.AllergySymptoms.Include<Symptom>(It.IsAny<Expression<Func<AllergySymptom, Symptom>>>()))
                                                  .Returns(_allergySymptomList.AsQueryable().Include(a => a.Symptom));

            //Act
            var actual = await new AllergySymptomService(_mockData.Object).GetAllergySymptom(2);

            //Assert
            Assert.AreEqual(actual, _allergySymptomList[1]);
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(0)]
        [DataRow(-2)]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task AllergySymptoms_GetAllergySymptom_ThrowArgumentNullException(int id)
        {
            //Arrange
            _mockData.Setup(s => s.AllergySymptoms.Include<Symptom>(It.IsAny<Expression<Func<AllergySymptom, Symptom>>>()))
                                                  .Returns(_allergySymptomList.AsQueryable().Include(a => a.Symptom));

            //Act
            var actual = await new AllergySymptomService(_mockData.Object).GetAllergySymptom(id);
        }

        [TestMethod]
        public async Task AllergySymptoms_DeleteAllergySymptomAsync_IdIs_1_Correct()
        {
            //Arrange
            _mockData.Setup(s => s.AllergySymptoms.Include<Symptom>(It.IsAny<Expression<Func<AllergySymptom, Symptom>>>()))
                                                  .Returns(_allergySymptomList.AsQueryable().Include(a => a.Symptom));
            _mockData.Setup(s => s.AllergySymptoms.Get(1)).Returns(Task.FromResult(_allergySymptomList[0]));

            //Act
            var actual = await new AllergySymptomService(_mockData.Object).DeleteAllergySymptomAsync(1);

            //Assert
            Assert.IsTrue(actual.IsDeleted);
            _mockData.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        [DataRow(6)]
        [DataRow(0)]
        [DataRow(-2)]
        public void AllergySymptoms_DeleteAllergySymptomAsync_ThrowArgumentNullException(int id)
        {
            //Arrange
            _mockData.Setup(s => s.AllergySymptoms.Get(1)).Returns(Task.FromResult(default(AllergySymptom)));

            //Assert
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => 
                                    new AllergySymptomService(_mockData.Object).DeleteAllergySymptomAsync(id));
        }
    }
}
