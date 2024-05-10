namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public static class ConfirmEmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>
<head>
</head>
<body>
<h1>CONFIRM YOUR EMAIL</h1>


<h5>Dear User,</h5>

CONFIRM YOUR EMAIL
< a href = ""http://localhost:4200/reset?email={email}&code={emailToken}"">KRLE CONFIRM</a>
</body>
</html>




";
        }
    }
}
