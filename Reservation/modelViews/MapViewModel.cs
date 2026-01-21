using Mapsui.Limiting;
using Mapsui.Tiling;
using Mapsui.Utilities;

namespace Reservations.modelViews
{
    public partial class MapViewModel : BaseViewModel
    {
        public INavigationDataService NavigationDataService { get; set; }
        private readonly IGeocoding _geocoding;

        public ObservableCollection<Restaurant>? Restaurants { get; set; }
        public  ObservableCollection<RestaurantAddress>? Adresses { get; set; }
        public  ObservableCollection<RestaurantImage>? RestaurantImages { get; set; }

        private readonly Dictionary<string, Location> _locationsCache = [];
        private readonly Dictionary<string, Placemark> _placemarksCache = [];

        [ObservableProperty]
        public partial MapView MapView { get; set; }
        [ObservableProperty]
        public partial bool IsVisible {  get; set; }
        [ObservableProperty]
        public partial Restaurant? Restaurant {  get; set; }

        private TempClass? TempClass;
        public MapViewModel(IGeocoding geocoding, INavigationDataService navigationDataService)
        {
           
            this._geocoding = geocoding;
            this.NavigationDataService = navigationDataService;

            Reload();

            ConfigureMap();
        }

        private void MapView_MapClicked(object? sender, MapClickedEventArgs e)
        {
            IsActive = false;
        }

        private async void MapView_PinClicked(object? sender, PinClickedEventArgs e)
        {
            e.Handled = true;

            Location location = _locationsCache.FirstOrDefault(l => l.Value.Latitude == e.Pin.Position.Latitude && l.Value.Longitude == e.Pin.Position.Longitude).Value;

            if (location != null)
            {
                string locationKey = $"{location.Latitude},{location.Longitude}";

                if(!_placemarksCache.ContainsKey(locationKey))
                {
                    
                    var placemarks = await _geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);

                    Placemark placemark = placemarks.FirstOrDefault()!;
                    _placemarksCache.Add(locationKey, placemark);

                   
                    AssignRestaurantAndadresToVariables(placemark);
                }
                else
                {
                    string lKey = $"{location.Latitude},{location.Longitude}";
                    var placemark = _placemarksCache.SingleOrDefault(placemark => placemark.Key == lKey).Value;
                    AssignRestaurantAndadresToVariables(placemark);
                }

                

            }
            
            
        }

        private async Task<Location> GetCachedlocationAsync(string adress)
        {
            if(_locationsCache.ContainsKey(adress))
            {
                return _locationsCache[adress];
            }

            var locations = await _geocoding.GetLocationsAsync(adress);
            var location = locations.FirstOrDefault();
            if (location != null)
            {
                _locationsCache[adress] = location;
            }

            return location!;
        }
            
        public async Task AddPinOnMap()
        {
            if (MapView.Pins.Count != 0)
            {
                MapView.Pins.Clear();
            }

            var adressAndRestaurant = (from r in Restaurants
                                      join a in Adresses!
                                      on r.Id equals a.IdRestaurant
                                      select new TempClass
                                      {
                                          Restaurant = r,
                                          AdressRestaurant = a
                                      }).ToList();
            foreach(var temp in adressAndRestaurant)
            {
                string adresRestaurant = $"{temp.AdressRestaurant?.Street} {temp.AdressRestaurant?.ApartmentNumber} {temp.AdressRestaurant?.City}";
                Location location = await GetCachedlocationAsync(adresRestaurant);
                if (location != null)
                {
                    MapView.Pins.Add(new Pin
                    {
                        Label = temp.Restaurant?.Name,
                        Position = new Position(location.Latitude, location.Longitude)

                    });
                }
            }
        }
        [RelayCommand]
        private async Task GoToRestaurantDetailPage()
        {
            await Shell.Current.GoToAsync(nameof(RestaurantDetailPage), true, new Dictionary<string, object>
            {
                {"Restaurant",TempClass! }
            });
        }


        private void AssignRestaurantAndadresToVariables(Placemark placemark)
        {
            var temp = (from r in Restaurants
                        join a in Adresses! on r.Id equals a.IdRestaurant
                        join Ir in RestaurantImages! on r.Id equals Ir.Id 
                        where a.Street == placemark.Thoroughfare && a.ApartmentNumber == Convert.ToInt16(placemark.SubThoroughfare) && a.City == placemark.Locality
                        select new TempClass
                        {
                            Restaurant = r,
                            AdressRestaurant = a,
                            ImagesRestaurant = [..(from Images in RestaurantImages
                                                    where Images.IdRestaurant == r.Id
                                                    select Images)],
                        }).SingleOrDefault();

            TempClass = temp;

            Restaurant = temp!.Restaurant!;

            IsActive = true;
        }

        public void Reload()
        {
            /*(ObservableCollection<Restaurant> reservations, ObservableCollection<AdressRestaurant> aR, ObservableCollection<RestaurantImage> rI)
                = NavigationDataService.GetTupleCollections();
            Restaurants = reservations;
            Adresses = aR;
            RestaurantImages = rI;*/
        }

        public void ConfigureMap()
        {
            this.MapView = [];
            this.MapView.Map.Navigator.Limiter = new ViewportLimiterKeepWithinExtent();
            this.MapView.Map.Layers.Add(OpenStreetMap.CreateTileLayer($"myAgent"));
            this.MapView.Map.Layers.Remove(this.MapView.MyLocationLayer);
            this.MapView.RotationLock = true;
            this.MapView.IsNorthingButtonVisible = false;
            this.MapView.IsMyLocationButtonVisible = false;

            MapView.MapClicked += MapView_MapClicked;
            MapView.PinClicked += MapView_PinClicked;
        }
    }
}
