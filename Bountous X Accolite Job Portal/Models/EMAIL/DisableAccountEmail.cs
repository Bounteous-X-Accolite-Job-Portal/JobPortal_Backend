namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public class DisableAccountEmail
    {
        public static string EmailStringBody(string Empname)
        {
            return $@"<html>
<head>
</head>
<body>
<h2>Account Disabled!</h2>


<h4>Dear {Empname},</h4>
Your Account has been Disabled by the Administrator.<br>
In case of any queries please contact to support@accolitedigital.com <br>

Regards,<br>
Team bounteuous x Accolite Job Portal

</body>
</html>";
        }
    }
}
