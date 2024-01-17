using AomTung.DataAccessLayer.Data;
using AomTung.Domain.Member.Abstractions;
using AomTung.Share.Model.Member;
using Microsoft.EntityFrameworkCore;

namespace AomTung.DataAccessLayer.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AomTungExtendDbContext dbContext;

        public MemberRepository(AomTungExtendDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<GetMemberModel>> GetAll()
        {
            var query = await dbContext.tbl_members
                .AsNoTracking()
                .ToListAsync();

            var result = query.Select(x => new GetMemberModel
            {
                id = x.id,
                username = x.username,
                firstname = x.firstname,
                lastname = x.lastname,
                date_of_birth = x.date_of_birth,
            });

            return result;
        }
    }
}
