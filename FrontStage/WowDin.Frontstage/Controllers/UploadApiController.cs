using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WowDin.Frontstage.Services.Member.Interface;

namespace WowDin.Frontstage.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UploadApiController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public UploadApiController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpPost("uploadImg")]
        public async Task<IActionResult> UploadImg(IFormFile file)
        {
            var result = await _memberService.UploadImg(file);
            return Ok(result);
        }
    }
}
