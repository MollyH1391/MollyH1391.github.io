using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WowDin.Backstage.Models;
using WowDin.Backstage.Models.Dto.Member;
using WowDin.Backstage.Models.ViewModel.Member;
using WowDin.Backstage.Services.Interface;

namespace WowDin.Backstage.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberApiController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IMapper _mapper;
        public MemberApiController(IMemberService memberService, IMapper mapper)
        {
            _memberService = memberService;
            _mapper = mapper;
        }

        [HttpGet("getMemberPoint")]
        public IActionResult GetMemberPoint(int brandId)
        {
            brandId = int.Parse(User.Identity.Name);

            var result = _memberService.GetBrandMemberData(brandId);
            var dtos = (IEnumerable<GetBrandMemberDataDto>)result.Result;
            var memberPointVM = dtos.Select(x => _mapper.Map<MemberPointViewModel>(x));
            result.Result = memberPointVM;

            return Ok(result);
        }

        [HttpGet("getCardGrading")]
        public IActionResult GetMemberCardGrading(int brandId)
        {
            brandId = int.Parse(User.Identity.Name);

            var dto = _memberService.GetCardGradingData(brandId);
            var cardGradingVM = dto.Select(x => _mapper.Map<MemberCardGradingViewModel>(x));

            return Ok(cardGradingVM);

            //var result = _memberService.GetCardGradingData(brandId);
            //var data = (IEnumerable<MemberCardGradingViewModel>)result.Result;
            //var cardGradingVM = data.Select(x => _mapper.Map<MemberCardGradingViewModel>(x));
            //result.Result = cardGradingVM;

            //return Ok(result);
        }

        [HttpPost("createCardGrading")]
        public IActionResult CreateMemberCardGrading([FromBody] CreateCardCradingVM query)
        {
            var brandId = int.Parse(User.Identity.Name);
            var dto = _mapper.Map<CreateCardCradingDto>(query);
            dto.BrandId = brandId;

            var result = _memberService.CreateCardGrading(dto, brandId);
            return Ok(result);
        }

        [HttpDelete("deleteCard")]
        public IActionResult DeleteCard([FromBody] DeleteCardVM query)
        {
            var input = _mapper.Map<DeleteCardDto>(query);
            var result = _memberService.DeleteCard(input);

            return Ok(result);
        }

        [HttpPut("updateCard")]
        public IActionResult UpdateCard([FromBody] UpdateCardVM query)
        {

            var input = _mapper.Map<UpdateCardDto>(query);
            var result = _memberService.UpdateCard(input);

            return Ok(result);
        }

        [HttpPost("uploadImg")]
        public async Task<IActionResult> UploadImg(IFormFile file)
        {
            var result = await _memberService.UploadImg(file);
            return Ok(result);
        }
    }
}
