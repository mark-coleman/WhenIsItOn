using System;
using System.Net;
using System.Net.Mail;
using SendGrid;
using System.Collections.Generic;

namespace Common
{
    public class EmailNotifier : INotifier
    {
        public void SendNotification(string title, string content, IList<string> recipients)
        {
            var myMessage = new SendGridMessage();

            myMessage.From = new MailAddress("mark.coleman@outlook.com", "When Is It On?");
            myMessage.AddTo(recipients);

            myMessage.Subject = title;

            //myMessage.Html = "<p>Hello World!</p>";
            myMessage.Text = content;

            var username = "azure_f7c7043b8d6152b4ce5b7737e9945c3b@azure.com";
            var pswd = "1fDI8rJn18RkK6l";

            var credentials = new NetworkCredential(username, pswd);

            var transportWeb = new Web(credentials);

            // You can also use the **DeliverAsync** method, which returns an awaitable task.
            transportWeb.Deliver(myMessage);
        }
    }
}
