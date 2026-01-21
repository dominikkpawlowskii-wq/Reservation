namespace Services
{
    public class ReservationHttpClient : IReservationHttpClient
    {
        public HttpClient? HttpClient { get; set; }

        public ReservationHttpClient()
        {
            ConfigureCertificate();
        }

        public async Task<TempDataAccount> Login(Authentication authentication)
        {
            var response = await HttpClient!.PostAsJsonAsync("https://10.0.2.2:7082/api/Authentication/login", authentication);

            if(response!.IsSuccessStatusCode)
            {
                TempDataAccount tempDataAccount = await response.Content.ReadFromJsonAsync<TempDataAccount>();

                return tempDataAccount!;
            }

            return null;
        }

        public async Task<bool> Register(Account account)
        {
            bool isSuccess;
            var response = await HttpClient!.PostAsJsonAsync("https://10.0.2.2:7082/api/Accounts/Register", account);

            if(response.IsSuccessStatusCode)
            {
                isSuccess = true;
                return isSuccess;
            }
            else
            {
                isSuccess = false;
                return isSuccess;
            }
        }
        public async Task<HttpResponseMessage> CreateReservation(Reservation reservation)
        {
            var response = await HttpClient!.PostAsJsonAsync("https://10.0.2.2:7082/api/Reservations/Reservations", reservation);

            
            return response.EnsureSuccessStatusCode();
        }

        public async Task<List<Restaurant>> GetAllRestaurants()
        {
            var response = await HttpClient!.GetAsync("https://10.0.2.2:7082/api/Restaurants/Restaurants");

            if (response.IsSuccessStatusCode)
            {
                var Restaurants = await response.Content.ReadFromJsonAsync<List<Restaurant>>();

                return Restaurants!;
            }

            return [];
        }

        public async Task<List<TempClass>> GetAccountReservations(Account account)
        {
            var response = await HttpClient!.PostAsJsonAsync("https://10.0.2.2:7082/api/Reservations/ReservationAccount", account);

            if(response.IsSuccessStatusCode)
            {
                var reservationList = await response.Content.ReadFromJsonAsync<List<TempClass>>();

                return reservationList!;
            }

            return [];
        }

        public async Task<HttpResponseMessage> UpdateAccountPoints(Account account)
        {
            var response = await HttpClient!.PatchAsJsonAsync("https://10.0.2.2:7082/api/Accounts/UpdateAccount", account);

            return response.EnsureSuccessStatusCode();
        }

        public async Task<List<RestaurantImage>> GetAllImageRestaurants()
        {
            var response = await HttpClient!.GetAsync("https://10.0.2.2:7082/api/RestaurantImages/RestaurantImages");

            if (response.IsSuccessStatusCode)
            {
                var Restaurants = await response.Content.ReadFromJsonAsync<List<RestaurantImage>>();

                return Restaurants!;
            }

            return [];
        }

        public async Task<ApiResponse<Order>> CreateOrder(Reservation reservation)
        {
            var response = await HttpClient!.PostAsJsonAsync("https://10.0.2.2:7082/api/Paypal/Order", reservation);

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<Order> result = await response.Content.ReadFromJsonAsync<ApiResponse<Order>>();

                return result!;
            }

            return null;
        }
        public async Task<ApiResponse<Order>> CaptureOrder(string orderId)
        {
                var response = await HttpClient!.PostAsJsonAsync("https://10.0.2.2:7082/api/Paypal/Capture", orderId);

                if (response.IsSuccessStatusCode)
                {
                    ApiResponse<Order> result = await response.Content.ReadFromJsonAsync<ApiResponse<Order>>();

                    return result!;
                }
                return null;
            
        }

        public async Task<List<RestaurantAddress>> GetAllAdressRestaurant()
        {
            var response = await HttpClient!.GetAsync("https://10.0.2.2:7082/api/AdressRestaurants/AdressRestaurants");

            if (response.IsSuccessStatusCode)
            {
                var Restaurants = await response.Content.ReadFromJsonAsync<List<RestaurantAddress>>();

                return Restaurants!;
            }

            return [];
        }

        public async Task<RestaurantTable> GetTableRestaurant(TempObject tempObject)
        {
            var response = await HttpClient!.PostAsJsonAsync("https://10.0.2.2:7082/api/TableRestaurants/TableRestaurants", tempObject);

            if(response.IsSuccessStatusCode)
            {
                RestaurantTable tableRestaurant = await response.Content.ReadFromJsonAsync<RestaurantTable>();

                return tableRestaurant;
            }
            return null;
        }
        public async Task<List<Account>> GetAllAccounts()
        {
            var response = await HttpClient!.GetAsync("https://10.0.2.2:7082/api/Accounts/GetAccounts");

            if (response.IsSuccessStatusCode)
            {
                List<Account> accounts = await response.Content.ReadFromJsonAsync<List<Account>>();

                return accounts!;
            }
            return null;
        }
        public void ConfigureCertificate()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    if (cert != null && cert.Issuer.Equals("CN=localhost"))
                        return true;
                    return errors == System.Net.Security.SslPolicyErrors.None;
                }
            };

            HttpClient = new HttpClient(handler);
        }
    }
}
