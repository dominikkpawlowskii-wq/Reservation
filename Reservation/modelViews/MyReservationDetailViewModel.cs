

using System.Diagnostics.CodeAnalysis;

namespace Reservations.modelViews
{
    public partial class MyReservationDetailViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        public partial TempClass? TempClass {  get; set; }
        [ObservableProperty]
        public partial int Index {  get; set; }
        [ObservableProperty]
        public partial TimeSpan Time {  get; set; }
        [ObservableProperty]
        public partial DateTime Date {  get; set; }
        public List<string> NumberOfPersons { get; set; }

        private readonly IReservationRepository _reservationRepository;
        public MyReservationDetailViewModel(IReservationRepository reservationRepository)
        {
            Date = DateTime.Now;
            TimeOnly timeOnly = TimeOnly.FromDateTime(Date);
            Time = timeOnly.ToTimeSpan();
            Index = Preferences.Get("index", 0);
            Title = "Szczegóły rezerwacji";
            NumberOfPersons = [];
            AddNumberOfPersons();
            _reservationRepository = reservationRepository;
        }



        private void AddNumberOfPersons()
        {
            if (NumberOfPersons.Count != 0)
                NumberOfPersons.Clear();

            for (int i = 0; i < 10; i++)
            {
                if (i == 0)
                {
                    string temp = $"{i + 1} osoba";
                    NumberOfPersons.Add(temp);
                }
                else if (i > 0 && i < 4)
                {
                    string temp = $"{i + 1} osoby";
                    NumberOfPersons.Add(temp);
                }
                else
                {
                    string temp = $"{i + 1} osób";
                    NumberOfPersons.Add(temp);
                }
            }
        }
        [RelayCommand]
        private async Task RemoveReservation()
        {
            bool answer = await Shell.Current.DisplayAlert("Potwierdzenie", "Na pewno chcesz anulować rezerwacje?", "TAK", "NIE");
            if (!answer)
            {
                return;
            }
            
                await _reservationRepository.RemoveReservation(TempClass!.Account!, TempClass!.Reservation!);
                await Shell.Current.GoToAsync("..", true);

            
        }
        [RelayCommand]
        private async Task UpdateReservation()
        {
            bool answer = await Shell.Current.DisplayAlert("Potwierdzenie", "Na pewno chcesz zmienic godzine lub czas rezerwacji?", "TAK", "NIE");

            if(!answer)
            {
                return;
            }
            Index++;
            await _reservationRepository.UpdateAccountReservation(TempClass!, Date, Time, Index);
            await Shell.Current.GoToAsync("..", true);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            TempClass = query[nameof(TempClass)] as TempClass;
            OnPropertyChanged(nameof(TempClass));
        }
    }
}
