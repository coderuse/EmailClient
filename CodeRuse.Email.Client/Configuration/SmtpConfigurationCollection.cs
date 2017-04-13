using System.Configuration;

namespace CodeRuse.Email.Client.Configuration
{
    public class SmtpConfigurationCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets a <see cref="SmtpConfigurationElement"/> at a given index.
        /// </summary>
        public SmtpConfigurationElement this[int index]
        {
            get { return (SmtpConfigurationElement)BaseGet(index); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SmtpConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SmtpConfigurationElement)element).Name;
        }
    }
}