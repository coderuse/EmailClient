using CodeRuse.Email.Client.Configuration;
using CodeRuse.Email.Client.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace CodeRuse.Email.Client.Controllers
{
    [Authorize]
    public class EmailController : ApiController
    {
        private const string ConfigSectionName = "coderuse";

        public EmailController()
        {
        }

        /// <summary>
        /// Send email, by calling this api from clients
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Send(EmailModels email)
        {
            try
            {
                SmtpConfigurationElement currentSmtpConf = null;
                SmtpConfigurationSection configurationSection = (SmtpConfigurationSection)ConfigurationManager.GetSection(ConfigSectionName);
                if (configurationSection != null && configurationSection.Factories != null)
                {
                    foreach (SmtpConfigurationElement factoryConfiguration in configurationSection.Factories)
                    {
                        string currentSmtp = System.Configuration.ConfigurationManager.AppSettings["CurrentSmtp"].ToString();
                        if (factoryConfiguration.Name == currentSmtp)
                        {
                            currentSmtpConf = factoryConfiguration;
                            break;
                        }
                    }
                }
                Chilkat.MailMan mailMan = new Chilkat.MailMan()
                {
                    SmtpHost = currentSmtpConf.SmtpHost,
                    SmtpPort = currentSmtpConf.SmtpPort,
                    SmtpSsl = currentSmtpConf.SmtpSsl,
                    SmtpUsername = currentSmtpConf.SmtpUser,
                    SmtpPassword = currentSmtpConf.SmtpPass
                };

                bool success = mailMan.UnlockComponent("30-day trial");
                if (success != true)
                {
                    throw new Exception("30 day trial is not available...");
                }
                Chilkat.Email msg = new Chilkat.Email()
                {
                    From = email.From,
                    Subject = email.Subject
                };
                var source = WebUtility.HtmlDecode(email.Body);
                HtmlDocument resultant = new HtmlDocument();
                resultant.LoadHtml(source);

                List<Task> tasks = new List<Task>();

                List<HtmlAttribute> imageAttributes = resultant.DocumentNode.Descendants()
                    .Where(x => x.Name == "img").SelectMany(i => i.Attributes)
                    .Where(a => a.Name == "src" && a.Value.IndexOf("data:image/") == 0).ToList();

                foreach (var attr in imageAttributes)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        string fileName = string.Empty;
                        try
                        {
                            fileName = string.Format("img_{0}_{1}.{2}",
                                System.Guid.NewGuid(), DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss"),
                                attr.Value.Substring(attr.Value.IndexOf('/') + 1, attr.Value.IndexOf(';') - attr.Value.IndexOf('/') - 1));

                            // Embed the file into mail body
                            string contentId = msg.AddRelatedData(fileName, Convert.FromBase64String(attr.Value.Substring(attr.Value.IndexOf(',') + 1)));
                            if (msg.LastMethodSuccess != true)
                            {
                                throw new Exception("Couldn't get the content id: " + msg.LastErrorText);
                            }
                            attr.Value = string.Format("cid:{0}", contentId);
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Couldn't write the image file: " + e.Message);
                        }
                    }));
                }
                Task.WaitAll(tasks.ToArray());

                msg.SetHtmlBody(resultant.DocumentNode.InnerHtml);
                if (string.IsNullOrEmpty(email.ToAddresses))
                {
                    throw new Exception("Specify to addresses...");
                }
                foreach (var toAddress in email.ToAddresses.Split(';'))
                {
                    msg.AddTo("", toAddress);
                }
                if (!string.IsNullOrEmpty(email.CcAddresses))
                {
                    foreach (var toAddress in email.CcAddresses.Split(';'))
                    {
                        msg.AddCC("", toAddress);
                    }
                }
                success = mailMan.SendEmail(msg);
                if (!success)
                {
                    throw new Exception("Could not send the mail due to " + mailMan.LastErrorText);
                }

                success = mailMan.CloseSmtpConnection();
                if (!success)
                {
                    throw new Exception("Connection to SMTP server not closed cleanly.");
                }
                else
                {
                    return Ok(new
                    {
                        status = "Success",
                        code = 200,
                        email = email
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
