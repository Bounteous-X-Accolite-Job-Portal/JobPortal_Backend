﻿namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public static class ChangePasswordBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>
<head>
</head>
<body>
<h1>Change Your Password</h1>


<h5>Dear User,</h5>

Your request to change your password has been completed successfully.
< a href = https://kind-dune-058eee70f.5.azurestaticapps.net/forgot-password?email={email}&code={emailToken}> Reset Password</a>
</body>
</html>




";


        }
    }
}

