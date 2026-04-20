using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace EmployeeDatabase.Models
{

	public class AddEmployeeDto
	{

			public required String FName { get; set; }
			public required String LName { get; set; }
			public String? Email { get; set; }

		}


}
