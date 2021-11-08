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
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;

namespace Gafware.Modules.Feedback
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Settings class manages Module Settings
    /// 
    /// Typically your settings control would be used to manage settings for your module.
    /// There are two types of settings, ModuleSettings, and TabModuleSettings.
    /// 
    /// ModuleSettings apply to all "copies" of a module on a site, no matter which page the module is on. 
    /// 
    /// TabModuleSettings apply only to the current module on the current page, if you copy that module to
    /// another page the settings are not transferred.
    /// 
    /// If you happen to save both TabModuleSettings and ModuleSettings, TabModuleSettings overrides ModuleSettings.
    /// 
    /// Below we have some examples of how to access these settings but you will need to uncomment to use.
    /// 
    /// Because the control inherits from FeedbackSettingsBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Settings : FeedbackModuleSettingsBase
    {
        #region Base Method Implementations

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// LoadSettings loads the settings from the Database and displays them
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void LoadSettings()
        {
            yesitdid.Visible = false;
            noitdidnt.Visible = false;

            try
            {
                if (Page.IsPostBack == false)
                {
                    //Check for existing settings and use those on this page
                    //Settings["SettingName"]

                    txtTitle.Text = ButtonText;
                    txtThankYou.Text = AlertText;
                    txtSendTo.Text = EmailAddress;
                    ChkEnableRecaptcha.Checked = EnableGooglereCaptcha;
                    txtprivateKey.Text = GetreCaptchaPrivateKey;
                    txtpublickey.Text = GetreCaptchaPublicKey;
                    RecaptchaThemeDDL.Items.FindByValue(GetreCaptchaTheme).Selected = true;
                    txtincorrectcaptcha.Text = IncorrectCaptcha;
                    txtblankcaptcha.Text = BlankRecaptcha;
                    pnlRecaptcha.Visible = ChkEnableRecaptcha.Checked;

                    chkusednn.Checked = UseDnnSettings;
                    txtSmtpServer.Text = SMTPServerSettings;
                    txtsmtpusername.Text = SmtpUsername;
                    txtsmtppassword.Text = PasswordEncrypter.Decrypt(SmtpPassword);
                    txtsmtpdomain.Text = SmtpDomain;
                    chksmtpssl.Checked = SmtpSSL;
                    txtsmtpport.Text = SmtpPortnum;

                    pnlSmtp.Visible = !chkusednn.Checked;
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UpdateSettings()
        {
            try
            {
                var modules = new ModuleController();
                modules.UpdateModuleSetting(ModuleId, "ButtonText", txtTitle.Text);
                modules.UpdateModuleSetting(ModuleId, "AlertText", txtThankYou.Text);
                modules.UpdateModuleSetting(ModuleId, "EmailAddress", txtSendTo.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "EnableGooglereCaptcha", ChkEnableRecaptcha.Checked.ToString());
                modules.UpdateModuleSetting(ModuleId, "reCaptchaPublicKey", txtpublickey.Text);
                modules.UpdateModuleSetting(ModuleId, "reCaptchaPrivateKey", txtprivateKey.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "reCaptchaTheme", RecaptchaThemeDDL.SelectedItem.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "reCaptchaThemeblank", txtblankcaptcha.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "reCaptchaThemeincorret", txtincorrectcaptcha.Text);

                modules.UpdateModuleSetting(ModuleId, "DNNSMTPServer", chkusednn.Checked.ToString());
                modules.UpdateModuleSetting(ModuleId, "SMTPServer", txtSmtpServer.Text);
                modules.UpdateModuleSetting(ModuleId, "SMTPPort", txtsmtpport.Text);
                modules.UpdateModuleSetting(ModuleId, "SMTPUsername", txtsmtpusername.Text);
                modules.UpdateModuleSetting(ModuleId, "SMTPPassword", PasswordEncrypter.Encrypt(txtsmtppassword.Text));
                modules.UpdateModuleSetting(ModuleId, "SMTPDomain", txtsmtpdomain.Text);
                modules.UpdateModuleSetting(ModuleId, "SMTPSSL", chksmtpssl.Checked.ToString());

            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void ChkEnableRecaptcha_CheckedChanged(object sender, EventArgs e)
        {
            pnlRecaptcha.Visible = ChkEnableRecaptcha.Checked;
        }

        #endregion

        protected void chkusednn_CheckedChanged(object sender, EventArgs e)
        {
            pnlSmtp.Visible = !chkusednn.Checked;
        }

        protected void TestSMTPSettings_Click(object sender, EventArgs e)
        {
            if (chkusednn.Checked)
            {
                try
                {
                    DotNetNuke.Services.Mail.Mail.SendEmail(smtpuser.Text, smtpuser.Text, "Test Email", "This is a test email");
                    yesitdid.Visible = true;
                    noitdidnt.Visible = false;
                }
                catch (Exception)
                {
                    noitdidnt.Visible = true;
                    yesitdid.Visible = false;
                }

            }
            else
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.Subject = "Test Email";
                    mail.To.Add(txtSendTo.Text);
                    MailAddress FromA = new MailAddress(txttestemail.Text);
                    mail.From = FromA;
                    mail.Body = "This is a test email";
                    SmtpClient SmtpMail = new SmtpClient(txtSmtpServer.Text, Convert.ToInt32(txtsmtpport.Text));
                    SmtpMail.EnableSsl = chksmtpssl.Checked;
                    if (!String.IsNullOrEmpty(txtsmtpusername.Text) && !String.IsNullOrEmpty(txtsmtppassword.Text))
                    {
                        if (!String.IsNullOrEmpty(txtsmtpdomain.Text))
                        {
                            SmtpMail.Credentials = new System.Net.NetworkCredential(txtsmtpusername.Text, txtsmtppassword.Text, txtsmtpdomain.Text);
                        }
                        else
                        {
                            SmtpMail.Credentials = new System.Net.NetworkCredential(txtsmtpusername.Text, txtsmtppassword.Text);
                        }
                    }
                    SmtpMail.Send(mail);
                    mail.Dispose();
                    yesitdid.Visible = true;
                    noitdidnt.Visible = false;
                }
                catch (Exception)
                {
                    noitdidnt.Visible = true;
                    yesitdid.Visible = false;
                }
            }
        }
    }
}