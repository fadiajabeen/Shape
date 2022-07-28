using Dapper;
using Shape.Models;
using System.Data;
using System.Data.Common;

namespace Shape.Repositories
{
    public class UserRepository : Contracts.IUserRepository 
    {
        readonly ShapeContext dbContext;

        public UserRepository(ShapeContext context)
        {
            dbContext = context;
        }

        public async Task<User> SignUpAsync(User user)
        {
            DbConnection? connection = null;

            string query = "INSERT INTO [User] (Firstname,Lastname,Email,Password,PasswordUpdateRequired" +
                ",CreatedDate,UpdatedDate) VALUES (@Firstname,@Lastname,@Email,@Password,@PasswordUpdateRequired," +
                "@CreatedDate, @UpdatedDate) SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            DynamicParameters parameters = new();
            parameters.Add("Firstname", user.Firstname);
            parameters.Add("Lastname", user.Lastname);
            parameters.Add("Email", user.Email);
            parameters.Add("Password", user.Password);
            parameters.Add("PasswordUpdateRequired", user.PasswordUpdateRequired);
            parameters.Add("CreatedDate", user.CreatedDate);
            parameters.Add("UpdatedDate", user.UpdatedDate);

            try
            {
                connection = dbContext.GetConnection();
                user.Id = await connection.QuerySingleAsync<long>(query, parameters);
                connection.Close();
            }
            catch { throw; }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return user;
        }
    }
}
