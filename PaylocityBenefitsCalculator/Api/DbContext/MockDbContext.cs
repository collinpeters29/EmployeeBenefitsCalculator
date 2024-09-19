using Api.Data;
using Api.Models;

namespace Api.DbContext
{
    public class MockDbContext
    {

        // Mock DB Context Class to act as the "database" for the Employees and their Dependents

        //Mock "Db Sets" to hold
        public List<Employee> EmployeeList { get; set; }
        public List<Dependent> DependentList { get; set; }


        public MockDbContext() {
            EmployeeList = new List<Employee>();
            DependentList = new List<Dependent>();
            SeedMockContext();
            
        }

        //Mock Single Entity to DB
        public void AddEmployee(Employee employee)
        {
            EmployeeList.Add(employee);
            foreach (Dependent dependent in DependentList)
            {
                DependentList.Add(dependent);
            }
        }

        // Mock Adding Single Entity to DB
        public void AddDependent(Dependent dependent)
        {
            DependentList.Add(dependent);
        }

        //Mock Pulling Single Entity By ID
        public Employee GetEmployee(int id)
        {
            var employee = EmployeeList.Where(e => e.Id == id).FirstOrDefault();
            if (employee == null) { return null; }
            else
            {
                employee.Dependents = DependentList.Where(e => e.EmployeeId == employee.Id).ToList();
            }
            return employee;
        }

        //Mock Pulling All Entities
        public List<Employee> GetEmployees()
        {
            var employeeList = EmployeeList;
            foreach (Employee employee in employeeList)
            {
                employee.Dependents = DependentList.Where(e => e.EmployeeId == employee.Id).ToList();
            }

            return employeeList;
        }
        //Mock Pulling Single Entity By ID
        public List<Dependent> GetDependentsUnderEmployee(int id)
        {
            var dependents = DependentList.Where(e => e.EmployeeId == id).ToList();
            if (dependents == null) { return null; }
            
            return dependents;
        }

        //Mock Pulling Single Entity By ForeignKey EmployeeId
        public Dependent GetDependent(int id)
        {
            var dependent = DependentList.Where(e => e.Id == id).FirstOrDefault();
            if (dependent == null) { return null; }

            return dependent;
        }
        //Mock Pulling All Entities
        public List<Dependent> GetDependents()
        {
            var dependents = DependentList;
            return dependents;
        }

        private void SeedMockContext()
        {                        
            EmployeeList.AddRange(new List<Employee> {
                DataFactory.GenerateEmployee(1, "LeBron", "James", 75420.99m, new DateTime(1984, 12, 30)),
                DataFactory.GenerateEmployee(2, "Ja", "Morant", 92365.22m, new DateTime(1999, 8, 10)),
                DataFactory.GenerateEmployee(3, "Michael", "Jordan", 143211.12m, new DateTime(1963, 2, 17))
            });

            DependentList.AddRange(new List<Dependent> {
                DataFactory.GenerateDependent(1,2, "Spouse", "Morant", new DateTime(1998, 3, 3), Relationship.Spouse),
                DataFactory.GenerateDependent(2,2, "Child1", "Morant", new DateTime(2020, 6, 23), Relationship.Child),
                DataFactory.GenerateDependent(3,2, "Child2", "Morant", new DateTime(2021, 5, 18), Relationship.Child),
                DataFactory.GenerateDependent(4,3, "DP", "Jordan", new DateTime(1974, 1, 2), Relationship.DomesticPartner)
            });

        }
    }
}
