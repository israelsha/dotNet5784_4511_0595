namespace BO;
using BlImplementation;
using DalApi;
using Dal;
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

    public static DO.Task? findTask(int ingineerId)
    {
        IEnumerable<DO.Task> tasks = DalApi.Factory.Get.Task.ReadAll();//bring the list of all tasks
        return (from item in tasks     //select the task that this (our) engineer is doing (ie: id=EngineerId)
                where (item.EngineerId == ingineerId)
                select item).FirstOrDefault();
    }
}
