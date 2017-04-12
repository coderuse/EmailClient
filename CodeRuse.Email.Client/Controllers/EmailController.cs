using CodeRuse.Email.Client.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
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
                Chilkat.MailMan mailMan = new Chilkat.MailMan()
                {
                    SmtpHost = "",
                    SmtpPort = 465,
                    SmtpSsl = true,
                    SmtpUsername = "",
                    SmtpPassword = @""
                };
                
                bool success = mailMan.UnlockComponent("30-day trial");
                if (success != true)
                {
                    throw new Exception("30 day trial is not available...");
                }
                Chilkat.Email eml = new Chilkat.Email() {
                    From = email.From,
                    Subject = email.Subject
                };
                var source = WebUtility.HtmlDecode(email.Body);
                HtmlDocument resultant = new HtmlDocument();
                resultant.LoadHtml(source);

                List<HtmlNode> images = resultant.DocumentNode.Descendants().Where(x => x.Name == "img").ToList();

                foreach (var img in images)
                {
                    foreach (var attr in img.Attributes)
                    {
                        if (!string.IsNullOrEmpty(attr.Name) && attr.Name.ToLower() == "src" &&
                            attr.Value.IndexOf("data:") == 0)
                        {
                            File.WriteAllBytes(@"<path>\myCurrentFile.jpg", Convert.FromBase64String(attr.Value.Substring(attr.Value.IndexOf(',') + 1)));
                            string contentId = eml.AddRelatedFile(@"<path>\myCurrentFile.jpg");
                            if (eml.LastMethodSuccess != true)
                            {
                                Console.WriteLine(eml.LastErrorText);
                                throw new Exception("Couldn't get the content id...");
                            }
                            attr.Value = string.Format("cid:{0}", contentId);
                        }
                    }
                }
                
                eml.SetHtmlBody(resultant.DocumentNode.InnerHtml);
                if (string.IsNullOrEmpty(email.ToAddresses)) {
                    throw new Exception("Specify to addresses...");
                }
                foreach (var toAddress in email.ToAddresses.Split(';')) {
                    eml.AddTo("Test", toAddress);
                }
                if (!string.IsNullOrEmpty(email.CcAddresses))
                {
                    foreach (var toAddress in email.CcAddresses.Split(';'))
                    {
                        eml.AddCC("Test", toAddress);
                    }
                }
                success = mailMan.SendEmail(eml);
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
