using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeRuse.Email.Client.Models
{
    public class EmailModels
    {
        public string ToAddresses { get; set; }

        public string CcAddresses { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}