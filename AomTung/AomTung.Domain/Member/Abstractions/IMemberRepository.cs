using AomTung.Share.Model.Member;

namespace AomTung.Domain.Member.Abstractions
{
    public interface IMemberRepository
    {
        Task<IEnumerable<GetMemberModel>> GetAll();
    }
}
