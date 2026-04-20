using EmployeeDatabase.Data;
using EmployeeDatabase.Models;
using EmployeeDatabase.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDatabase.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeesController : ControllerBase
	{
		private readonly ApplicationDbContext dbContext;
		/// <summary>
		///	This is  a controller class for endpoints to add data to Employee Database
		public EmployeesController(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		/// <summary>
		/// This endpoint adds employee data to the database.
		/// </summary>
		/// <param name="addEmployeeDto">The employee data to be added.</param>
		/// <returns>The newly created employee.</returns>>
		[HttpPost]
		public IActionResult AddEmployeeDetails(AddEmployeeDto addEmployeeDto)
		{
			var employeeEntity = new EmployeeDetails()
			{

				FirstName = addEmployeeDto.FName,
				LastName = addEmployeeDto.LName,
				Email = addEmployeeDto.Email

			};
			dbContext.EmployeeDetails.Add(employeeEntity);
			dbContext.SaveChanges();
			//return Ok(employeeEntity);
			return StatusCode(201, employeeEntity);
		}

		[HttpGet("all")]
		public IActionResult GetAllEmployeeDetails(){
			return Ok(dbContext.EmployeeDetails.ToList());
		}

		[HttpPut]
		[Route("v2/{id:guid}")]
		public IActionResult UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto)
		{
			var employee = dbContext.EmployeeDetails.Find(id);
			if (employee is null)
			{
				return NotFound();
			}

			// Only update if a value is provided
			if (!string.IsNullOrWhiteSpace(updateEmployeeDto.FName))
				employee.FirstName = updateEmployeeDto.FName;

			if (!string.IsNullOrWhiteSpace(updateEmployeeDto.LName))
				employee.LastName = updateEmployeeDto.LName;

			if (!string.IsNullOrWhiteSpace(updateEmployeeDto.Email))
				employee.Email = updateEmployeeDto.Email;

			dbContext.SaveChanges();

			return Ok(employee);
		}

		[HttpDelete]
		[Route("delete/{id:guid}")]
		public IActionResult DeleteEmployee(Guid id)
		{
			var employee = dbContext.EmployeeDetails.Find(id);

			// 1. If not found, return NotFound immediately
			if (employee is null)
			{
				return NotFound();
			}

			// 2. If found, remove them
			dbContext.EmployeeDetails.Remove(employee);
			dbContext.SaveChanges();

			// 3. Return a value for the "success" path
			return Ok("Employee deleted successfully.");
		}
	}
}
