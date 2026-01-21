
namespace Reservations.modelViews
{
    public partial class SettingViewModel : BaseViewModel
    {
        [ObservableProperty]
        public partial bool IsToggled { get; set; }
        public SettingViewModel()
        {
            Title = "Ustawienia";
            IsToggled = Preferences.Get(nameof(IsToggled), false);
        }


        partial void OnIsToggledChanged(bool value)
        {
            Preferences.Set(nameof(IsToggled), value);

            ApplyTheme();
        }

        public void ApplyTheme()
        {
            ICollection<ResourceDictionary> mergedDictioneries = Application.Current!.Resources.MergedDictionaries;
            if (mergedDictioneries != null)
            {
                var existingTheme = mergedDictioneries.FirstOrDefault(d => d is darkTheme || d is lightTheme);

                if (existingTheme != null)
                {
                    mergedDictioneries.Remove(existingTheme);
                }
                mergedDictioneries.Add(IsToggled ? new darkTheme() : new lightTheme());
                
            }
        }
        
    }
}
