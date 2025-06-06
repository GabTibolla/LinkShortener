
using System.Text;

namespace LinkShortener.DAO.MSSQL
{
    public class LinkShortenerDBMSSQL : LinkShortener.DAO.LinkShortenerDB
    {
        private readonly Microsoft.Data.SqlClient.SqlConnection _connection;

        public LinkShortenerDBMSSQL(string connection) : base(connection)
        {
            _connection = new Microsoft.Data.SqlClient.SqlConnection(connection);
        }

        public override bool AddLink(LinkShortener.Data.LinkShortener link)
        {
            try
            {
                _connection.Open();
                _connection.BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO Links (ShortUrl, LongUrl, Clicks) ");
                sql.Append("VALUES (@ShortUrl, @LongUrl, @Clicks);");

                var command = new Microsoft.Data.SqlClient.SqlCommand(sql.ToString(), _connection);
                command.Parameters.AddWithValue("@ShortUrl", link.shortUrl);
                command.Parameters.AddWithValue("@LongUrl", link.longUrl);
                command.Parameters.AddWithValue("@Clicks", link.clicks);

                int rowsAffected = command.ExecuteNonQuery();
                _connection.BeginTransaction().Commit();

                return rowsAffected > 0;
            }
            catch (Exception)
            {
                _connection.BeginTransaction().Rollback();
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        public override LinkShortener.Data.LinkShortener? GetLink(string shortLiunk)
        {
            try
            {
                _connection.Open();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT ShortUrl, LongUrl, Clicks FROM Links ");
                sql.Append("WHERE ShortUrl = @ShortUrl;");

                var command = new Microsoft.Data.SqlClient.SqlCommand(sql.ToString(), _connection);
                command.Parameters.AddWithValue("@ShortUrl", shortLiunk);
                var reader = command.ExecuteReader();


                LinkShortener.Data.LinkShortener link = null;

                while(reader.Read())
                {
                    link = new LinkShortener.Data.LinkShortener
                    {
                        shortUrl = reader["ShortUrl"] as string,
                        longUrl = reader["LongUrl"] as string,
                        clicks = Convert.ToInt32(reader["Clicks"])
                    };

                    link.clicks++;
                }
                return link;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
