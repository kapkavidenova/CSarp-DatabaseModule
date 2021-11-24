using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //var supplierJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //var partsJson = File.ReadAllText("../../../Datasets/parts.json");
            //var carsJson = File.ReadAllText("../../../Datasets/cars.json");
            //var customersJson = File.ReadAllText("../../../Datasets/customers.json");
            //var salesJson = File.ReadAllText("../../../Datasets/sales.json");



            //Console.WriteLine(ImportSuppliers(context, supplierJson));
            //Console.WriteLine(ImportParts(context, partsJson));
            //Console.WriteLine(ImportCars(context, carsJson));
            //Console.WriteLine(ImportCustomers(context, customersJson));
            //Console.WriteLine(ImportSales(context,salesJson));

            //Export
            // Console.WriteLine(GetOrderedCustomers(context));
            //Console.WriteLine(GetCarsFromMakeToyota(context));
            //Console.WriteLine(GetLocalSuppliers(context));
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));
            Console.WriteLine(GetTotalSalesByCustomer(context));
            //Console.WriteLine(GetSalesWithAppliedDiscount(context));





        }

        //Query 8. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var supplierDtos = JsonConvert.DeserializeObject<IEnumerable<ImportSupplierInputModel>>(inputJson);

            var suppliers = supplierDtos
                .Select(x => new Supplier
                {
                    Name = x.Name,
                    IsImporter = x.IsImporter
                })
                .ToList();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        //Query 9. Import Parts

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var suppliedIds = context.Suppliers.Select(x => x.Id).ToArray();

            var partsDto = JsonConvert.DeserializeObject<IEnumerable<ImportPartInputModel>>(inputJson);



            var parts = partsDto
                .Where(x => suppliedIds.Contains(x.SupplierId))
                .Select(s => new Part
                {
                    Name = s.Name,
                    Price = s.Price,

                    Quantity = s.Quantity,

                    SupplierId = s.SupplierId
                })
                .ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
        }

        //Query 10. Import Cars

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDtos = JsonConvert.DeserializeObject<IEnumerable<CarDTO>>(inputJson);

            var listOfCars = new List<Car>();

            foreach (var car in carsDtos)
            {
                var currentCar = new Car()
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance
                };

                foreach (var partId in car?.PartsId.Distinct())
                {
                    currentCar.PartCars.Add(new PartCar
                    {
                        PartId = partId
                    });
                }

                listOfCars.Add(currentCar);
            }

            context.Cars.AddRange(listOfCars);
            context.SaveChanges();

            return $"Successfully imported {listOfCars.Count}.";
        }

        //Query 11. Import Customers

       public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            //var customersDtos = JsonConvert.DeserializeObject<IEnumerable<CustomDTO>>(inputJson);

            //var customers = customersDtos.Select(c => new Customer
            //{
            //    Name = c.Name,
            //    BirthDate = c.BirthDay,
            //    IsYoungDriver = c.IsYoungDriver
            //})
            //    .ToList();
            var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

            context.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";

        }

        // Query 12. Import Sales

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var salesDtos = JsonConvert.DeserializeObject<IEnumerable<SaleDTO>>(inputJson);

            var sales = salesDtos.Select(s => new Sale
            {
                CarId = s.Carid,
                CustomerId = s.CustomerId,
                Discount = s.Discount
            })
                .ToList();

            context.AddRange(sales);
            context.SaveChanges();

           return $"Successfully imported {sales.Count}.";
        }

        //Query and Export Data
        //Query 13. Export Ordered Customers

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(d => d.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(c => new 
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToList();

            var result = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return result;

        }

        //Export Cars from Make Toyota


        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {

            var cars = context
                .Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(x => new
                {
                    Id = x.Id,
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                 .ToList();

            var result = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return result;
        }

        //Query 14. Export Local Suppliers

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count()
                })
                .ToList();

            var result = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return result;
        }

        //Query 15. Export Cars with Their List of Parts

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance
                    },
                    parts = c.PartCars
                    .Select(a => new
                    {
                        Name = a.Part.Name,
                        Price = $"{a.Part.Price:f2}"
                    }),
                })
                .ToList();

            var result = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return result;
        }

        //Query 16. Export Total Sales by Customer

        //public static string GetTotalSalesByCustomer(CarDealerContext context)
        //{
        //    var customers = context.Customers
        //        .Where(s => s.Sales.Count > 0)
        //        .Select(x => new
        //        {
        //            FullName = x.Name,
        //            bougthCars = x.Sales.Count,
        //            spentMoney = x.Sales
        //            .SelectMany(s => s.Car.PartCars.Select(pc => pc.Part.Price)).Sum()

        //        })
        //        .OrderByDescending(s => s.bougthCars)
        //        .ThenByDescending(s => s.spentMoney)
        //        .ToList();

        //    var result = JsonConvert.SerializeObject(customers, Formatting.Indented);


        //    return result;
     
        //}

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context
                .Customers
                .Where(c => c.Sales.Any(s => s.CustomerId == c.Id))
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count(),
                    spentMoney = c.Sales.Sum(y => y.Car.PartCars.Sum(z => z.Part.Price))
                })
                .OrderByDescending(x => x.spentMoney)
                .ThenByDescending(x => x.boughtCars)
                .ToList();

            var result = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return result;
        }

        //Query 16. Export Total Sales by Customer

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(c => new
                {
                    car = new
                    {
                        Make = c.Car.Make,
                        Model = c.Car.Model,
                        TravelledDistance = c.Car.TravelledDistance
                    },
                    customerName = c.Customer.Name,
                    Discount = c.Discount.ToString("f2"),
                    price = (c.Car.PartCars.Sum(pc => pc.Part.Price)).ToString("f2"),
                    priceWithDiscount =
                                    (c.Car.PartCars.Sum(pc => pc.Part.Price) * (1 - (c.Discount / 100)))
                                    .ToString("f2")

                })
                .ToList();

            var result = JsonConvert.SerializeObject(sales, Formatting.Indented);
            return result;
        }
    }
}