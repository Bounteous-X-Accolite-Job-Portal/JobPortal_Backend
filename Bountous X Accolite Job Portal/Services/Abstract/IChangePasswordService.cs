﻿using Bountous_X_Accolite_Job_Portal.Models.EMAIL;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IChangePasswordService
    {
        void SendEmail(EmailData request);
    }
}
