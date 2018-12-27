using Core.Common.Contracts;

namespace CarRental.Data.Contracts.Repository_Interfaces
{
    public interface IDataRepositoryFactory
    {
        T GetDataRepository<T>() where T : IDataRepository;
    }
}
