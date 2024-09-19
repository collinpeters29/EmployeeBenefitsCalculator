using Api.Models;

namespace Api.Data
{
    public static class DataFactory
    {

        public static Employee GenerateEmployee(int id, string firstName, string lastName, decimal salary, DateTime dateOfBirth)
        {
            Employee employee = new Employee { 
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Salary = salary,
                DateOfBirth = dateOfBirth
            };
            return employee;
        } 

        public static Dependent GenerateDependent(int id, int employeeId, string firstName, string lastname, DateTime dateOfBirth, Relationship relationship)
        {

            Dependent dependent = new Dependent
            {
                Id = id,
                EmployeeId = employeeId,
                FirstName = firstName,
                LastName = lastname,
                DateOfBirth = dateOfBirth,
                Relationship = relationship
            };
            return dependent;


        }

    }
}
