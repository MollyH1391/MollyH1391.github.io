#pragma checksum "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Member\MyPoint.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "bc14d4620c12690adfe6453509e5b251fbb4ecc9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Member_MyPoint), @"mvc.1.0.view", @"/Views/Member/MyPoint.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\_ViewImports.cshtml"
using WowDin.Frontstage;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\_ViewImports.cshtml"
using WowDin.Frontstage.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\_ViewImports.cshtml"
using WowDin.Frontstage.Models.ViewModel.PartialView;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\_ViewImports.cshtml"
using WowDin.Frontstage.Models.ViewModel.Store;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\_ViewImports.cshtml"
using WowDin.Frontstage.Models.ViewModel.Home;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Member\MyPoint.cshtml"
using WowDin.Frontstage.Models.ViewModel.Member;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bc14d4620c12690adfe6453509e5b251fbb4ecc9", @"/Views/Member/MyPoint.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"13600abd3733b49dab8efb536e9735dccd03b4d9", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Member_MyPoint : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<MemberMyPointViewModel>>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/Member/member_style.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Member\MyPoint.cshtml"
  
    ViewData["Title"] = "點數歷史";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral(@"    ;
    <main class=""bg_page vh-100 pt-3 d-flex align-items-center justify-content-center"">
        <div class=""account_block bg-white rounded-4 overflow-hidden shadow"">
            <div class=""position-relative"">
                <img class=""w-100"" src=""/img/Member/member_card.png"" alt=""member_card"">
                <div class=""photo_size rounded-circle bg-info mx-auto position-absolute"">
                    <div class=""member_icon_size rounded-circle fb_bg d-flex justify-content-center position-absolute""><img class=""w-50"" src=""/img/Member/login_fb_icon.svg"" alt=""facebook""></div>
                </div>
            </div>
            <div class=""d-flex  justify-content-center mt-1"">
                <span class=""fs-13 me-1"">王小明</span>
            </div>

            <div class=""d-flex text_blue justify-content-between fw-bold px-5 mt-4"">
                <h5 class=""fs-16 "">目前點數</h5>
                <p class=""mb-0 fs-16"">");
#nullable restore
#line 23 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Member\MyPoint.cshtml"
                                 Write(ViewData["point"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n            </div>\r\n\r\n            <div class=\"pointHistory_card px-5 mt-4 overflow-auto\">\r\n\r\n\r\n\r\n\r\n");
#nullable restore
#line 31 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Member\MyPoint.cshtml"
                 foreach (MemberMyPointViewModel index in Model)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                    <div class=""d-flex align-items-center mb-3"">
                        <div class=""pointHistory_cardtitle_size bg_blue me-2 rounded-1""></div>
                        <div class=""d-flex flex-column"">
                            <p class=""mb-0 fs-15 pointHistory_cardtext_lh"">");
#nullable restore
#line 36 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Member\MyPoint.cshtml"
                                                                      Write(Html.DisplayName(index.ConsumeType));

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                            <span class=\"fs-12 text_gray pointHistory_cardtext_lh\">訂單編號: ");
#nullable restore
#line 37 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Member\MyPoint.cshtml"
                                                                                    Write(Html.DisplayName(index.OrderId.ToString()));

#line default
#line hidden
#nullable disable
            WriteLiteral(" </span>\r\n                            <span class=\"fs-12 text_gray pointHistory_cardtext_lh\">");
#nullable restore
#line 38 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Member\MyPoint.cshtml"
                                                                              Write(Html.DisplayName(index.Date.ToString()));

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n                        </div>\r\n                        <span class=\"text_blue ms-auto\">");
#nullable restore
#line 40 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Member\MyPoint.cshtml"
                                                   Write(Html.DisplayName(index.PointAmount.ToString()));

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n                    </div>\r\n");
#nullable restore
#line 42 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Member\MyPoint.cshtml"

                }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n\r\n\r\n            </div>\r\n        </div>\r\n    </main>\r\n\r\n");
            DefineSection("topCSS", async() => {
                WriteLiteral("\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "bc14d4620c12690adfe6453509e5b251fbb4ecc99512", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
            }
            );
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<MemberMyPointViewModel>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591