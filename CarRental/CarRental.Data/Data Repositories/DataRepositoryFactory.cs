using CarRental.Data.Contracts.Repository_Interfaces;
using Core.Common.Contracts;
using Core.Common.Core;
using System.ComponentModel.Composition;

namespace CarRental.Data.Data_Repositories
{
    [Export(typeof(IDataRepositoryFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)] // disable singleton
    public class DataRepositoryFactory : IDataRepositoryFactory
    {
        public T GetDataRepository<T>() where T : IDataRepository
        {
            return ObjectBase.Container.GetExportedValue<T>();
        }
    }
}
