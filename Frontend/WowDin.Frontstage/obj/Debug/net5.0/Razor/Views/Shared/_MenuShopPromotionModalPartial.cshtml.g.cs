#pragma checksum "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Shared\_MenuShopPromotionModalPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8dc8eeffee810287233f0888d6bb4f4110d39db5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__MenuShopPromotionModalPartial), @"mvc.1.0.view", @"/Views/Shared/_MenuShopPromotionModalPartial.cshtml")]
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
#line 6 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\_ViewImports.cshtml"
using WowDin.Frontstage.Models.ViewModel.Member;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8dc8eeffee810287233f0888d6bb4f4110d39db5", @"/Views/Shared/_MenuShopPromotionModalPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"13600abd3733b49dab8efb536e9735dccd03b4d9", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared__MenuShopPromotionModalPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<StoreShopMenuViewModel>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<div class=""modal fade"" id=""menu_promotion_modal"" tabindex=""-1"" aria-labelledby=""din_ModalLabel"" aria-hidden=""true"">
    <div class=""modal-dialog modal-dialog-centered modal-dialog-scrollable"">
        <div class=""modal-content"">
            <div class=""menu_modal_header"">
                <div class=""d-flex px-3 pt-3 align-items-center"">
                    <h3 class=""fz_16 fw-bold m-0"">活動與優惠</h3>
                    <button type=""button"" class=""ms-auto"" data-bs-dismiss=""modal"" aria-label=""Close"">
                        <i class=""fas fa-times-circle text_pink fz_18""></i>
                    </button>
                </div>
            </div>
            <div class=""modal-body pt-0"">
");
#nullable restore
#line 15 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Shared\_MenuShopPromotionModalPartial.cshtml"
                 foreach(var promo in Model.Promotions)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"bg_page p-4\">\r\n                        <h4 class=\"menu_first_color_text border-bottom\">");
#nullable restore
#line 18 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Shared\_MenuShopPromotionModalPartial.cshtml"
                                                                   Write(promo.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n                        <p>\r\n                            ");
#nullable restore
#line 20 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Shared\_MenuShopPromotionModalPartial.cshtml"
                       Write(promo.Description);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </p>\r\n                    </div>\r\n");
#nullable restore
#line 23 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Shared\_MenuShopPromotionModalPartial.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                \r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<StoreShopMenuViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
