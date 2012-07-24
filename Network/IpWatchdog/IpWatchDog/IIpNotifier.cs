namespace IpWatchDog
{
    interface IIpNotifier
    {
        void OnIpChanged(string oldIp, string newIp);
    }
}
