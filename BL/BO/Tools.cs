namespace BO;
using System.Net.Mail;
internal class Tools
{
    public static string isValidMail(string mail)
    {
        try
        {
            MailAddress email = new MailAddress(mail);
            return "";
        }
        catch
        {
            return "Email";
        }
    }
}
