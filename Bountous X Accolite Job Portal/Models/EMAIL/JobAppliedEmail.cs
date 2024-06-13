namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public class JobAppliedEmail
    {
        public static string EmailStringBody(string name, string jobCode, string jobTitle)
        {
            return $@"<html>
<head>
</head>
<body>
<h2>Thanks for Applying!</h2>


<h4>Dear {name},</h4>
Thanks for taking out time to apply for {jobTitle} [{jobCode}]. We appreciate your interest in bounteous x Accolite.<br>
We're currently in the process of taking applications for this position. If you are selected to proceed for the interview process, our human resource department will be in contact with you soon.<br>

Regards,<br>
Team bounteous x Accolite Job Portal

</body>
</html>




";


        }
    }
}
