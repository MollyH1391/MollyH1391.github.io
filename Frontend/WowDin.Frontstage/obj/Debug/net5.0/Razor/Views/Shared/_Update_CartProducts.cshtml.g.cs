#pragma checksum "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Shared\_Update_CartProducts.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fea55cbf6e65283602532eb6cc47ffcb93998c3b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__Update_CartProducts), @"mvc.1.0.view", @"/Views/Shared/_Update_CartProducts.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fea55cbf6e65283602532eb6cc47ffcb93998c3b", @"/Views/Shared/_Update_CartProducts.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"13600abd3733b49dab8efb536e9735dccd03b4d9", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared__Update_CartProducts : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<WowDin.Frontstage.Models.ViewModel.Order.CartDetailViewModel>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Shared\_Update_CartProducts.cshtml"
      foreach(var cd in Model.ProductDetailsByCart)
    {
                                

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div id=\"productListByUser\"");
            BeginWriteAttribute("userid", " userid=\"", 197, "\"", 222, 1);
#nullable restore
#line 6 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Shared\_Update_CartProducts.cshtml"
WriteAttributeValue("", 206, cd.UserAcountId, 206, 16, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n            ");
#nullable restore
#line 7 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Shared\_Update_CartProducts.cshtml"
       Write(await Html.PartialAsync("_CartDetail_GroupBuyList", cd));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n");
#nullable restore
#line 9 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Shared\_Update_CartProducts.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<WowDin.Frontstage.Models.ViewModel.Order.CartDetailViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
