using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();
            using (context)
            {
                //Console.WriteLine(GetEmployeesInPeriod(context));
                //Console.WriteLine(GetAddressesByTown(context));
                //Console.WriteLine(GetEmployee147(context));
                Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
            }

        }


       // 6. Adding a New Address and Updating Employee

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address newAddress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4

            };
            context.Addresses.Add(newAddress);

            Employee findEmployee = context.Employees
                .First(x => x.LastName == "Nakov");

            findEmployee.Address = newAddress;

            context.SaveChanges();

            string[] employeesAddresses = context.Employees
                .OrderByDescending(x => x.AddressId)
                .Select(x => x.Address.AddressText)
                .Take(10)
                .ToArray();

           string result = String.Join(Environment.NewLine, employeesAddresses);
            return result;

        }

        //7. Employees and Projects

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
                var findEmployees = context.Employees
                .Where(x => x.EmployeesProjects.Any(ep =>
                        ep.Project.StartDate.Year>=2001 && ep.Project.StartDate.Year<=2003))
                .Take(10)
                .Select(x=> new 
                { 
                    x.FirstName,
                    x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects
                       .Select(ep => new
                       { 
                           ProjectName = ep.Project.Name,
                           StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                           EndDate = ep.Project.EndDate.HasValue ?
                           ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt",CultureInfo.InvariantCulture) : "not finished"
                       })
                        .ToList()
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach(var e in findEmployees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");
                foreach (var item in e.Projects)
                {
                    sb.AppendLine($"--{item.ProjectName} - {item.StartDate} - {item.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //8. Addresses by Town

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var allAddresses = context.Addresses
                .OrderByDescending(e => e.Employees.Count)
                .ThenBy(e => e.Town.Name)
                .ThenBy(e => e.AddressText)
                .Take(10)
                .Select(x => new
                {
                    EmployeesCount = x.Employees.Count,
                    TownName = x.Town.Name,
                    AddressText = x.AddressText
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in allAddresses)
            {
                sb.AppendLine($"{item.AddressText}, {item.TownName} - {item.EmployeesCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        //9. Employee 147

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Select(x => new
                {
                    x.EmployeeId,
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    Projects = x.EmployeesProjects
                    .Select(p => new
                    {
                        ProjectName = p.Project.Name
                    })
                .OrderBy(x => x.ProjectName)
                .ToList()
                })
                .Where(x => x.EmployeeId == 147)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in employee)
            {
                sb.AppendLine($"{item.FirstName} {item.LastName} - {item.JobTitle}");
                foreach (var p in item.Projects)
                {
                    sb.AppendLine($"{p.ProjectName}");
                }
            }


            return sb.ToString().TrimEnd();

        }

        //10. Departments with More Than 5 Employees

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var allDepartments = context.Departments
                .Where(x => x.Employees.Count > 5)
                .OrderBy(x => x.Employees.Count)
                .ThenBy(x => x.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees.Select(e => new
                    {
                        EmployeeFirstName = e.FirstName,
                        EmployeeLastName = e.LastName,
                        EmployeeJobTitle = e.JobTitle
                    })
                    .OrderBy(x => x.EmployeeFirstName)
                    .ThenBy(x => x.EmployeeLastName)
                    .ToList()

                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var d in allDepartments)
            {
                sb.AppendLine($"{d.DepartmentName} - {d.ManagerFirstName} {d.ManagerLastName}");

                foreach (var item in d.Employees)
                {
                    sb.AppendLine($"{item.EmployeeFirstName} {item.EmployeeLastName} - {item.EmployeeJobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }    
    }
}
