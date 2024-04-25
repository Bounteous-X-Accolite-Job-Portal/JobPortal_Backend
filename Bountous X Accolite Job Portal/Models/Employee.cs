using System;
using System.ComponentModel.DataAnnotations;
using Bountous_X_Accolite_Job_Portal.Models;
using Microsoft.AspNetCore.Identity;

public class Emplyoee: IdentityUser
{
	[Key]
	public string EmpID {  get; set; }

	public string FName { get; set; }

	public string LName {  get; set; }

	public string Email {  get; set; }

	public string Password { get; set; }

	public string PhoneNo { get; set; }
	public int DesignationId {  get; set; }
	public Designation Designation { get; set; }

}
