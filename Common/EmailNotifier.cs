﻿using System;
using System.Net;
using System.Net.Mail;
using SendGrid;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common
{
    public class EmailNotifier : INotifier
    {
        public void SendNotification(string title, string content, IList<string> recipients)
        {
            Tracer.WriteLine("Sending notification via email");

            SendGridMessage message = CreateMessage(title, content, recipients);
            NetworkCredential credentials = CreateAuthenticationToken();
            var transportWeb = new Web(credentials);

            Tracer.WriteLine("Calling DeliverAsync...");

            try
            {
                var result = transportWeb.DeliverAsync(message);
                Tracer.WriteLine("Called DeliverAsync");

                result.Wait();
                Tracer.WriteLine("DeliverAsync completed: {0}",
                    (result.Exception == null ? "{no error}" : result.Exception.ToString()));
            }
            catch (Exception ex)
            {
                Tracer.WriteLine("Call to DeliverAsync failed: {0}", ex.ToString());
            }
        }

        private static NetworkCredential CreateAuthenticationToken()
        {
            var username = "azure_f7c7043b8d6152b4ce5b7737e9945c3b@azure.com";
            var pswd = "1fDI8rJn18RkK6l";

            return new NetworkCredential(username, pswd);
        }

        private static SendGridMessage CreateMessage(string title, string content, IList<string> recipients)
        {
            var message = new SendGridMessage();

            message.From = new MailAddress("mark.coleman@outlook.com", "When Is It On?");
            message.AddTo(recipients);
            message.Subject = title;
            message.Text = content;

            return message;
        }
    }
}
