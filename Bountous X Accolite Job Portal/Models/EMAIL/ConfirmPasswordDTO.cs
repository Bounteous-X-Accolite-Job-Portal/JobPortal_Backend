﻿namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public record ConfirmPasswordDTO
    {
        public string Email { get; set; }
        public string EmailToken { get; set; }
    }
}
