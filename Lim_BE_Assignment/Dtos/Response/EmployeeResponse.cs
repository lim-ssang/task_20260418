using Lim_BE_Assignment.Entites;

namespace Lim_BE_Assignment.Dtos.Response
{
    public class EmployeeResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Joined { get; set; }

        public EmployeeResponse()
        {
            
        }

        public EmployeeResponse(Employee employee)
        {
            Name = employee.Name;
            Email = employee.Email;
            Tel = employee.TelNo;
            Joined = employee.JoinedDate;
        }
    }
}
