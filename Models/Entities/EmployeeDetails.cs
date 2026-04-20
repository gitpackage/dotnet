namespace EmployeeDatabase.Models.Entities
{
	public class EmployeeDetails
	{

		public Guid Id { get; set; }
		public required String FirstName { get; set; }
		public required String LastName { get; set; }
		public String? Email { get; set; }

	}

}
