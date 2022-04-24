using ImmediateDelivery.Common;

namespace ImmediateDelivery.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subject, string body);
    }
}
