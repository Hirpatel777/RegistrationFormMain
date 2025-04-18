using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using UserRegistrationForm.Models;
using Dapper;
namespace UserRegistrationForm.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly string _connectionString;

        public ContactRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Contact>> GetAllContacts()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<Contact>("sp_GetAllContacts",
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Contact> GetContactById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var parameters = new { ContactId = id };
                return await connection.QueryFirstOrDefaultAsync<Contact>("sp_GetContactById",
                    parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> AddContact(Contact contact)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@Name", contact.Name);
                parameters.Add("@Email", contact.Email);
                parameters.Add("@Phone", contact.Phone);
                parameters.Add("@Address", contact.Address);
                parameters.Add("@StateId", contact.StateId);
                parameters.Add("@CityId", contact.CityId);
                parameters.Add("@Agree", contact.Agree);
                parameters.Add("@ContactId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("sp_AddContact", parameters,
                    commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@ContactId");
            }
        }

        public async Task UpdateContact(Contact contact)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var parameters = new
                {
                    ContactId = contact.ContactId,
                    Name = contact.Name,
                    Email = contact.Email,
                    Phone = contact.Phone,
                    Address = contact.Address,
                    StateId = contact.StateId,
                    CityId = contact.CityId,
                    Agree = contact.Agree
                };

                await connection.ExecuteAsync("sp_UpdateContact", parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteContact(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var parameters = new { ContactId = id };
                await connection.ExecuteAsync("sp_DeleteContact", parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<State>> GetAllStates()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<State>("sp_GetAllStates",
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<City>> GetCitiesByState(int stateId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var parameters = new { StateId = stateId };
                return await connection.QueryAsync<City>("sp_GetCitiesByState",
                    parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }

}
