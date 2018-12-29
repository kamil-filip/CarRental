using CarRental.Business.Common;
using CarRental.Business.Contracts;
using CarRental.Business.Entities;
using CarRental.Common;
using CarRental.Data.Contracts.Repository_Interfaces;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Managers
{
    //TODO in wcf we cannot have unhandled exception as it's gonna crash the proxu

    // IF WCF is not decorated with any specific behaviour, the service
    // is decorated with per session mode, which means that the lifetime
    // of the service is gonna be same long as proxy
    // it means that so long the proxy is open all calls will be executed
    // on the same instance of the service
    // its not scalable as instance of the service is in the memory as long as proxy is open

    // post construction resolve using mef
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, 
        ConcurrencyMode = ConcurrencyMode.Multiple, 
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class InventoryManager : ManagerBase, IInventoryService
    {
        public InventoryManager() // wcf cares only about default constructor
        {
        }


        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        public InventoryManager(IBusinessEngineFactory businessEngineFactory)
        {
            _BusinessEngineFactory = businessEngineFactory;
        }

        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }

        [Import] // in oderd to make it initialized by MEF
        IDataRepositoryFactory _DataRepositoryFactory;

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Car GetCar(int carId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();

                Car carEntity = carRepository.Get(carId);
                if (carEntity == null)
                {
                    NotFoundException ex
                        = new NotFoundException($"Car with id {carId} is not stored int the data base");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                return carEntity;
            });
        }

        //TODO check what happens when timeout, and this catches
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Car[] GetAllCars()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository =
                           _DataRepositoryFactory.GetDataRepository<ICarRepository>();

                IRentalRepository rentalRepository =
                    _DataRepositoryFactory.GetDataRepository<IRentalRepository>();


                IEnumerable<Car> cars = carRepository.Get();
                IEnumerable<Rental> rentedCars = rentalRepository.GetCurrentlyRentedCars();

                foreach (Car car in cars)
                {
                    Rental rentedCar = rentedCars.Where(it => it.CarId == car.CarId).FirstOrDefault();
                    car.CurrentlyRented = (rentedCars != null);
                }

                return cars.ToArray();
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public Car UpdateCar(Car car)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository =
                    _DataRepositoryFactory.GetDataRepository<ICarRepository>();

                Car updatedEntity = null;

                if (car.CarId == 0)
                    updatedEntity = carRepository.Add(car);
                else
                    updatedEntity = carRepository.Update(car);

                return updatedEntity;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public void DeleteCar(int carId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();

                carRepository.Remove(carId);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate)
        {
           return ExecuteFaultHandledOperation(() =>
           {
               var carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();
               var rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
               var reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
               var carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

               IEnumerable<Car> allCars = carRepository.Get();
               IEnumerable<Rental> rentedCars = rentalRepository.GetCurrentlyRentedCars();
               IEnumerable<Reservation> reservedCars = reservationRepository.Get();

               List<Car> availableCars = new List<Car>();

               foreach(var car in allCars)
               {
                   if (carRentalEngine.IsCarAvailableForRental(
                       car.CarId, pickupDate, returnDate, rentedCars,reservedCars))
                       availableCars.Add(car);
               }

               return availableCars.ToArray();
           });
        }
    }
}
