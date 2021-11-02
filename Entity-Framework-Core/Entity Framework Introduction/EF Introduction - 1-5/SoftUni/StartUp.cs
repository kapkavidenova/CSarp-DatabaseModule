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
                //Console.WriteLine(GetEmployeesFullInformation(context));
                //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
                Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
            }
        }

        //3. Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .OrderBy(x => x.EmployeeId)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var item in employees)
            {
                sb.AppendLine($"{item.FirstName} {item.LastName} {item.MiddleName} {item.JobTitle} {item.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        //4. Employees with Salary Over 50 000

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(x => x.Salary > 50000)
                .OrderBy(x => x.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.Salary:F2}");

            }

            return sb.ToString().TrimEnd();
        }

        // 5. Employees from Research and Development

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.Department,
                    x.Salary
                })
                .Where(x => x.Department.Name == "Research and Development")
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}