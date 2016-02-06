namespace Lab1_WOMU.Migrations
{
    using System;
    using Lab1_WOMU.Models;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<Lab1_WOMU.Models.TankDatabase>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Lab1_WOMU.Models.TankDatabase";
        }

        protected override void Seed(Lab1_WOMU.Models.TankDatabase context)
        {
            var Produkter = new List<Produkt>
            {
                new Produkt { ProduktNamn = "M4 Sherman",   ProduktBeskrivning = "Amerikansk medeltung stridsvagn",
                    Pris = 250000, AntalILager =  39},
                new Produkt { ProduktNamn = "Panzerkampfwagen IV",   ProduktBeskrivning = "Tysk medeltung stridsvagn",
                    Pris = 200000, AntalILager =  154},
                new Produkt { ProduktNamn = "T-34",   ProduktBeskrivning = "Sovjetisk medeltung stridsvagn",
                    Pris = 320080, AntalILager =  15},
                new Produkt { ProduktNamn = "M24 Chaffee",   ProduktBeskrivning = "Amerikansk lätt stridsvagn",
                    Pris = 150000, AntalILager =  54},
                new Produkt { ProduktNamn = "M4A3E2 Jumbo",   ProduktBeskrivning = "Amerikansk medeltung stridsvagn",
                    Pris = 250000, AntalILager =  39},
                new Produkt { ProduktNamn = "M26 Pershing",   ProduktBeskrivning = "Amerikansk tung stridsvagn",
                    Pris = 350000, AntalILager =  11}
            };
            Produkter.ForEach(s => context.Produkter.AddOrUpdate(p => p.ProduktNamn, s));
            context.SaveChanges();


            var Kunds = new List<Kund>
            {
                new Kund {FörNamn = "Test", Efternamn = "Testing", Epost = "test@Test.com", Ort = "TestTown", PostAdress = "Test 1", PostNr = 51299, Telefonnummer = 123456789 },
                new Kund {FörNamn = "Test2", Efternamn = "Testing", Epost = "test2@Test.com", Ort = "TestTown", PostAdress = "Test 2", PostNr = 51299, Telefonnummer = 987654321 },
                new Kund {FörNamn = "Test3", Efternamn = "Testing", Epost = "test3@Test.com", Ort = "TestTown", PostAdress = "Test 3", PostNr = 51299, Telefonnummer = 456789123 },
            };
            Kunds.ForEach(s => context.Kund.AddOrUpdate(p => p.Epost, s));
            context.SaveChanges();

            var Orders = new List<Order>
            {
                new Order { OrderDate = DateTime.Now, OrderID = 1, Total = 812500.0},
                new Order { OrderDate = DateTime.Now, OrderID = 2, Total = 2312500.0},
                new Order { OrderDate = DateTime.Now, OrderID = 3, Total = 5125000.0},
            };
            Orders.ForEach(s => context.Order.AddOrUpdate(p => p.OrderID, s));
            context.SaveChanges();

            var OrderRader = new List<OrderRad>
            {
                new OrderRad {OrderID = Orders.Single(o => o.OrderID == 1).OrderID,
                    ProduktID = Produkter.Single(p => p.ProduktNamn == "M4 Sherman").ProduktID,
                 Antal = 2, TotalPris = 500000},
               new OrderRad {OrderID = Orders.Single(o => o.OrderID == 1).OrderID,
                    ProduktID = Produkter.Single(p => p.ProduktNamn == "M24 Chaffee").ProduktID,
                 Antal = 1, TotalPris = 150000},
               new OrderRad {OrderID = Orders.Single(o => o.OrderID == 2).OrderID,
                    ProduktID = Produkter.Single(p => p.ProduktNamn == "M4 Sherman").ProduktID,
                 Antal = 5, TotalPris = 1250000},
               new OrderRad {OrderID = Orders.Single(o => o.OrderID == 2).OrderID,
                    ProduktID = Produkter.Single(p => p.ProduktNamn == "Panzerkampfwagen IV").ProduktID,
                 Antal = 3, TotalPris = 600000},
               new OrderRad {OrderID = Orders.Single(o => o.OrderID == 3).OrderID,
                    ProduktID = Produkter.Single(p => p.ProduktNamn == "M4 Sherman").ProduktID,
                 Antal = 10, TotalPris =  2500000},
               new OrderRad {OrderID = Orders.Single(o => o.OrderID == 3).OrderID,
                    ProduktID = Produkter.Single(p => p.ProduktNamn == "M4A3E2 Jumbo").ProduktID,
                 Antal = 5, TotalPris = 1250000},
               new OrderRad {OrderID = Orders.Single(o => o.OrderID == 3).OrderID,
                    ProduktID = Produkter.Single(p => p.ProduktNamn == "M26 Pershing").ProduktID,
                 Antal = 1, TotalPris = 350000}
            };
            foreach (OrderRad e in OrderRader)
            {
                var enrollmentInDataBase = context.OrderRad.Where(
                    s => s.OrderID == e.OrderID && s.ProduktID == e.ProduktID);
                if (enrollmentInDataBase == null)
                {
                    context.OrderRad.Add(e);
                }
            }
        }
    }
}
