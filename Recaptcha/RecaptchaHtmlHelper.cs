using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Gafware.Modules.Feedback.Recaptcha.Web
{
    /// <summary>
    /// Represents the functionality to generate recaptcha HTML.
    /// </summary>
    public class RecaptchaHtmlHelper
    {
        /// <summary>
        /// Creates an instance of the <see cref="RecaptchaHtmlHelper"/> class.
        /// </summary>
        /// <param name="publicKey">Sets the public key to be part of the recaptcha HTML.</param>
        public RecaptchaHtmlHelper(string publicKey)
        {
            if (String.IsNullOrEmpty(publicKey))
            {
                throw new InvalidOperationException("Public key cannot be null or empty.");
            }

            this.PublicKey = RecaptchaKeyHelper.ParseKey(publicKey);
        }

        /// <summary>
        /// Creates an instance of the <see cref="RecaptchaHtmlHelper"/> class.
        /// </summary>
        /// <param name="publicKey">Sets the public key of the recaptcha HTML.</param>
        /// <param name="theme">Sets the theme of the recaptcha HTML.</param>
        /// <param name="language">Sets the language of the recaptcha HTML.</param>
        /// <param name="tabIndex">Sets the tab index of the recaptcha HTML.</param>        
        public RecaptchaHtmlHelper(string publicKey, RecaptchaTheme theme, string language, int tabIndex)
        {
            this.PublicKey = RecaptchaKeyHelper.ParseKey(publicKey);

            if (String.IsNullOrEmpty(this.PublicKey))
            {
                throw new InvalidOperationException("Public key cannot be null or empty.");
            }

            this.Theme = theme;
            this.Language = language;
            this.TabIndex = tabIndex;
        }

        /// <summary>
        /// Gets the public key of the recaptcha HTML.
        /// </summary>
        public string PublicKey
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the theme of the recaptcha HTML.
        /// </summary>
        public RecaptchaTheme Theme
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the language of the recaptcha HTML.
        /// </summary>
        public string Language
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tab index of the recaptcha HTML.
        /// </summary>
        public int TabIndex
        {
            get;
            set;
        }      

        /// <summary>
        /// Gets the recaptcha's HTML that needs to be rendered in an HTML page.
        /// </summary>
        /// <returns>Returns the HTML as an instance of the <see cref="String"/> type.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<script type=\"text/javascript\">\nvar RecaptchaOptions = {");

            string language = this.Language;

            if (String.IsNullOrEmpty(language))
            {
                language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            }

            sb.Append(String.Format("\ntheme : '{0}',\nlang : '{1}',\ntabindex : {2}\n", Theme.ToString().ToLower(), language, TabIndex));
            sb.Append("};\n</script>");

            bool isHttps = HttpContext.Current.Request.Url.AbsoluteUri.ToString().StartsWith("https");            

            if (!isHttps)
            {
                sb.Append(String.Format("<script type=\"text/javascript\"src=\"http://www.google.com/recaptcha/api/challenge?k={0}\">", PublicKey));
            }
            else
            {
                sb.Append(String.Format("<script type=\"text/javascript\"src=\"https://www.google.com/recaptcha/api/challenge?k={0}\">", PublicKey));
            }

            sb.Append("</script>");

            return sb.ToString();
        }
    }
}
