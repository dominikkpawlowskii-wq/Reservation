

using System.Diagnostics.CodeAnalysis;

namespace Reservations.modelViews
{
    public partial class ReservationViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        public partial TempClass? TempClass { get; set; }
        [ObservableProperty]
        public partial int Index {  get; set; }
        [ObservableProperty]
        public partial TimeSpan Time {  get; set; }
        [ObservableProperty]
        public partial DateTime Date {  get; set; }
        [ObservableProperty]
        public partial string? NumberPersons { get; set; }
        public List<string>? NumberOfPersons { get; set; }

        private readonly IReservationRepository _reservationRepozitory;

        public ReservationViewModel(IReservationRepository reservationRepozitory)
        {
            RefreshTime();
            Index = Preferences.Get("int", 0);
            Date = DateTime.Now;

            Title = "Rezerwacja";
            NumberOfPersons = [];
            AddNumberOfPersons();
            _reservationRepozitory = reservationRepozitory;
        }


        private void AddNumberOfPersons()
        {
            if (NumberOfPersons?.Count != 0)
                NumberOfPersons?.Clear();

            for (int i = 0; i < 10; i++)
            {
                if (i == 0)
                {
                    string temp = $"{i + 1} osoba";
                    NumberOfPersons?.Add(temp);
                }
                else if (i > 0 && i < 4)
                {
                    string temp = $"{i + 1} osoby";
                    NumberOfPersons?.Add(temp);
                }
                else
                {
                    string temp = $"{i + 1} osób";
                    NumberOfPersons?.Add(temp);
                }
            }
        }

        public void RefreshTime()
        {
            TimeOnly timeOnly = TimeOnly.FromDateTime(DateTime.Now);
            Time = timeOnly.ToTimeSpan();
        }
        [RelayCommand]
        private async Task GoToSummaryReservationPage()
        {
            if(TempClass != null)
            {
                TempClass.Time = TimeOnly.FromTimeSpan(Time);

                TempClass.Date = DateOnly.FromDateTime(Date);

                TempClass.NumberOfPerson = NumberPersons;

                string? substring = NumberPersons?.Substring(0, 1);

                TempClass.TableRestaurants = await _reservationRepozitory.GetTableRestaurantWithNumberOfPlaceArgument(Convert.ToInt16(substring), TempClass!.Restaurant!.Id);

                await Shell.Current.GoToAsync(nameof(SummaryReservationPage), true, new Dictionary<string, object>
                {
                    {nameof(TempClass), TempClass},
                });
            } 
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            TempClass = query[nameof(TempClass)] as TempClass;

            OnPropertyChanged(nameof(TempClass));
        }

    }
}
