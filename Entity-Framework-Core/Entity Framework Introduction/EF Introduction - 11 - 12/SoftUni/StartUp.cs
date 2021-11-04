using SoftUni.Data;
using System;
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
               // Console.WriteLine(GetLatestProjects(context));
                Console.WriteLine(IncreaseSalaries(context));
            }
        }


       // 11. find latest 10 projects

        public static string GetLatestProjects(SoftUniContext context)
        {
            var allProjects = context.Projects
                .OrderByDescending(x => x.StartDate)
                .Take(10)
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    x.StartDate 
                    //= x.startdate.tostring("m/d/yyyy h:mm:ss tt", cultureinfo.invariantculture),
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var item in allProjects)
            {
                sb.AppendLine($"{item.Name}");
                sb.AppendLine($"{item.Description}");
                sb.AppendLine($"{item.StartDate}");

            }

            return sb.ToString().TrimEnd();
        }


        //12. Increase Salaries    

        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

          var findEmployees = context
                .Employees
                .Where(e => e.Department.Name == "Engineering" ||
                            e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" ||
                            e.Department.Name == "Information Services");

            foreach (var e in findEmployees)
            {
                e.Salary = e.Salary * 1.12m;
            }

            context.SaveChanges();

            var displayedEmployees = findEmployees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            foreach (var e in displayedEmployees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

    }
}
