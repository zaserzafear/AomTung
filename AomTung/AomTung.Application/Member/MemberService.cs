using AomTung.Domain.Member.Abstractions;
using AomTung.Share.Model.Member;

namespace AomTung.Application.Member
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            this.memberRepository = memberRepository;
        }

        public async Task<IEnumerable<GetMemberModel>> GetAll()
        {
            var result = await memberRepository.GetAll();

            return result;
        }
    }
}
