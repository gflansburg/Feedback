using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace Gafware.Recaptcha.Web.UI.Controls
{
    public class GoogleVerificationResponseOutput
    {
        public bool success { get; set; }
    }

    //[DefaultProperty("Text")]
    [ToolboxData("<{0}:GoogleReCaptcha runat=server></{0}:GoogleReCaptcha>")]
    public class GoogleReCaptcha : WebControl
    {
        string ValidateResponseField = "g-recaptcha-response";
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string PublicKey
        {
            get
            {
                String publicKey = (String)ViewState["_PublicKey"];
                return publicKey;
            }

            set
            {
                ViewState["_PublicKey"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string PrivateKey
        {
            get
            {
                String privateKey = (String)ViewState["_PrivateKey"];
                return privateKey;
            }

            set
            {
                ViewState["_PrivateKey"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(RecaptchaTheme.Light)]
        [Localizable(true)]
        public RecaptchaTheme Theme
        {
            get
            {
                object t = ViewState["RecaptchaTheme"];
                return ((t == null) ? RecaptchaTheme.Light : (RecaptchaTheme)t);
            }

            set
            {
                ViewState["RecaptchaTheme"] = value;
            }
        }


        protected override void RenderContents(HtmlTextWriter output)
        {
            if (!string.IsNullOrEmpty(PublicKey) && !string.IsNullOrEmpty(PrivateKey))
            {
                string reCaptchaHTML = string.Format("<div id='{2}_Ctrl'></div>", PublicKey, Theme.ToString(), this.ClientID);

                //string scriptToRenderCaptcha = @" <script src=""https://www.google.com/recaptcha/api.js?onload=onLoadreCaptcha&render=explicit"" async defer></script>";
                output.Write(reCaptchaHTML);
                //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "reCapctchaScript", scriptToRenderCaptcha, false);
            }
            else
            {
                //Public/Private Keys not provided
                string noKeyFound = "<div class='noKeyFound'>Public/Private keys not provided for Captcha control. You can get your keys from <a href='https://www.google.com/recaptcha' target='_blank'>Google Recaptcha</a></div>";
                output.Write(noKeyFound);
            }
        }

        /// <summary>
        /// Gets the user's response to the recaptcha challenge of the recaptcha verification request.
        /// </summary>
        public string Response
        {
            get;
            private set;
        }

        public bool Validate()
        {
            this.Response = this.Page.Request.Form[ValidateResponseField];
            
            string EncodedResponse = this.Response;

            if (string.IsNullOrEmpty(EncodedResponse) || string.IsNullOrEmpty(PrivateKey))
                return false;
            //by pass certificate validation
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var client = new System.Net.WebClient();

            var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            GoogleVerificationResponseOutput gOutput = serializer.Deserialize<GoogleVerificationResponseOutput>(GoogleReply);

            return gOutput.success;
        }
    }
}
