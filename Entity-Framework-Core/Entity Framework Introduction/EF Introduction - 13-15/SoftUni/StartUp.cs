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
               //(GetEmployeesByFirstNameStartingWithSa(context));
               //Console.WriteLine(DeleteProjectById(context));
                Console.WriteLine(RemoveTown(context));
            }

        }

        //13. Find Employees by First Name Starting with "Sa"

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var findEmployees = context.Employees
                .Where(x => x.FirstName.ToLower().StartsWith("sa"))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    x.Salary
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var e in findEmployees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }

        // 14. Delete Project by Id
        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectDelete = context.Projects
                .Find(2);

            var project = context.EmployeesProjects.Where(ep => ep.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(project);
            context.Projects.Remove(projectDelete);
            context.SaveChanges();

            var sb = new StringBuilder();
            var projectsToDisplay = context.Projects
                .Select(x => x.Name)
                .Take(10)
                .ToList();

            foreach (var item in projectsToDisplay)
            {
                sb.AppendLine(item);
            }

            return sb.ToString().TrimEnd();
        }

        //15. Remove Town

        public static string RemoveTown(SoftUniContext context)

         {
            var findEmployees = context.Employees
                .Where(e => e.Address.Town.Name == "Seattle");

            foreach (var item in findEmployees)
            {
                item.AddressId = null;
            }

            var addressesToDelete = context.Addresses
                .Where(t => t.Town.Name == "Seattle");

            int count = addressesToDelete.Count();
            context.Addresses.RemoveRange(addressesToDelete);

            var townToDelete = context.Towns
                .Where(x => x.Name == "Seattle")
                .ToList();

                context.Towns.Remove(townToDelete.First());
                context.SaveChanges();

            return $"{count} addresses in Seattle were deleted";
        }
    }
}
