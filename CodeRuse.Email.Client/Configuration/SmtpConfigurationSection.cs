using System.Configuration;

namespace CodeRuse.Email.Client.Configuration
{
    public class SmtpConfigurationSection : ConfigurationSection
    {
        private const string SmtpCollectionName = "smtp.servers";

        private const string SmtpElementName = "server";

        /// <summary>
        /// The factories specified within the application configuration.
        /// </summary>
        [ConfigurationProperty(SmtpCollectionName, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(SmtpConfigurationCollection), AddItemName = SmtpElementName)]
        public SmtpConfigurationCollection Factories
        {
            get { return (SmtpConfigurationCollection)this[SmtpCollectionName]; }
            set { this[SmtpCollectionName] = value; }
        }
    }
}