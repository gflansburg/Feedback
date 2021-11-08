<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Gafware.Modules.Feedback.View" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="Gafware.Recaptcha.Web.UI.Controls" Assembly="Gafware.Feedback" %>
<div id="feedbackModule">
    <div class="feedback">
        <ul>
            <li>
                <a id="btnFeedback" role="aria-haspopup" href="javascript:toggleFeedbackWindow()"><%= ButtonText %></a>
            </li>
        </ul>
    </div>
    <div id="feedbackWindow">
        <div style="float:left; line-height: 19px;"><h3 style="line-height: 19px; margin: 0;">Feedback</h3></div>
        <div style="float:right; line-height: 19px; vertical-align: middle"><a href="javascript:setFocusToFeedbackButton()"><img src="<%= ControlPath %>/images/close.gif" alt="Close feedback button" title="Close Feedback" border="0" /></a></div>
        <br style="clear: both" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="IsValid" runat="server" Value="false" />
                <fieldset>
                    <div class="dnnFormItem">
                        <dnn:Label ID="lblName" runat="server" ResourceKey="lblName" ControlName="txtName" Suffix=":" /> 
                        <asp:TextBox ID="txtName" runat="server" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="Feedback" />
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="Feedback" ErrorMessage="Please enter your name." SetFocusOnError="true" />
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblEmail" runat="server" ResourceKey="lblEmail" ControlName="txtEmail" Suffix=":" />
                        <asp:TextBox ID="txtEmail" type="email" runat="server" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="Feedback" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="Feedback" ErrorMessage="Please enter your email address." SetFocusOnError="true" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Please enter a properly formated email address." Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="Feedback" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" SetFocusOnError="true"></asp:RegularExpressionValidator>
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblPage" runat="server" ResourceKey="lblPage" Suffix=":" />
                        <asp:label ID="lblTitle" runat="server" />
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblBody" runat="server" ResourceKey="lblBody" ControlName="txtBody" />
                        <asp:TextBox ID="txtBody" TextMode="MultiLine" Rows="10" runat="server" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="Feedback" />
                        <asp:RequiredFieldValidator ID="rfvBody" runat="server" ControlToValidate="txtBody" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="Feedback" ErrorMessage="Please enter your feedback." SetFocusOnError="true" />
                    </div>
                    <div>
                        <div style="text-align: center; width: 302px; margin: 0 auto 0 auto;"><cc1:GoogleReCaptcha ID="Recaptcha1"  runat="server" /></div>
                        <br style="clear: both;" />
                        <div style="text-align: center; width: 100%;"><asp:CustomValidator ID="rfvRecaptcha" runat="server" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ContactForm" ClientValidationFunction="validateRecaptchaLength" Enabled="false" /></div>
                        <div style="text-align: center; width: 100%;"><asp:CustomValidator ID="cvRecaptcha" runat="server" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ContactForm" Enabled="false" /></div>
                        <br style="clear: both;" />
                    </div>
                    <div class="dnnFormItem">
                        <dnn:label ID="lblSend" runat="server" ResourceKey="lblSend" ControlName="btnSend" />
	                    <asp:LinkButton ID="btnSend" runat="server" Text="Send" OnClick="btnSend_Click" CssClass="dnnPrimaryAction" ValidationGroup="Feedback" CausesValidation="true" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<script type="text/javascript">
    function validateRecaptchaLength(oSrc, args) {
        var text = $('#<%= Recaptcha1.ClientID %> #recaptcha_response_field').val();
        args.IsValid = (text.length > 0);
    }

    function onLoadreCaptcha() {
        try {
            $("#<%= Recaptcha1.ClientID %>_Ctrl").empty();
            grecaptcha.render('<%= Recaptcha1.ClientID %>_Ctrl', {
                'sitekey': '<%= GetreCaptchaPublicKey %>',
                'theme': '<%= GetreCaptchaTheme %>'
            });
        } catch (e) { }
    }

    function bindOutsideFeedBack() {
        $('#feedbackModule').bind('clickoutside', function (event) {
            if ($('#feedbackWindow').is(':visible')) {
                $('#feedbackWindow').hide();
            }
        });
        $('#feedbackModule').bind('focusoutside', function (event) {
            if ($('#feedbackWindow').is(':visible')) {
                $('#feedbackWindow').hide();
            }
        });
    }

    function setFocusToFeedbackButton() {
        $('#btnFeedback').focus();
        if ($('#feedbackWindow').is(':visible')) {
            $('#feedbackWindow').hide();
        }
    }

    function toggleFeedbackWindow() {
        if ($('#feedbackWindow').is(':visible')) {
            $('#feedbackWindow').hide();
        } else {
            $('#feedbackWindow').show();
            $('#<%= txtName.ClientID %>').focus();
        }
    }

    (function ($, Sys) {
        $(document).ready(function () {
            bindOutsideFeedBack();
            var postBackElement = null;
            Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(function (sender, args) {
                try {
                    postBackElement = args.get_postBackElement();
                    if (postBackElement.id === '<%= btnSend.ClientID %>') {
                        if ($('#feedbackWindow').is(':visible')) {
                            $('#feedbackWindow').block({ message: '<img src=\"<%= ControlPath %>/images/loading.gif\" />', css: { padding: '15px 0px 10px 0px' } });
                        }
                    }
                } catch (err) { }
            });
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (sender, args) {
                try {
                    if (postBackElement && postBackElement.id === '<%= btnSend.ClientID %>') {
                        postBackElement = null;
                        if ($('#feedbackWindow').is(':visible')) {
                            $('#feedbackWindow').unblock();
                        }
                        if ($('#<%= IsValid.ClientID %>').val() == 'true') {
                            $.dnnAlert({
                                text: "<%= AlertMessage %>",
                                dialogClass: 'dnnFormPopup',
                                title: "<%= AlertTitle %>",
                                modal: true
                            });
                            setFocusToFeedbackButton();
                        } else {
                            if ($('#feedbackWindow').is(':visible') === false) {
                                $('#feedbackWindow').show();
                            }
                            $('#btnFeedback').focus();
                        }
                        if (<%= EnableGooglereCaptcha.ToString().ToLower() %>) {
                            setTimeout(onLoadreCaptcha, 1000);
                        }
                    }
                } catch (err) { }
            });
        });
    }(jQuery, window.Sys));

</script>
