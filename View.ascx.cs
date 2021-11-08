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

using System;
using System.Net.Mail;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using Gafware.Modules.Feedback.Recaptcha.Web;
using DotNetNuke.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Gafware.Modules.Feedback
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from FeedbackModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : FeedbackModuleBase, IActionable
    {
        public const string CSS_TAG_INCLUDE_FORMAT = "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />";
        public const string SCRIPT_TAG_INCLUDE_FORMAT = "<script language=\"javascript\" type=\"text/javascript\" src=\"{0}\"></script>";

        private const string c_jqBlockUIKey = "jquery.plugin.blockui";
        private const string c_jqOutsideKey = "jquery.plugin.outside";
        private const string c_jRecaptchaAjax = "google.recaptcha.ajax";
        private string _alertMessage = String.Empty;
        private string _alertTitle = String.Empty;

        private readonly INavigationManager _navigationManager;

        public View()
        {
            _navigationManager = DependencyProvider.GetRequiredService<INavigationManager>();
        }

        private void ImportPlugins()
        {
            //load the plugin client scripts on every page load
            if (!Page.ClientScript.IsClientScriptBlockRegistered(c_jqBlockUIKey))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), c_jqBlockUIKey, String.Format(SCRIPT_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "Scripts/jquery.blockUI.js")), false);
            }
            if (!Page.ClientScript.IsClientScriptBlockRegistered(c_jqOutsideKey))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), c_jqOutsideKey, String.Format(SCRIPT_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "Scripts/jquery.outside.js")), false);
            }
            if (!Page.ClientScript.IsClientScriptBlockRegistered(c_jRecaptchaAjax) && EnableGooglereCaptcha)
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), c_jRecaptchaAjax, String.Format(SCRIPT_TAG_INCLUDE_FORMAT, "https://www.google.com/recaptcha/api/js/recaptcha_ajax.js"), false);
                string scriptToRenderCaptcha = @"<script src=""https://www.google.com/recaptcha/api.js?onload=onLoadFeedbackreCaptcha&render=explicit"" async defer></script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), c_jRecaptchaAjax, scriptToRenderCaptcha , false);
            }
            //Page.Header.Controls.Add(new System.Web.UI.LiteralControl(String.Format(CSS_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "Scripts/jquery.qtip.css"))));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ImportPlugins();
                if (!IsPostBack)
                {

                    rfvRecaptcha.EnableClientScript = EnableGooglereCaptcha;
                    cvRecaptcha.Enabled = EnableGooglereCaptcha;
                    rfvRecaptcha.ErrorMessage = BlankRecaptcha;

                    DotNetNuke.Entities.Users.UserInfo currentUser = DotNetNuke.Entities.Users.UserController.Instance.GetCurrentUserInfo();
                    if (currentUser.UserID > -1)
                    {
                        txtEmail.Text = currentUser.Email;
                        txtName.Text = currentUser.DisplayName;
                    }
                    lblTitle.Text = PortalSettings.ActiveTab.TabName;
                }
                if (EnableGooglereCaptcha)
                {
                    Recaptcha1.Visible = true;
                    Recaptcha1.Enabled = true;
                    Recaptcha1.PrivateKey = GetreCaptchaPrivateKey;
                    Recaptcha1.PublicKey = GetreCaptchaPublicKey;
                    switch (GetreCaptchaTheme)
                    {
                        case "Dark":
                            Recaptcha1.Theme = RecaptchaTheme.Dark;
                            break;
                        case "Light":
                            Recaptcha1.Theme = RecaptchaTheme.Light;
                            break;
                        default:
                            Recaptcha1.Theme = RecaptchaTheme.Light;
                            break;
                    }
                }
                else
                {
                    Recaptcha1.Enabled = false;
                    Recaptcha1.Visible = false;
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                return new ModuleActionCollection();
            }
        }

        public void VerifyFields()
        {
            if (EnableGooglereCaptcha)
            {
                cvRecaptcha.IsValid = true;
                if (!Recaptcha1.Validate())
                {
                    if (!string.IsNullOrWhiteSpace(Recaptcha1.Response))
                    {
                        cvRecaptcha.ErrorMessage = IncorrectCaptcha;
                        cvRecaptcha.IsValid = false;
                    }
                    else
                    {
                        cvRecaptcha.ErrorMessage = BlankRecaptcha;
                        cvRecaptcha.IsValid = false;
                    }
                }
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                VerifyFields();
                this.IsValid.Value = Page.IsValid.ToString().ToLower();
                if (Page.IsValid)
                {
                    string body = String.Format("{0}<br /><br />Name: {1}<br />Email: {2}<br />Page: {3}<br />URL: {4}", txtBody.Text, txtName.Text, txtEmail.Text, PortalSettings.ActiveTab.TabName, _navigationManager.NavigateURL(TabId));
                    string error = String.Empty;
                    if (!UseDnnSettings)
                    {
                        if (SendEmail(txtEmail.Text, "HR Website Feedback", body, ref error))
                        {
                            AlertMessage = AlertText;
                            AlertTitle = "Feedback Received";
                            txtBody.Text = String.Empty;
                        }
                        else
                        {
                            AlertMessage = error;
                            AlertTitle = "Error Sending Feedback";
                        }
                    }
                    else
                    {
                        if (SendEmailUsingDnn(txtEmail.Text, "HR Website Feedback", body, ref error))
                        {
                            AlertMessage = AlertText;
                            AlertTitle = "Feedback Received";
                            txtBody.Text = String.Empty;
                        }
                        else
                        {
                            AlertMessage = error;
                            AlertTitle = "Error Sending Feedback";
                        }
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public bool SendEmail(string senderAddress, string subject, string body, ref string error)
        {
            try
            {
                MailMessage mail = new MailMessage(senderAddress, EmailAddress) { Sender = new MailAddress(EmailAddress) };
                mail.IsBodyHtml = true;
                mail.Subject = subject;
                mail.Body = body;
                SmtpClient smtpMail = new SmtpClient(SMTPServerSettings);
                if (!String.IsNullOrEmpty(SmtpUsername) && !String.IsNullOrEmpty(SmtpPassword))
                {
                    if (!String.IsNullOrEmpty(SmtpDomain))
                    {
                        smtpMail.Credentials = new System.Net.NetworkCredential(SmtpUsername, SmtpPassword, SmtpDomain);
                    }
                    else
                    {
                        smtpMail.Credentials = new System.Net.NetworkCredential(SmtpUsername, SmtpPassword);
                    }
                }
                smtpMail.Port = Convert.ToInt32(SmtpPortnum);
                smtpMail.EnableSsl = SmtpSSL;
                smtpMail.Send(mail);
                mail.Dispose();
                return true;
            }
            catch(Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool SendEmailUsingDnn(string senderAddress, string subject, string body, ref string error)
        {
            try
            {
                DotNetNuke.Services.Mail.Mail.SendEmail(senderAddress, EmailAddress, EmailAddress, subject, body);
                return true;
            }
            catch(Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public string AlertMessage
        {
            get { return AlertText; /* _alertMessage; */ }
            set { _alertMessage = value; }
        }

        public string AlertTitle
        {
            get { return "Feedback Received"; /* _alertTitle; */ }
            set { _alertTitle = value; }
        }
    }
}