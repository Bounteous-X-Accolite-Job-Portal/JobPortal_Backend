namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public static class EmplyoeeRegisterdMail
    {
        public static string EmailStringBody()
        {

            return $@"<html>
<head>
</head>
<body>
<h1>Welcome to bounteous x Accolite Job Portal!</h1>


<h3>Dear Emplyoee,</h3>
<div>
Welcome aboard to bounteous x Accolite Job Portal! We're thrilled to have you join our team and embark on this exciting journey with us.<br>
As a registered employee on our job portal, you now have access to a range of features designed to streamline your hiring process and enhance your experience.
<br><br>
<h4>Visit our Job Portal:- http://localhost:4200/login </h4>
<br>

To get started, you are requested to reset your password by following simple steps.
<br>
<br>
<h4>Instructions to Reset Your Password:</h4> 

Follow these simple steps:
<ol>
<li>Visit the login page of our job portal.</li>
<li>Click on the ""Forgot Password"" link below the login form.</li>
<li>Enter your registered email address.</li>
<li>You'll receive an email with further instructions on resetting your password.</li>
<li>Follow the provided link and instructions to set a new password for your account.</li>
<br>
</ol>
If you have any questions or need assistance, don't hesitate to reach out to our support team. We're here to help you make the most of your experience on our job portal.
<br>
<br>
Once again, welcome to bounteous x Accolite] Job Portal! We look forward to your contributions and wish you success in your hiring endeavors.


<br>
<br>
</div>
<div>
Regards,<br>
Team<br>
bounteuous x Accolite Job Portal

</div>

</body>
</html>
        ";
        }
    }
}





  