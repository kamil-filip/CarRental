﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Proxies.Tests
{
    [TestClass]
    public class ServiceAccessTests
    {
        [TestMethod]
        public void test_inventory_client_connection()
        {
            InventoryClient proxy = new InventoryClient();

            proxy.Open();
        }

        [TestMethod]
        public void test_account_client_connection()
        {
            AccountClient proxy = new AccountClient();

            proxy.Open();
        }

        [TestMethod]
        public void test_rental_client_connection()
        {
            RentalClient proxy = new RentalClient();

            proxy.Open();
        }

    }
}
