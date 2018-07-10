using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Domain;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Providers : IProviderRepository
    {

        public Domain.Provider ById(int id)
        {
            var provider = new Domain.Provider();

            using (SqlCommand command = new SqlCommand("SELECT Id, Name FROM Provider WHERE Id = @id", Tools.Connection()))
            {
                command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                Tools.OpenConnection();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        provider.Id = reader.GetInt32(0);
                        provider.Name = reader.GetString(1);
                    }
                }
                finally
                {
                    Tools.CloseConnection();
                }
            }

            return provider;
        }

        public IEnumerable<Provider> Paged(int requestedPage, int pageSize, string searchString, string orderByFieldName, bool orderByDescending)
        {
            var providers = new List<Domain.Provider>();
            var cmdText =  Tools.GetPagedExpression("Provider", "Id, Name", requestedPage, pageSize, orderByFieldName, orderByDescending);

            using (SqlCommand command = new SqlCommand(cmdText, Tools.Connection()))
            {
                Tools.OpenConnection();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        providers.Add(new Domain.Provider() { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                    }
                }
                finally
                {
                    Tools.CloseConnection();
                }
            }

            return providers;
        }

        public IEnumerable<Domain.Provider> All()
        {
            var providers = new List<Domain.Provider>();

            using (SqlCommand command = new SqlCommand("SELECT Id, Name FROM Provider ", Tools.Connection()))
            {
                Tools.OpenConnection();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        providers.Add(new Domain.Provider() { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                    }
                }
                finally
                {
                    Tools.CloseConnection();
                }
            }

            return providers;
        }

        public void AddProvider(string name)
        {
            using (SqlCommand command = new SqlCommand("INSERT INTO Provider (Name) VALUES (@Name)", Tools.Connection()))
            {
                command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar).Value = name;
                Tools.OpenConnection();
                try
                {
                     command.ExecuteNonQuery();
                    
                }
                finally
                {
                    Tools.CloseConnection();
                }
            }
             
        }

        public void DeleteProvider(int id )
        {
            DeleteProviderProduct(id);

            using (SqlCommand command = new SqlCommand("DELETE FROM Provider WHERE Id=@Id", Tools.Connection()))
            {
                Tools.OpenConnection();
                try
                {
                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();

                }
                finally
                {
                    Tools.CloseConnection();
                }
            }

        }

        public void DeleteProviderProduct(int providerId)
        {
            using (SqlCommand command = new SqlCommand("DELETE FROM ProductProvider WHERE ProviderId=@ProviderId", Tools.Connection()))
            {
                Tools.OpenConnection();
                try
                {
                command.Parameters.Add("@ProviderId", System.Data.SqlDbType.Int).Value = providerId;
                    command.ExecuteNonQuery();

                }
                finally
                {
                    Tools.CloseConnection();
                }
            }

        }
    }
}
