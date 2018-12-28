using CarRental.Data.Data_Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Bootstrapper
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            AggregateCatalog catalog = new AggregateCatalog();

            // add item to catalog here, point for mef where to look for the assemblies
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(AccountRepository).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(CarRentalEngine).Assembly));

            CompositionContainer container = new CompositionContainer(catalog);

            return container;
        }
    }
}
