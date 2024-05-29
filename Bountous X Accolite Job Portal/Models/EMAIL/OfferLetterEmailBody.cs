namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public static class OfferLetterEmailBody
    {
        public static string EmailStringBody(string name)
        {

            return $@"<html>
<head>
</head>
<body>
<h1>Congratulations! Offer of Employment with bounteous x Accolite</h1>


<h3>Dear {name},</h3>

I hope this email finds you well. I am pleased to inform you that after careful consideration, we have selected you.<br>
<br>
Your qualifications, experience, and enthusiasm for the role stood out among the many candidates we interviewed. 
We are confident that you will make a valuable contribution to our team.<br><br>

Please find attached the formal offer letter outlining the terms and conditions of your employment. 
Kindly review the offer carefully, including details such as your start date, salary, benefits, and other relevant information.

If you have any questions or require further clarification about the offer, please don't hesitate to reach out to me or Talent Acquistion Team at humanresource@accolitedigital.com or 1234567892.<br><br>

We are excited about the opportunity to welcome you aboard and look forward to your positive response. Once again, congratulations on your selection, and we are eager to have you join our team.
<br><br>
Warm regards,
<br>
Talent Acquistion Team<br>
bonteous x Accolite



</a>
</body>
</html>
        ";
        }
    }
}
