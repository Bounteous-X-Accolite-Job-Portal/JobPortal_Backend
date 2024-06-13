namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public class InterviewEmployeeEmail
    {
        public static string EmailStringBody(string name,string candidateName , string link, DateOnly? InterviewDate, TimeOnly? InterviewTime)
        {
            return $@"<html>
<head>
</head>
<body>
<h1>Interview Scheduled</h1>


<h5>Dear Employer {name},</h5>

<p>Interview has been Scheduled with Candidate {candidateName}:- </p>
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
