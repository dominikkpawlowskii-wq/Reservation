
namespace Reservations.modelViews
{
    public partial class AboutViewModel : BaseViewModel
    {
        [ObservableProperty]
        public partial string Description {  get; set; }
        [ObservableProperty]
        public partial string AplicationVerion {  get; set; } 
        [ObservableProperty]
        public partial string PhoneModel {  get; set; } 
        [ObservableProperty]
        public partial string AdroidVersion {  get; set; } 
        public AboutViewModel()
        {
            Title = "O aplikacji";
            Description = "Nazywam się Dominik Pawłowski. Mam 23 lata. Jestem studentem Państwowej Akademi Nauk Stosowanych w Włocławku na kierunku Informatyka. Sama aplikacja jest tworzona jako praca Inżynierska z zakresu bazy danych o tematyce aplikacja / system do rezerwacji stolików w restauracjach z modułem lojalnościowym dla klienta";
            AplicationVerion = Microsoft.Maui.ApplicationModel.AppInfo.VersionString;
            PhoneModel =  DeviceInfo.Manufacturer + " " + DeviceInfo.Current.Model;
            AdroidVersion = DeviceInfo.Current.Platform + " " + DeviceInfo.Current.VersionString;
        }
    }
}
