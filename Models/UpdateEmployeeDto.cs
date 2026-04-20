namespace EmployeeDatabase.Models
{
	public class UpdateEmployeeDto
	{
		public required String FName { get; set; }
		public required String LName { get; set; }
		public String? Email { get; set; }
	}
}
