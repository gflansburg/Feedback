/*
' Copyright (c) 2021  Gafware
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Entities.Modules;

namespace Gafware.Modules.Feedback
{
    public class FeedbackModuleSettingsBase : ModuleSettingsBase
    {
        private string _smtpserver;
        public string SMTPServerSettings
        {
            get
            {
                _smtpserver = string.Empty;
                if (Settings.Contains("SMTPServer"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["SMTPServer"].ToString()))
                    {
                        _smtpserver = Settings["SMTPServer"].ToString();
                    }
                }
               return _smtpserver;
            }
        }

        private bool _usednnsettings;
        public bool UseDnnSettings
        {
            get
            {
                _usednnsettings = true;

                if (Settings.Contains("DNNSMTPServer"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["DNNSMTPServer"].ToString()))
                    {
                        bool.TryParse(Settings["DNNSMTPServer"].ToString(), out _usednnsettings);
                    }
                }
                return _usednnsettings;
            }
        }

        private string _smtpusername;
        public string SmtpUsername
        {
            get
            {
                _smtpusername = string.Empty;
                if (Settings.Contains("SMTPUsername"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["SMTPUsername"].ToString()))
                    {
                        _smtpusername = Settings["SMTPUsername"].ToString();
                    }
                }
                return _smtpusername;
            }
        }

        private string _smtppassword;
        public string SmtpPassword
        {
            get
            {
                _smtppassword = string.Empty;
                if (Settings.Contains("SMTPPassword"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["SMTPPassword"].ToString()))
                    {
                        _smtppassword = Settings["SMTPPassword"].ToString();
                    }
                }
                
                return _smtppassword;
            }
        }

        private string _smtpdomain;
        public string SmtpDomain
        {
            get
            {
                _smtpdomain = string.Empty;
                if (Settings.Contains("SMTPDomain"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["SMTPDomain"].ToString()))
                    {
                        _smtpdomain = Settings["SMTPDomain"].ToString();
                    }
                }
                
                return _smtpdomain;
            }
        }

        private bool _smtpssl;
        public bool SmtpSSL
        {
            get
            {
                _smtpssl = false;
                if (Settings.Contains("SMTPSSL"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["SMTPSSL"].ToString()))
                    {
                        bool.TryParse(Settings["SMTPSSL"].ToString(), out _smtpssl);
                    }
                }
                
                return _smtpssl;
            }
        }

        private string _smtpportnum;
        public string SmtpPortnum
        {
            get
            {
                _smtpportnum = string.Empty;
                if (Settings.Contains("SMTPPort"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["SMTPPort"].ToString()))
                    {
                        _smtpportnum = Settings["SMTPPort"].ToString();
                    }
                }
                return _smtpportnum;
            }
        }

        private string _emailAddress = "ohr@ou.edu";
        public string EmailAddress
        {
            get
            {
                if (Settings.Contains("EmailAddress") && !string.IsNullOrEmpty(Settings["EmailAddress"].ToString()))
                {
                    _emailAddress = Settings["EmailAddress"].ToString();
                }
                return _emailAddress;
            }
        }

        private string _buttonText = "Send Feedback";
        public string ButtonText
        {
            get
            {
                if (Settings.Contains("ButtonText") && !string.IsNullOrEmpty(Settings["ButtonText"].ToString()))
                {
                    _buttonText = Settings["ButtonText"].ToString();
                }
                return _buttonText;
            }
        }

        private string _alertText = "Thank you for your feedback.";
        public string AlertText
        {
            get
            {
                if (Settings.Contains("AlertText") && !string.IsNullOrEmpty(Settings["AlertText"].ToString()))
                {
                    _alertText = Settings["AlertText"].ToString();
                }
                return _alertText;
            }
        }

        private bool _enablerecaptcha = false;
        public bool EnableGooglereCaptcha
        {
            get
            {
                if (Settings.Contains("EnableGooglereCaptcha"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["EnableGooglereCaptcha"].ToString()))
                    {
                        bool.TryParse(Settings["EnableGooglereCaptcha"].ToString(), out _enablerecaptcha);
                    }
                }
                return _enablerecaptcha;
            }
        }

        private string _recaptchaprivatekey = string.Empty;
        public string GetreCaptchaPrivateKey
        {
            get
            {
                if (Settings.Contains("reCaptchaPrivateKey"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["reCaptchaPrivateKey"].ToString()))
                    {
                        _recaptchaprivatekey = Settings["reCaptchaPrivateKey"].ToString();
                    }
                }
                return _recaptchaprivatekey;
            }
        }
        
        private string _recaptchapublickey = string.Empty;
        public string GetreCaptchaPublicKey
        {
            get
            {
                if (Settings.Contains("reCaptchaPublicKey"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["reCaptchaPublicKey"].ToString()))
                    {
                        _recaptchapublickey = Settings["reCaptchaPublicKey"].ToString();
                    }
                }
                return _recaptchapublickey;
            }
        }

        private string _recaptchatheme = "Light";
        public string GetreCaptchaTheme
        {
            get
            {
                if (Settings.Contains("reCaptchaTheme"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["reCaptchaTheme"].ToString()))
                    {
                        _recaptchatheme = Settings["reCaptchaTheme"].ToString();
                    }
                }
                return _recaptchatheme;
            }
        }

        private string _blankrecaptcha = string.Empty;
        public string BlankRecaptcha
        {
            get
            {
                if (Settings.Contains("reCaptchaThemeblank"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["reCaptchaThemeblank"].ToString()))
                    {
                        _blankrecaptcha = Settings["reCaptchaThemeblank"].ToString();
                    }
                }
                return _blankrecaptcha;
            }
        }

        private string _incorrectcaptcha = string.Empty;
        public string IncorrectCaptcha
        {
            get
            {
                if (Settings.Contains("reCaptchaThemeincorret"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["reCaptchaThemeincorret"].ToString()))
                    {
                        _incorrectcaptcha = Settings["reCaptchaThemeincorret"].ToString();
                    }
                }
                return _incorrectcaptcha;
            }
        }
    }
}