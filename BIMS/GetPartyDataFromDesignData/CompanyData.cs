using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetPartyDataFromDesignData
{
    public class CompanyData
    {
        public CompanyData()
        {
            Employees = new List<Employee>();
        }
        public void AddAnEmployee(string name, string email)
        {
            if (Employees.Count(e=>e.Name.Equals(name)) == 0)
            {
                Employees.Add(new Employee() { Name = name, Email = email });
            }
        }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public List<Employee> Employees { get; set; }
    }
    public class Employee
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
