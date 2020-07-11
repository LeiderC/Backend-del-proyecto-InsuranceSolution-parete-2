using Dapper;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class DeleteDocumentPermissionRepository : Repository<DeleteDocumentPermission>, IDeleteDocumentPermissionRepository
    {
        public DeleteDocumentPermissionRepository(string connectionString) : base(connectionString)
        {
        }

        public List<DeleteDocumentPermission> DeleteDocumentPermissionByUserNModule(int idUser, int module)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idUser", idUser);
            parameters.Add("@module", module);
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<DeleteDocumentPermission>("dbo.DeleteDocumentPermissionByUserModule", parameters,
                    commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
        }
    }
}