using CommunityToolkit.Maui.Core;

namespace Reservations.modelViews
{
    public partial class PaymentMethodViewModel : BaseViewModel
    {
        public Action<string>? OnItemChecked { get; set; }

        [ObservableProperty]
        public partial bool IsChecked {  get; set; }
        [ObservableProperty]
        public partial string? Text {  get; set; }
        [ObservableProperty]
        public partial ImageSource? ImageSource {  get; set; }

        private readonly IPopupService _popupService;
        public PaymentMethodViewModel(IPopupService popupService)
        {
            ImageSource = ImageSource.FromFile("blik_icon.png");

            Text = "BLIK";

            var selected = Preferences.Get("selectedPaymentMethod", string.Empty);

            IsChecked = Text == selected;

            this._popupService = popupService;
        }
        [RelayCommand]
        private async Task SwitchRadioButton()
        {
            IsChecked = true;
            Preferences.Set("selectedPaymentMethod", Text);

            await _popupService.ClosePopupAsync(Shell.Current);

        }

        partial void OnIsCheckedChanged(bool value)
        {
            if (value)
             OnItemChecked?.Invoke(Text!);
            
        }
    }
}
