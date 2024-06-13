namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public static class InterviewSendEmail
    {
        public static string EmailStringBody(string name, string link, DateOnly? InterviewDate, TimeOnly? InterviewTime)
        {
            return $@"<html>
<head>
</head>
<body>
<h1>Interview Scheduled</h1>


<h5>Dear {name},</h5>
<p>We are pleased to schedule an interview for you. Please accept the meeting invite.</p>
<p>Your Interview has been Scheduled :- </p>
<p>Interview Date : {InterviewDate}</p>
<p>Interview Time : {InterviewTime}</p>
<p>Interview Link : {link}</p>

Regards,<br>
Team bounteous x Accolite Job Portal

</body>
</html>




";


        }
    }
}
