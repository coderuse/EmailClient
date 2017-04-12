using CodeRuse.Email.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
