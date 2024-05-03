using Bountous_X_Accolite_Job_Portal.Models;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>
<head>
</head>
<body>
<h1>Reset your Password</h1>


<h5>Dear User,</h5>

Your request to reset forgotten password has been completed successfully.
< a href = ""http://localhost:4200/reset?email={email}&code={emailToken}"">Reset Password</a>
</body>
</html>




";


        }
    }
}

