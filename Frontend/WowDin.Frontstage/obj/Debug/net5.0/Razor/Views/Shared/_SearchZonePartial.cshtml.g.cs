#pragma checksum "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Shared\_SearchZonePartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4b26e75ad196985f230adcf3f7933ce6a07df536"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__SearchZonePartial), @"mvc.1.0.view", @"/Views/Shared/_SearchZonePartial.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4b26e75ad196985f230adcf3f7933ce6a07df536", @"/Views/Shared/_SearchZonePartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"13600abd3733b49dab8efb536e9735dccd03b4d9", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared__SearchZonePartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Shared\_SearchZonePartial.cshtml"
  
    ViewData["Title"] = "SearchZone";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

<!-- 搜尋區塊 -->
 <div class=""row col-12 col-sm-12 col-lg-4 mx-auto p-0"">
    <div class=""row searchzone bg_blue justify-content-center"">
        <div onclick=""getSearchNearInfo()"" type=""button"" class=""searchzone_icon col-3 col-sm-3 col-lg-6 d-flex align-items-center flex-column"">
            <i class=""fas fa-street-view rounded-circle text_blue bg_white d-flex justify-content-center align-items-center""></i>
            <p class=""mt-2 text_white"">找附近</p>
        </div>
        <div type=""button"" class=""searchzone_icon searchzone_borderleft col-3 col-sm-3 col-lg-6 d-flex align-items-center flex-column"" data-bs-toggle=""modal"" data-bs-target=""#searchzone_finddistrict_modal"">
            <i class=""fas fa-compass rounded-circle text_blue bg_white d-flex justify-content-center align-items-center""></i>
            <p class=""mt-2 text_white"">找區域</p>
        </div>
        <hr class=""searchzone_baseline"">
        <div onclick=""getSearchOrderedInfo()"" type=""button"" class=""searchzone_icon searchzone_borderl");
            WriteLiteral(@"eft col-3 col-sm-3 col-lg-6 d-flex align-items-center flex-column"">
            <i class=""fas fa-history rounded-circle text_blue bg_white d-flex justify-content-center align-items-center""></i>
            <p class=""mt-2 text_white"">曾點過</p>
        </div>
    </div>
</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591