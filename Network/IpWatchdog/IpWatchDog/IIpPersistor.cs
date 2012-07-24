namespace IpWatchDog
{
    interface IIpPersistor : IIpRetriever
    {
        void SaveIp(string ip);
    }
}
