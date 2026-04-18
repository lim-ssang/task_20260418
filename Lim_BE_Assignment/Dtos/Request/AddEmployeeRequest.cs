using Lim_BE_Assignment.Entites;

namespace Lim_BE_Assignment.Dtos.Request
{
    public class AddEmployeeRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Joined { get; set; }

        public Employee ToEntity()
        {
            return new Employee()
            {
                Name = this.Name,
                Email = this.Email,
                TelNo = this.Tel,
                JoinedDate = Joined
            };
        }
    }    
}
