
namespace Services
{
    public class ConnectivityCheckerService
    {
        public HttpClient HttpClient { get; set; }
        public IConnectivity Connectivity { get; set; }

        public ConnectivityCheckerService(IConnectivity connectivity)
        {
            HttpClient = new HttpClient();

            this.Connectivity = connectivity;

        }

        public NetworkAccess CheckNetworkEnabled()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) return NetworkAccess.None;
            else return NetworkAccess.Internet;
                
        }

        


    }
}
