﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Designation :IdentityUser
    {
        [Key]
        public int DesignationId {  get; set; }

        public string DesignationName { get; set; }

        public DateTime CreatedAt { get; set; }

    //  [ForeignKey(nameof(DesignationId))]
        public int EmpId {  get; set; }

        public Emplyoee Emplyoee { get; set; }


    }
}
