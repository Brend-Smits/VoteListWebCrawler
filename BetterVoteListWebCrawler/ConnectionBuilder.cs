using MySql.Data.MySqlClient;

namespace BetterVoteListWebCrawler
{
    public class ConnectionBuilder
    {
        private string Database = "database";
        private string Server = "localhost";
        private string User = "user";
        private string Password = "password";

        public MySqlConnection ConnectionString()
        {
            return new MySqlConnection($"Server={Server};Database={Database};UID={User};Password={Password};SslMode=none");
        }
    }
}