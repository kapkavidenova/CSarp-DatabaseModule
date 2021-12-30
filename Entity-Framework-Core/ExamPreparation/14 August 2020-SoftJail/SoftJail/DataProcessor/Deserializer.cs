namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string SUCCESSFULLY_ADDED_OFFICER = "Imported {0} ({1} prisoners)";
        private const string ERROR_MESSAGE = "Invalid Data";
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var dtos = JsonConvert.DeserializeObject<ICollection<ImportDepartmentDto>>(jsonString);

            HashSet<Department> departments = new HashSet<Department>();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto)
                    || !dto.Cells.All(IsValid)
                    || !dto.Cells.Any())
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var department = new Department
                {
                    Name = dto.Name,
                    Cells = dto.Cells.Select(x => new Cell
                    {
                        CellNumber = x.CellNumber,
                        HasWindow = x.HasWindow
                    })
                     .ToList()
                };
                departments.Add(department);

                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.Departments.AddRange(departments);
            // context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var dtos = JsonConvert.DeserializeObject<ImportPrisonerDto[]>(jsonString);

            HashSet<Prisoner> prisoners = new HashSet<Prisoner>();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto)
                   || !dto.Mails.Any(IsValid) || dto.Mails == null)

                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var incarcerationDate = DateTime.ParseExact(dto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                bool IsValidReleaseDate = DateTime.TryParseExact(dto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);

                var prisoner = new Prisoner
                {
                    FullName = dto.FullName,
                    Nickname = dto.Nickname,
                    Age = dto.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDate,
                    Bail = dto.Bail,
                    CellId = dto.CellId,
                    Mails = dto.Mails.Select(x => new Mail
                    {
                        Description = x.Description,
                        Sender = x.Sender,
                        Address = x.Address
                    }).ToList()

                };
                prisoners.Add(prisoner);

                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            // context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Officers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportOfficerDto[]), xmlRoot);

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringReader sr = new StringReader(xmlString);

            ImportOfficerDto[] dtos = (ImportOfficerDto[])
                xmlSerializer.Deserialize(sr);

            HashSet<Officer> officers = new HashSet<Officer>();
            foreach (var dto in dtos)
            {

                if (IsValid(dto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Position position;
                bool IsValidEnum = Enum.TryParse<Position>(dto.Position, out position);

                Weapon weapon;
                 IsValidEnum = Enum.TryParse<Weapon>(dto.Weapon, out weapon);

                if (!IsValidEnum)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Officer officer = new Officer()
                {
                    FullName = dto.FullName,
                    Salary = dto.Money,
                    Position = position,
                    Weapon = weapon,

                    DepartmentId = dto.DepartmentId
                };

                List<OfficerPrisoner> officerPrisoners = new List<OfficerPrisoner>();

                foreach (var prisonerDto in dto.Prisoners)
                {
                    //officerPrisoners.Add(new OfficerPrisoner()
                    var prisoner = new OfficerPrisoner()
                    {
                        Officer = officer,
                        PrisonerId = prisonerDto.Id
                    };
                    officer.OfficerPrisoners.Add(prisoner);
                }           

                officers.Add(officer);

                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();



        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}