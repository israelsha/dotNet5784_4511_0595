namespace BO;
using System.Net.Mail;
internal class Tools
{
    public static string IsValidEmail(string email)
    {
        try
        {
            MailAddress mailAddress = new MailAddress(email);
            return "";
        }
        catch
        {
            return "Email";
        }
    }

}
