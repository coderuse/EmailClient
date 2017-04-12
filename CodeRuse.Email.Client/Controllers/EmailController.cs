using CodeRuse.Email.Client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
                Chilkat.MailMan mailMan = new Chilkat.MailMan() {
                    SmtpHost = "smtp.google.com",
                    SmtpPort = 465,
                    SmtpSsl = true,
                    SmtpUsername = "",
                    SmtpPassword = "",
                    SmtpAuthMethod = "LOGIN"
                };
                Chilkat.Email eml = new Chilkat.Email() {
                    Body = email.Body,
                    FromAddress = email.CcAddresses,
                    
                };
                return Ok(new
                {
                    status = "Success",
                    code = 200,
                    email = email
                });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
