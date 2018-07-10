using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace DataLayer
{
    public class Products:IProductRepository
    {

        public Domain.Product ById(int id)
        {
            var product = new Domain.Product();

            using (SqlCommand command = new SqlCommand("SELECT Id, Name, Description FROM Product WHERE Id = @id", Tools.Connection()))
            {
                command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                Tools.OpenConnection();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        product.Id = reader.GetInt32(0);
                        product.Name = reader.GetString(1);
                        product.Description = reader.GetString(2);
                    }
                }
                finally
                {
                    Tools.CloseConnection();
                }
            }

            product.Providers = GetProviders(id);
            return product;
        }

        public IEnumerable<Product> Paged(int requestedPage, int pageSize, string searchString, string orderByFieldName, bool orderByDescending)
        {
            var products = new List< Domain.Product>();
            var cmdText =  Tools.GetPagedExpression("Product", "Id, Name, Description",requestedPage, pageSize, orderByFieldName, orderByDescending);

            using (SqlCommand command = new SqlCommand(cmdText, Tools.Connection()))
            {
                Tools.OpenConnection();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(new Domain.Product() { Id = reader.GetInt32(0), Name = reader.GetString(1), Description = reader.GetString(2) });
                    }
                }
                finally
                {
                    Tools.CloseConnection();
                }
            }

            return products;
        }

        public IEnumerable< Domain.Product >All( )
        {
            var products = new List< Domain.Product>();

            using (SqlCommand command = new SqlCommand("SELECT Id, Name, Description FROM Product ", Tools.Connection()))
            {
                Tools.OpenConnection();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(new Domain.Product() { Id = reader.GetInt32(0), Name = reader.GetString(1),  Description = reader.GetString(2) });
                    }
                }
                finally
                {
                    Tools.CloseConnection();
                }
            }

            return products;
        }

        private IEnumerable<Domain.Provider> GetProviders(int productId)
        {
            var providers = new List<Domain.Provider>();

            using (SqlCommand command = new SqlCommand("SELECT ProviderId,Name FROM ProductProvider" +
                                                    " INNER JOIN Provider on ProviderId=Id" +
                                                    " WHERE ProductId = @ProductId", Tools.Connection()))
            {
                Tools.OpenConnection();
                try
                {
                    command.Parameters.Add("@ProductId", System.Data.SqlDbType.Int).Value = productId;
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

        public void AddProduct(string name,string description)
        {
            using (SqlCommand command = new SqlCommand("INSERT INTO Product (Name,Description) VALUES (@Name,@Description)", Tools.Connection()))
            {
                command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar).Value = name;
                command.Parameters.Add("@Description", System.Data.SqlDbType.VarChar).Value = description;
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

        public void AddProductProvider(int productId,int providerId)
        {
            using (SqlCommand command = new SqlCommand("INSERT INTO ProductProvider (ProductId,ProviderId) VALUES (@ProductId,@ProviderId)", Tools.Connection()))
            {
                command.Parameters.Add("@ProductId", System.Data.SqlDbType.Int).Value = productId;
                command.Parameters.Add("@ProviderId", System.Data.SqlDbType.Int).Value = providerId;
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

        public void DeleteProduct(int id)
        {
            DeleteAllProductProviders(id);

            using (SqlCommand command = new SqlCommand("DELETE FROM Product WHERE Id=@Id", Tools.Connection()))
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

        public void DeleteAllProductProviders(int productId)
        {
            using (SqlCommand command = new SqlCommand("DELETE FROM ProductProvider WHERE ProductId=@ProductId", Tools.Connection()))
            {
                Tools.OpenConnection();
                try
                {
                    command.Parameters.Add("@ProductId", System.Data.SqlDbType.Int).Value = productId;
                    command.ExecuteNonQuery();

                }
                finally
                {
                    Tools.CloseConnection();
                }
            }

        }
        public void DeleteProductProvider(int productId,int providerId)
        {
            using (SqlCommand command = new SqlCommand("DELETE FROM ProductProvider WHERE ProductId=@ProductId and ProviderId=@ProviderId", Tools.Connection()))
            {
                Tools.OpenConnection();
                try
                {
                    command.Parameters.Add("@ProductId", System.Data.SqlDbType.Int).Value = productId;
                    command.Parameters.Add("@ProviderId", System.Data.SqlDbType.Int).Value = providerId;
                    command.ExecuteNonQuery();

                }
                finally
                {
                    Tools.CloseConnection();
                }
            }

        }
        public void UpdatePoduct(Domain.Product product)
        {

            using (SqlCommand command = new SqlCommand("UPDATE Product SET Name=@Name,Description=@Description WHERE Id=@Id", Tools.Connection()))
            {
                Tools.OpenConnection();
                try
                {
                    command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar).Value = product.Name;
                    command.Parameters.Add("@Description", System.Data.SqlDbType.VarChar).Value = product.Description;
                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = product.Id;
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
