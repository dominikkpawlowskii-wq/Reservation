

using CommunityToolkit.Maui.Core;

namespace Reservations.modelViews
{
    public partial class PopupViewModel : BaseViewModel
    {
        [ObservableProperty]
        public partial IUserService UserService {  get; set; }

        private readonly IPopupService? _popupService;
        public PopupViewModel(IPopupService popupService, IUserService userService)
        {
            this._popupService = popupService;
            this.UserService = userService;
        }

        [RelayCommand]
        private async Task GoToSettingPage()
        {
            await _popupService!.ClosePopupAsync(Shell.Current);
            await Shell.Current.GoToAsync("..", true);
        }
    }
}
