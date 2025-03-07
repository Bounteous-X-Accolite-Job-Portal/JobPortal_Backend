﻿namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public static class EmailBody
    {
        public static string EmailStringBody(string name , string email, string emailToken)
        {
            return $@"<html>
<head>
</head>
<body>
<h1>Reset Your bounteous x Accolite Job Portal Password</h1>


<h3>Dear {name},</h3>
<div>

It seems like you've forgotten your password for the bounteous x Accolite Job Portal. Not to worry, we're here to help you regain access to your account.<br>

<h5>To reset your password, please follow these simple steps:</h5>
<ol>
<li>Visit the bounteous x Accolite Job Portal forget password page at https://kind-dune-058eee70f.5.azurestaticapps.net/forgot-password?email={email}&code={emailToken}.
<li>Please note that the link to reset your password will expire after a certain period of time for security reasons. If you don't receive the email within a few minutes, please check your spam/junk folder, as it may have been filtered incorrectly.</li>
</ol>
Once you receive the email, follow the instructions provided to set a new password for your account. Make sure to choose a strong and secure password to protect your account information.

If you have any questions or need further assistance, please don't hesitate to contact our support team at support@accolitedigital.com . We're here to assist you with any concerns you may have.

Thank you for choosing bounteous x Accolite. We appreciate your patience and cooperation in resetting your password.



<br>
<br>
</div>
<div>
Regards,<br>
Team<br>
bounteous x Accolite Job Portal

</div>

</body>
</html>




";


        }
    }
}

