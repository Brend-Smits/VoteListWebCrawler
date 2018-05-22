using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using MySql.Data.MySqlClient;

namespace BetterVoteListWebCrawler
{
    class Program
    {
        private static int checkId;

        static void Main(string[] args)
        {
            GetLatestCheckId();
            while (true)
            {
                checkId++;
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("Check ID:" + checkId);
                Console.WriteLine("------------------------------------------------------------------------");
                foreach (Server server in ReadServerListData())
                {
                    AddServersToDatabase(server);
                }
                System.Threading.Thread.Sleep(TimeSpan.FromMinutes(15));
            }
        }

        private static int GetLatestCheckId()
        {
            try
            {
                using (MySqlConnection connection = Database.GetConnectionString())
                {
                    connection.Open();

                    using (MySqlCommand SelectMaxCheckId =
                        new MySqlCommand("SELECT IFNULL(MAX(CheckId), 0) FROM ftbservers", connection))
                    {
                        return checkId = Convert.ToInt32(SelectMaxCheckId.ExecuteScalar());
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private static void AddServersToDatabase(Server server)
        {
            try
            {
                using (MySqlConnection connection = Database.GetConnectionString())
                {
                    connection.Open();

                    using (MySqlCommand InsertServer =
                        new MySqlCommand(
                            "INSERT INTO ftbservers (CheckId, Ip, Points, PlayerCount, Date) VALUES (@CheckId, @Ip, @Points, @Playercount, @Date)",
                            connection))
                    {
                        InsertServer.Parameters.AddWithValue("CheckId", checkId);
                        InsertServer.Parameters.AddWithValue("Ip", server.ip);
                        InsertServer.Parameters.AddWithValue("Points", server.points);
                        InsertServer.Parameters.AddWithValue("Playercount", server.playercount);
                        InsertServer.Parameters.AddWithValue("Date", server.date);
                        int numberOfRecords = InsertServer.ExecuteNonQuery();
                        Console.WriteLine(server.ip + " " + server.points + " " + server.playercount + " " +
                                          server.date + " " + numberOfRecords);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private static List<Server> ReadServerListData()
        {
            string Url = "https://ftbservers.com/";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);
            int maxServerCount = 21;
            List<Server> serverListData = new List<Server>();
            for (int serverCount = 1; serverCount < maxServerCount; serverCount++)
            {
                double pointsScore =
                    Convert.ToDouble(doc.DocumentNode.SelectNodes(
                            $"//*[@id=\"servers\"]/div/section[2]/article[{serverCount}]/ul/li[3]/span[2]/strong")[0]
                        .InnerText);
                double playerScore =
                    Convert.ToDouble(doc.DocumentNode.SelectNodes(
                            $"//*[@id=\"servers\"]/div/section[2]/article[{serverCount}]/ul/li[2]/span[2]/strong")[0]
                        .InnerText);
                string serverIp =
                    doc.DocumentNode.SelectNodes(
                        $"//*[@id=\"servers\"]/div/section[2]/article[{serverCount}]/ul/li[1]/span[2]")[0].InnerText;
                Server server = new Server(serverIp, pointsScore, playerScore, DateTime.Now.ToString("G"));
                serverListData.Add(server);
            }

            return serverListData;
        }
    }
}