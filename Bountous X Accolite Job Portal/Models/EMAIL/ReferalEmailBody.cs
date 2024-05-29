namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public static class ReferalEmailBody
    {
        public static string EmailStringBody(string name ,string email, string referalToken,string AutoPassword)
        {
            return $@"<html>
<head>
</head>
<body>
<h1>You've Been Referred to bounteous x Accolite Job Portal!</h1>


<h5>Dear {name},</h5>

We hope this email finds you well. We are excited to inform you that you've been referred to bounteous x Accolite  by one of our esteemed employees.
<br>

This referral is a testament to your skills and qualifications, and we are thrilled to have the opportunity to consider you for potential opportunities within our organization.

<h5>To take advantage of this referral and explore job opportunities with us, please follow these simple steps:</h5>
<ol>
<li>Visit the bounteous x Accolite Job Portal login page at https://kind-dune-058eee70f.5.azurestaticapps.net/login .</li>
<li>Login to your account using the password {AutoPassword}</li>
<li>Once logged in, navigate to the ""Job Listings"" section to view our current job openings.</li>
<li>Browse through the available positions and select the ones that align with your skills, experience, and career goals.</li>
<li>Complete the application process for the positions you're interested in by submitting your resume and any other required documents.</li>
</ol>
<br>
<br>
We encourage you to explore our job listings thoroughly and apply to the roles that best match your qualifications. Your application will receive special attention as a referred candidate, and we look forward to considering you for suitable opportunities within our organization.
<br>
If you have any questions or need assistance with the application process, please don't hesitate to reach out to our support team at support@accolitedigital.com. We're here to help you every step of the way.
<br>
Thank you for considering a career with bounteous x Accolite. We appreciate your interest and look forward to the possibility of welcoming you to our team.
<br>
<br>
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
