#pragma checksum "C:\Users\molly\OneDrive\AC學期一\BS專題\FrontStage_WowDin_Azure\WowDin\WowDin\WowDin.Frontstage\Views\Shared\_MenuMainPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2b81a3e7af14d75dc3fa4fcdadd6357d2b02a1f9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__MenuMainPartial), @"mvc.1.0.view", @"/Views/Shared/_MenuMainPartial.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2b81a3e7af14d75dc3fa4fcdadd6357d2b02a1f9", @"/Views/Shared/_MenuMainPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"13600abd3733b49dab8efb536e9735dccd03b4d9", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared__MenuMainPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
    <div class=""row g-0 g-lg-2 mt-3 position-relative"">
        <div class=""menu_nav col-12 d-lg-none"">
            <div class=""menu_first_color_bg position-relative d-flex h-100"">
                <div class=""swiper menu_nav_swiper"">
                    <!-- Additional required wrapper -->
                    <ul class=""menu_nav_class swiper-wrapper"">
                        <!-- Slides -->
                       <template id=""menu_nav_class"">
                           <li class=""swiper-slide"">
                                <a class=""menu_category_anchor fw-bold nav-link menu_nav_active py-2 h-100 text_gray""></a>
                            </li>
                       </template>
                    </ul>
                </div>
                <input type=""checkbox"" id=""menu_nav_control"" class=""d-none"">
                <ul class=""menu_dropdown_menu position-absolute bg_white w-100 row row-cols-2 p-4 m-auto"">
                   <template id=""menu_nav_class_drop"">
                        ");
            WriteLiteral(@"<li class=""col p-2"">
                            <a class=""menu_category_anchor dropdown-item bg_page rounded_corner_small""></a>
                        </li>
                   </template>
                </ul>
                <div class=""nav-item dropdown d-inline-block p-2"">
                    <label class=""nav-link dropdown-toggle rounded_corner_small bg_white h-100 d-flex justify-content-center align-items-center""
                           href=""#"" role=""button"" aria-expanded=""false"" for=""menu_nav_control"">
                        <i class=""fas fa-angle-down menu_first_color_text""></i>
                    </label>
                </div>
            </div>
        </div>
        <div class=""menu_main_col col-12 col-lg-4"">
            <template id=""menu_borad_temp"">
                <div class=""menu_borad mb-2"">
                    <div class=""menu_borad_outfit bg_white menu_shadow rounded_corner"">
                        <div class=""menu_borad_title row justify-content-between p-3"">
   ");
            WriteLiteral(@"                         <h2 class=""menu_category col col-9 menu_first_color_text fw-bold d-inline-block fz_16"">
                                分類分類
                            </h2>
                            <div class=""col col-3 menu_borad_select_show"">
                                <div class=""row"">
                                    <span class=""col text-end p-0""></span>
                                </div>
                            </div>
                        </div>
                        <ul>
");
            WriteLiteral(@"                        </ul>
                    </div>
                </div>
            </template>

            <!-- 填入ul -->
            <template id=""menu_item_temp"">
                <li class=""menu_borad_item p-3"">
                    <button data-bs-toggle=""modal"" data-bs-target=""#menu_item_modal""
                        class=""row justify-content-between align-items-center w-100"">
                        <div class=""col col-10 d-flex align-items-center text-start"">
                            <div class=""menu_borad_item_img"">
                                <img src=""https://fakeimg.pl/53x40/200"" class=""h-100 rounded_corner_small"">
                            </div>
                            <p class=""m-0 menu_borad_item_name mx-2 fw-bold"">
                                名稱名稱
                            </p>
                            <span class=""badge bg_pink""></span>
                        </div>
                        <div class=""col col-2 menu_borad_select_show"">
     ");
            WriteLiteral(@"                       <div class=""row"">
                                <span class=""menu_item_price col text-end p-0""></span>
                            </div>
                        </div>
                    </button>
                </li>
            </template>
        </div>
        <div class=""menu_main_col col-12 col-lg-4"">
        </div>
        <div class=""menu_main_col col-12 col-lg-4"">
        </div>
    </div>

");
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
