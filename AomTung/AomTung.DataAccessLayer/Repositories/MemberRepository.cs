using AomTung.DataAccessLayer.Data;
using AomTung.Domain.Member.Abstractions;
using AomTung.Share.Model.Member;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace AomTung.DataAccessLayer.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AomTungExtendDbContext dbContext;

        public MemberRepository(AomTungExtendDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<GetMemberModel> AddSingle(AddMemberModel model)
        {
            var usernameParam = new MySqlParameter("@p_username", model.username);
            var passwordParam = new MySqlParameter("@p_password", model.password);
            var firstnameParam = new MySqlParameter("@p_firstname", model.firstname);
            var lastnameParam = new MySqlParameter("@p_lastname", model.lastname);
            var dateOfBirthParam = new MySqlParameter("@p_date_of_birth", model.date_of_birth ?? (object)DBNull.Value);

            var query = await dbContext.tbl_members
                .FromSqlRaw("CALL MemberInsert(@p_username, @p_password, @p_firstname, @p_lastname, @p_date_of_birth)",
                    usernameParam, passwordParam, firstnameParam, lastnameParam, dateOfBirthParam)
                .ToListAsync();

            var result = query.Select(x => new GetMemberModel
            {
                id = x.id,
                username = x.username,
                firstname = x.firstname,
                lastname = x.lastname,
                date_of_birth = x.date_of_birth,
            }).Single();

            return result;
        }

        public async Task<IEnumerable<GetMemberModel>> GetAll(int skip, int take)
        {
            var query = await dbContext.tbl_members
                .AsNoTracking()
                .Where(x => !x.is_deleted)
                .Skip(skip)
                .Take(take)
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
