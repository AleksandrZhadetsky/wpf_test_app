using System;

namespace ExchangeConnectionsViewer
{
    public class ExchangeConnection
    {
        public string Status { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public DateTime LastConnect { get; set; }
        public string Id { get; set; }

        public ExchangeConnection()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ExchangeConnection(DateTime lastConnected) : this()
        {
            LastConnect = lastConnected;
        }
    }
} 