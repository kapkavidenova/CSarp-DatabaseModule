using CarDealer.Data;
using CarDealer.DTO.ImportDTO;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using CarDealer.DTO.ExportDTO;
using System.Text;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext context = new CarDealerContext();
            // context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //string inputXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //string inputXml = File.ReadAllText("../../../Datasets/parts.xml");
            //string inputXml = File.ReadAllText("../../../Datasets/cars.xml");
            //string inputXml = File.ReadAllText("../../../Datasets/customers.xml");
            //string inputXml = File.ReadAllText("../../../Datasets/sales.xml");


            //string result = ImportSuppliers(context, inputXml);
            //string result = ImportParts(context, inputXml);
            //string result = ImportCars(context, inputXml);
            //string result = ImportCustomers(context, inputXml);
            //string result = ImportSales(context, inputXml);



            //export

            //Query 14. Cars With Distance
            //string result = GetCarsWithDistance(context);

            // Query 15.Cars from make BMW
            //string result = GetCarsWithDistance(context);

            //string result = GetLocalSuppliers(context);
            //string result = GetCarsWithTheirListOfParts(context);
            //string result = GetTotalSalesByCustomer(context);
            //string result = GetSalesWithAppliedDiscount(context);



            Console.WriteLine(result);

        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Suppliers");
            XmlSerializer xmlSerializer = new XmlSerializer(
               typeof(ImportSupplierDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportSupplierDto[] dtos = (ImportSupplierDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Supplier> suppliers = new HashSet<Supplier>();

            foreach (ImportSupplierDto supplierDto in dtos)
            {
                Supplier s = new Supplier()
                {
                    Name = supplierDto.Name,
                    IsImporter = bool.Parse(supplierDto.IsImporter)
                };

                suppliers.Add(s);
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        // Query 10. Import Parts//

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Parts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportPartDto[] dtos = (ImportPartDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Part> parts = new HashSet<Part>();

            foreach (ImportPartDto partDto in dtos)
            {
                Supplier supplier = context
                    .Suppliers
                    .Find(partDto.SupplierId);

                if (supplier == null)
                {
                    continue;
                }

                Part p = new Part()
                {
                    Name = partDto.Name,
                    Price = decimal.Parse(partDto.Price, (CultureInfo.InvariantCulture)),
                    Quantity = partDto.Quantity,



                    Supplier = supplier
                };

                parts.Add(p);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static XmlSerializer GenerateXmlSerializer(string rootName, Type dtoType)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(dtoType, xmlRoot);

            return xmlSerializer;
        }

        //Query 11. Import Cars//

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer =
               GenerateXmlSerializer("Cars", typeof(ImportCarDto[]));
            using StringReader stringReader = new StringReader(inputXml);

            ImportCarDto[] carDtos = (ImportCarDto[])
                xmlSerializer.Deserialize(stringReader);

            ICollection<Car> cars = new HashSet<Car>();
            //ICollection<PartCar> partCars = new HashSet<PartCar>();

            foreach (ImportCarDto carDto in carDtos)
            {
                Car c = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TraveledDistance

                };

                ICollection<PartCar> currentCarParts = new HashSet<PartCar>();

                foreach (int partId in carDto.Parts.Select(p => p.Id).Distinct())
                {
                    Part part = context
                        .Parts
                        .Find(partId);

                    if (part == null)
                    {
                        continue;
                    }

                    PartCar partCar = new PartCar()
                    {
                        Car = c,
                        Part = part
                    };
                    currentCarParts.Add(partCar);
                }

                c.PartCars = currentCarParts;
                cars.Add(c);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //Query 12. Import Customers//

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Customers");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomerDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportCustomerDto[] customerDtos = (ImportCustomerDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Customer> customers = new HashSet<Customer>();

            foreach (ImportCustomerDto customerDto in customerDtos)
            {
                Customer c = new Customer()
                {
                    Name = customerDto.Name,
                    BirthDate = customerDto.BirthDate,
                    IsYoungDriver = customerDto.IsYoungDriver

                };

                customers.Add(c);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();


            return $"Successfully imported {customers.Count}";
        }

        //Query 13. Import Sales//

        public static string ImportSales(CarDealerContext context, string inputXml)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSaleDto[]), new XmlRootAttribute("Sales"));
            ImportSaleDto[] salesDto = (ImportSaleDto[])xmlSerializer.Deserialize(new StringReader(inputXml));

            ICollection<Sale> sales = new HashSet<Sale>();

            salesDto = salesDto
                .Where(s => context.Cars.Any(c => c.Id == s.CarId))
                .ToArray();

            foreach (var saleDto in salesDto)
            {
                Sale sale = new Sale()
                {
                    CarId = saleDto.CarId,
                    CustomerId = saleDto.CustomerId,
                    Discount = saleDto.Discount
                };

                sales.Add(sale);
            }
            ;
            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }


        //3. Query and Export Data

        // Query 14. Cars With Distance//

        public static string GetCarsWithDistance(CarDealerContext context)
        {

            StringBuilder sb = new StringBuilder();

            using StringWriter stringWriter = new StringWriter(sb);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarsWithDistanceDto[]), new XmlRootAttribute("cars"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            ExportCarsWithDistanceDto[] carsDtos =
                 context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new ExportCarsWithDistanceDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TravelledDistance.ToString()
                })
                .ToArray();

            xmlSerializer.Serialize(stringWriter, carsDtos, namespaces);

            return sb.ToString().TrimEnd();
        }     

        }

        //Query 16. Local Suppliers//
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            ExportSupplierDto[] suppliersDtos = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new ExportSupplierDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count

                })
                .ToArray();

            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportSupplierDto[]), new XmlRootAttribute("suppliers"));

            xmlSerializer.Serialize(new StringWriter(sb), suppliersDtos, namespaces);

            return sb.ToString().TrimEnd();

        }
            
        }

        //Query 19. Sales with Applied Discount//

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportSalesWithDiscountDto[]), new XmlRootAttribute("sales"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            ExportSalesWithDiscountDto[] dtos = context.Sales
                .Select(s => new ExportSalesWithDiscountDto()
                {
                    Car = new ExportSalesCarDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TravelledDistance.ToString()

                    },

                    Discount = s.Discount.ToString(CultureInfo.InvariantCulture),
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price).ToString(CultureInfo.InvariantCulture),
                    PriceWitDiscount = (s.Car.PartCars.Sum(pc => pc.Part.Price) -
                                         s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100).ToString(CultureInfo.InvariantCulture)

                })
               .ToArray();

            xmlSerializer.Serialize(new StringWriter(sb), dtos, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}




