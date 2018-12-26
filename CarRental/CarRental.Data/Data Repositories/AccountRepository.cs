using System.Collections.Generic;
using CarRental.Business.Entieties;
using System.Linq;


namespace CarRental.Data.Data_Repositories
{
    public class AccountRepository : DataRepositoryBase<Account>
    {
        protected override Account AddEntity(CarRentalContext entityContext, Account entity)
        {
            return entityContext.AccountSet.Add(entity);
        }

        protected override IEnumerable<Account> GetEntities(CarRentalContext entityContext)
        {
            return from e in entityContext.AccountSet select e;
        }

        protected override Account GetEntity(CarRentalContext entityContext, int id)
        {
            var query = (from e in entityContext.AccountSet where e.AccountId == id
                        select e);

            var results = query.FirstOrDefault();

            return results;
        }

        protected override Account UpdateEntity(CarRentalContext entityContext, Account entity)
        {
            return (from e in entityContext.AccountSet
                    where e.AccountId == entity.AccountId
                    select e).FirstOrDefault();
        }
    }
}
