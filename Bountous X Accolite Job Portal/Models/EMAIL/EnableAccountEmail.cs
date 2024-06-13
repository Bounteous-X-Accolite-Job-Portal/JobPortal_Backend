namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public class EnableAccountEmail
    {
        public static string EmailStringBody(string Empname)
        {
            return $@"<html>
<head>
</head>
<body>
<h2>Account Enabled!</h2>


<h4>Dear {Empname},</h4>
Your Account has been Enabled by the Administrator.<br>
In case of any queries please contact to support@accolitedigital.com <br>
<br>
<br>
<b>Regards,</b><br>
Team bounteous x Accolite Job Portal

</body>
</html>";
        }
    }
}
