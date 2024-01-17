using AomTung.Share.Model.Member;

namespace AomTung.Domain.Member.Abstractions
{
    public interface IMemberService
    {
        Task<IEnumerable<GetMemberModel>> GetAll();
    }
}
