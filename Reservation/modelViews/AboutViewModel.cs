
namespace Reservations.modelViews
{
    public partial class AboutViewModel : BaseViewModel
    {
        [ObservableProperty]
        public partial string Description {  get; set; }
        [ObservableProperty]
        public partial string ApplicationVersion {  get; set; } 
        [ObservableProperty]
        public partial string PhoneModel {  get; set; } 
        [ObservableProperty]
        public partial string AndroidVersion {  get; set; } 
        public AboutViewModel()
        {
            Title = "O aplikacji";
            Description = "Nazywam się Dominik Pawłowski. Mam 24 lata. Jestem studentem Państwowej Akademi Nauk Stosowanych w Włocławku na kierunku Informatyka. Sama aplikacja jest tworzona jako praca Inżynierska z zakresu bazy danych o tematyce aplikacja / system do rezerwacji stolików w restauracjach z modułem lojalnościowym dla klienta";
            ApplicationVersion = Microsoft.Maui.ApplicationModel.AppInfo.VersionString;
            PhoneModel =  DeviceInfo.Manufacturer + " " + DeviceInfo.Current.Model;
            AndroidVersion = DeviceInfo.Current.Platform + " " + DeviceInfo.Current.VersionString;
        }
    }
}
