namespace Reservations.modelViews
{
    public partial class PopupViewModel : BaseViewModel
    {
        public ILoginService UserService { get; set; }
        private readonly IPopupService? _popupService;
        public PopupViewModel(IPopupService popupService, ILoginService userService)
        {
            this._popupService = popupService;
            UserService = userService;
        }

        [RelayCommand]
        private async Task GoToSettingPage()
        {
            await _popupService!.ClosePopupAsync(Shell.Current);
            await Shell.Current.GoToAsync("..", true);
        }
    }
}
