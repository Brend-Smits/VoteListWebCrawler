using MySql.Data.MySqlClient;

namespace BetterVoteListWebCrawler
{
    public class Database
    {
        static ConnectionBuilder connectionBuilder = new ConnectionBuilder();

        public static MySqlConnection GetConnectionString()
        {
            return connectionBuilder.ConnectionString();
        }
    }
}