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
<h1>Confirm Your Email Address</h1>


<h3>Dear User</h3>
Welcome to bounteous x Accolite Job Portal!
Thank you for registering with us. Before you can access your account, we need to confirm your email address.

<h4>To confirm your email address and activate your account, please click on the following link:</h4>
<br>
<br>
http://localhost:4200/Login?email={email}&code={emailToken}
<br>
<br>
Thank you for choosing bounteous x Accolite Job Portal. If you have any questions or need assistance, please contact our support team.

<div>
Regards,<br>
bounteuous x Accolite Job Portal

</div>
</body>
</html>




";
        }
    }
}
