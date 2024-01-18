using AomTung.Domain.Member.Abstractions;
using AomTung.Share.Model.Member;
using Microsoft.AspNetCore.Mvc;

namespace AomTung.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : BaseController
    {
        private readonly IMemberService memberService;

        public MemberController(IMemberService memberService)
        {
            this.memberService = memberService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await memberService.GetAll();

            return Ok(result);
        }

        [HttpPost("AddMember")]
        public async Task<IActionResult> AddMember([FromBody] AddMemberModel model)
        {
            var result = await memberService.AddSingle(model);

            return Ok(result);
        }
    }
}
