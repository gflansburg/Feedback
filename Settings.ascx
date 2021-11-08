<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="Gafware.Modules.Feedback.Settings" %>


<!-- uncomment the code below to start using the DNN Form pattern to create and update settings -->
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>

<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:Label ID="lblTitle" runat="server" ResourceKey="lblTitle" ControlName="txtTitle" Suffix=":" /> 
        <asp:TextBox ID="txtTitle" runat="server" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="FeedBackSettings" />
        <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="FeedBackSettings" ErrorMessage="Please enter the button text." />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblThankYou" runat="server" ResourceKey="lblThankYou" ControlName="txtThankYou" Suffix=":" />
        <asp:TextBox ID="txtThankYou" runat="server" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="FeedBackSettings" />
        <asp:RequiredFieldValidator ID="rfvThankYou" runat="server" ControlToValidate="txtThankYou" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="FeedBackSettings" ErrorMessage="Please enter the confirmation dialog text." />
    </div>
</fieldset>
<h2 id="H1" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded">Mail Settings</a></h2>
    <div class="dnnFormItem">
        <dnn:Label ID="lblSendTo" runat="server" ResourceKey="lblSendTo" ControlName="txtSendTo" Suffix=":" /> 
        <asp:TextBox ID="txtSendTo" runat="server" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="FeedBackSettings" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSendTo" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="FeedBackSettings" ErrorMessage="Please enter the email address to send the feedback to." />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" FontID="regEmailAddress" ValidationGroup="FeedBackSettings" runat="server" CssClass="dnnFormMessage dnnFormError" ErrorMessage="Invalid Email address" ControlToValidate="txtSendTo" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="usednnsmtp" runat="server" ControlName="chkusednn" Suffix=":" />
        <asp:CheckBox runat="server" ID="chkusednn" Checked="true" AutoPostBack="true" OnCheckedChanged="chkusednn_CheckedChanged" />
    </div>
    <asp:Panel ID="pnlSmtp" runat="server" Visible="false">
        <div class="dnnFormItem">
            <dnn:Label ID="SMTPServer" runat="server" ControlName="txtSmtpServer" Suffix=":" />
            <asp:TextBox ID="txtSmtpServer" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="smtpport" runat="server" ControlName="txtsmtpport" Suffix=":" />
            <asp:TextBox ID="txtsmtpport" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="smtpssl" runat="server" ControlName="chksmtpssl" Suffix=":" />
            <asp:CheckBox ID="chksmtpssl" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="smtpuser" runat="server" ControlName="txtsmtpusername" Suffix=":" />
            <asp:TextBox ID="txtsmtpusername" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="smtppassword" runat="server" ControlName="txtsmtppassword" Suffix=":" />
            <asp:TextBox ID="txtsmtppassword" runat="server" TextMode="Password" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="smtpdomain" runat="server" ControlName="txtsmtpdomain" Suffix=":" />
            <asp:TextBox ID="txtsmtpdomain" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="testemail" runat="server" />
            <asp:TextBox ID="txttestemail" runat="server" />
            <asp:LinkButton ID="TestSMTPSettings" runat="server" CssClass="dnnPrimaryAction" ResourceKey="TestSettings" OnClick="TestSMTPSettings_Click" />
        </div>
        <div class="dnnFormMessage dnnFormSuccess hidemelater" id="yesitdid" runat="server">Test Email Sent</div>
        <div class="dnnFormMessage dnnFormValidationSummary hidemelater" id="noitdidnt" runat="server">Error Sending test email</div>
    </asp:Panel>
</fieldset>
<h2 id="dnnSitePanel-GoogleRecaptcha" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded">Google reCaptcha</a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:Label ID="EnableRecaptcha" runat="server" ControlName="ChkEnableRecaptcha" Suffix=":" />
        <asp:CheckBox runat="server" ID="ChkEnableRecaptcha" AutoPostBack="true" OnCheckedChanged="ChkEnableRecaptcha_CheckedChanged" />
    </div>
    <asp:Panel ID="pnlRecaptcha" runat="server" Visible="false">
        <div class="dnnFormItem">
            <dnn:Label ID="PublicKey" runat="server" ControlName="txtpublickey" Suffix=":" />
            <asp:TextBox ID="txtpublickey" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="PrivateKey" runat="server" ControlName="txtprivatekey" Suffix=":" />
            <asp:TextBox ID="txtprivateKey" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="RecaptchaTheme" runat="server" ControlName="RecaptchaThemeDDL" Suffix=":" />
            <asp:DropDownList runat="server" ID="RecaptchaThemeDDL">
                <asp:ListItem>Light</asp:ListItem>
                <asp:ListItem>Dark</asp:ListItem>
            </asp:DropDownList>
        </div>
            <div class="dnnFormItem">
            <dnn:Label ID="blankrecaptcha" runat="server" ControlName="txtblankcaptcha" Suffix=":" />
            <asp:TextBox runat="server" ID="txtblankcaptcha"></asp:TextBox>
        </div>
            <div class="dnnFormItem">
            <dnn:Label ID="incorrectcaptcha" runat="server" ControlName="txtincorrectcaptcha" Suffix=":" />
            <asp:TextBox runat="server" ID="txtincorrectcaptcha"></asp:TextBox>
        </div>
    </asp:Panel>
</fieldset>
