using CarRental.Business.Entities;
using Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Contracts
{
    [ServiceContract] // to make it as a service contract
    public interface IInventoryService
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))] // for specifc exceptions
        Car GetCar(int carId);

        [OperationContract]
        Car[] GetAllCars();

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)] // as they are IO it would require transaction handling
        Car UpdateCar(Car car);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)] // as they are IO it would require transaction handling
        void DeleteCar(int carId);

        [OperationContract]
        Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate);
    }
}
