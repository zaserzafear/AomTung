using AomTung.Share.Model.Member;

namespace AomTung.Domain.Member.Abstractions
{
    public interface IMemberRepository
    {
        Task<IEnumerable<GetMemberModel>> GetAll(int skip = 0, int take = 10);
        Task<GetMemberModel> AddSingle(AddMemberModel model);
    }
}
