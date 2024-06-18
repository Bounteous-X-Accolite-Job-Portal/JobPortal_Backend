namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public static class NewRegisterEmailBody
    {
        public static string EmailStringBody(string name ,string email, string emailToken)
        {
            return $@"<html>
<head>
</head>
<body>
<h2>Welcome to bounteous x Accolite Job Portal!</h2>


<h3>Dear {name}</h3>
Welcome to bounteous x Accolite Job Portal!<br>
Thank you for registering with us<br>
We are excited to help you find your dream job. Please log in to your account to complete your profile and start applying for job opportunities today. 
Login to the Portal : <a href=https://kind-dune-058eee70f.5.azurestaticapps.net/login><b>Click Here to Login</b></a>
<br>
<br>
<br>
<br>
Thank you for choosing bounteous x Accolite Job Portal.<br>
If you have any questions or need assistance, please contact our support team at support@accolitedigital.com.
<div>
Regards,<br>
bounteous x Accolite Job Portal

</div>
</body>
</html>




";
        }
    }
}
