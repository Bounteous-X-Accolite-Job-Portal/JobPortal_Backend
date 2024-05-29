namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public static class InterviewSendEmail
    {
        public static string EmailStringBody(string email,string link , DateOnly? InterviewDate , TimeOnly InterviewTime)
        {
            return $@"<html>
<head>
</head>
<body>
<h1>Interview Scheduled</h1>


<h5>Dear Candidate,</h5>

<p>Your Interview has been Scheduled :- </p>
<p>Interview Date : {InterviewDate}</p>
<p>Interview Time : {InterviewTime}</p>
<p>Interview Link : {link}</p>

Regards,<br>
Team<br>
bounteuous x Accolite Job Portal

""></a>
</body>
</html>




";


        }
    }
}
