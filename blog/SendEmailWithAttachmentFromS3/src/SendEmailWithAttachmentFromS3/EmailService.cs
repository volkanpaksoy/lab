using Amazon;
using System;
using System.Collections.Generic;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using MimeKit;
using System.IO;

namespace SendEmailWithAttachmentFromS3
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string attachmentPath);
    }

    public class RawEmailService : IEmailService
    {
        private static readonly string subject = "RE: CV Request - Volkan Paksoy";
        private static readonly string htmlBody = "<p>Hello,</p><p>Please find attached my latest CV. Thanks for your interest.</p><p>Kind regards,</p><p>Volkan</p>";
        private static readonly string textBody = "Hello,\r\nPlease find attached my latest CV.\r\n\r\nKind regards,\r\nVolkan";

        private readonly ILambdaContext context;

        public RawEmailService(ILambdaContext context)
        {
            this.context = context;
        }

        public async Task SendEmailAsync(string to, string attachmentPath)
        {
            var fromAddress = System.Environment.GetEnvironmentVariable("FROM_ADDRESS");
            if (string.IsNullOrEmpty(fromAddress)) throw new Exception("Invalid configuration: FROM_ADDRESS environment variable is missing");

            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUWest1))
            {
                using (var messageStream = new MemoryStream())
                {
                    var message = new MimeMessage();
                    var builder = new BodyBuilder() 
                    { 
                        TextBody = string.Format(textBody, to),
                        HtmlBody = string.Format(htmlBody, to)
                    };

                    message.From.Add(new MailboxAddress(fromAddress));
                    message.To.Add(new MailboxAddress(to));
                    message.Subject = subject;

                    using (FileStream stream = File.Open(attachmentPath, FileMode.Open)) 
                    {
                        builder.Attachments.Add(attachmentPath, stream);
                    }
                        
                    message.Body = builder.ToMessageBody();
                    message.WriteTo(messageStream);

                    var request = new SendRawEmailRequest()
                    {
                        RawMessage = new RawMessage() { Data = messageStream }
                    };

                    await client.SendRawEmailAsync(request);
                }   
            }


        }
    }

    public class TemplatedEmailService : IEmailService
    {
        private static readonly string senderAddress = "cv@vlkn.me";
        private readonly ILambdaContext context;

        public TemplatedEmailService(ILambdaContext context)
        {
            this.context = context;
        }

        public async Task SendEmailAsync(string to, string attachmentPath)
        {
            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUWest1))
            {
                var request = new SendTemplatedEmailRequest
                {
                    Source = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { to }
                    },
                    Template = "cv_response_email_template",
                    TemplateData = JsonConvert.SerializeObject(new { sourceEmail = to })
                };

                try
                {
                    context.Logger.LogLine("Sending email using Amazon SES...");
                    var response = await client.SendTemplatedEmailAsync(request);
                    context.Logger.LogLine("The email was sent successfully.");
                }
                catch (Exception ex)
                {
                    context.Logger.LogLine($"Error while sending email: {ex.Message}");
                }
            }
        }
    }
}