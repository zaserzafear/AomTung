using AomTung.DataAccessLayer.Data;
using AomTung.DataAccessLayer.DataModels;
using AomTung.DataAccessLayer.Extensions;
using AomTung.Domain.Member.Abstractions;
using AomTung.Share.Model.Member;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace AomTung.DataAccessLayer.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AomTungExtendDbContext dbContext;
        private readonly IMySqlHelper mySqlHelper;

        public MemberRepository(AomTungExtendDbContext dbContext, IMySqlHelper mySqlHelper)
        {
            this.dbContext = dbContext;
            this.mySqlHelper = mySqlHelper;
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

            var decrypt = DecryptTblMember(query);
            var result = decrypt.Select(x => new GetMemberModel
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
                .OrderBy(x => x.id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var decrypt = DecryptTblMember(query);
            var result = decrypt.Select(x => new GetMemberModel
            {
                id = x.id,
                username = x.username,
                firstname = x.firstname,
                lastname = x.lastname,
                date_of_birth = x.date_of_birth,
            });

            return result;
        }

        private IEnumerable<tbl_member> DecryptTblMember(IEnumerable<tbl_member> model)
        {
            var saltKey = dbContext.GetAesSaltKey();

            return model.Select(x =>
            {
                x.username = mySqlHelper.aes_decrypt(x.username, saltKey);
                x.firstname = mySqlHelper.aes_decrypt(x.firstname, saltKey);
                x.lastname = mySqlHelper.aes_decrypt(x.lastname, saltKey);

                return x;
            }
            );
        }
    }
}
