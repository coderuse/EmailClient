using System.Configuration;

namespace CodeRuse.Email.Client.Configuration
{
    public class SmtpConfigurationElement : ConfigurationElement
    {
        private const string NameAttribute = "name";

        private const string SmtpHostAttribute = "host";

        private const string SmtpPortAttribute = "port";

        private const string SmtpSslAttribute = "ssl";

        private const string SmtpUserAttribute = "user";

        private const string SmtpPassAttribute = "password";

        /// <summary>
        /// Indicates the name of the configuration
        /// </summary>
        [ConfigurationProperty(NameAttribute, IsRequired = false)]
        public string Name
        {
            get
            {
                return (string)this[NameAttribute];
            }
            set
            {
                this[NameAttribute] = value;
            }
        }

        /// <summary>
        /// Indicates the FQDN of the smtp server
        /// </summary>
        [ConfigurationProperty(SmtpHostAttribute, IsRequired = true)]
        public string SmtpHost
        {
            get
            {
                return (string)this[SmtpHostAttribute];
            }
            set
            {
                this[SmtpHostAttribute] = value;
            }
        }

        /// <summary>
        /// Indicates the port for the smtp server to connect
        /// </summary>
        [ConfigurationProperty(SmtpPortAttribute, IsRequired = false, DefaultValue = 465)]
        public int SmtpPort
        {
            get
            {
                return (int)this[SmtpPortAttribute];
            }
            set
            {
                this[SmtpPortAttribute] = value;
            }
        }

        /// <summary>
        /// Indicates if ssl is required for the smtp server
        /// </summary>
        [ConfigurationProperty(SmtpSslAttribute, IsRequired = false, DefaultValue = true)]
        public bool SmtpSsl
        {
            get
            {
                return (bool)this[SmtpSslAttribute];
            }
            set
            {
                this[SmtpSslAttribute] = value;
            }
        }

        /// <summary>
        /// Indicates if ssl is required for the smtp server
        /// </summary>
        [ConfigurationProperty(SmtpUserAttribute, IsRequired = true)]
        public string SmtpUser
        {
            get
            {
                return (string)this[SmtpUserAttribute];
            }
            set
            {
                this[SmtpUserAttribute] = value;
            }
        }

        /// <summary>
        /// Indicates if ssl is required for the smtp server
        /// </summary>
        [ConfigurationProperty(SmtpPassAttribute, IsRequired = true)]
        public string SmtpPass
        {
            get
            {
                return (string)this[SmtpPassAttribute];
            }
            set
            {
                this[SmtpPassAttribute] = value;
            }
        }
    }
}