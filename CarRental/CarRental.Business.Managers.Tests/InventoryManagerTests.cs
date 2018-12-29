using System;
using System.Security.Principal;
using System.Threading;
using CarRental.Business.Entities;
using CarRental.Data.Contracts.Repository_Interfaces;
using Core.Common.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CarRental.Business.Managers.Tests
{
    [TestClass]
    public class InventoryManagerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            GenericPrincipal principal = new GenericPrincipal(new GenericIdentity("Kamil"), 
                new string[] { "CarRentalAdmin"});

            Thread.CurrentPrincipal = principal;
        }


        [TestMethod]
        public void UpdateCar_add_new()
        {
            Car newCar = new Car();
            Car addedCar = new Car { CarId = 1 };

            Mock<IDataRepositoryFactory> mockRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockRepositoryFactory.Setup(it => it.GetDataRepository<ICarRepository>().Add(newCar)).Returns(addedCar);

            InventoryManager inventoryManager = new InventoryManager(mockRepositoryFactory.Object);

            Car result = inventoryManager.UpdateCar(newCar);

            Assert.IsTrue(result == addedCar);
        }

        [TestMethod]
        public void UpdateCar_update_existing()
        {
            Car existingCar = new Car { CarId = 1 };
            Car updatedCar = new Car { CarId = 1 };

            Mock<IDataRepositoryFactory> mockRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockRepositoryFactory.Setup(it => it.GetDataRepository<ICarRepository>().Update(existingCar)).Returns(updatedCar);

            InventoryManager inventoryManager = new InventoryManager(mockRepositoryFactory.Object);

            Car result = inventoryManager.UpdateCar(existingCar);

            Assert.IsTrue(result == updatedCar);
        }
    }
}
