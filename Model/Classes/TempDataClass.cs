using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Model.Classes
{
    public partial class TempDataClass : ObservableObject
    {
        [ObservableProperty]
        public partial string Text {  get; set; }
        [ObservableProperty]
        public partial bool IsCheck {  get; set; }
        [ObservableProperty]
        public partial ImageSource? ImageSource {  get; set; }
    }
}
