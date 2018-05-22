using System.Collections.Generic;

namespace BetterVoteListWebCrawler
{
    public class Server
    {
        public string ip { get; private set; }
        public double points { get; private set; }
        public double playercount { get; private set; }
        public string date { get; private set; }
        public List<Server> serverList = new List<Server>();

        public Server(string _ip, double _points, double _playercount, string _date)
        {
            ip = _ip;
            points = _points;
            playercount = _playercount;
            date = _date;
        }

        public Server()
        {

        }
    }
}