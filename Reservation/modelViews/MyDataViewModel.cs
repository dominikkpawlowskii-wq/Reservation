

namespace Reservations.modelViews
{
    public partial class MyDataViewModel : BaseViewModel
    {
        public ILoginService UserService { get; set; }
        private readonly IPopupService _popupService;
        public MyDataViewModel(IPopupService popupService, ILoginService userService)
        {
            _popupService = popupService;
            _ = DisplayPopup();
            UserService = userService;
        }

        public async Task DisplayPopup()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await Task.Delay(3000);
                _popupService.ShowPopup<PopupViewModel>(shell: Shell.Current);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }

            
        }
    }
}
