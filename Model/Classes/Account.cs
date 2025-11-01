using CommunityToolkit.Mvvm.ComponentModel;

namespace Model.Classes
{
    public partial class Account : ObservableObject
    {
        [ObservableProperty]
        public partial int Id { get; set; }
        [ObservableProperty]
        public partial string? FirstName { get; set; }
        [ObservableProperty]
        public partial string? LastName { get; set; }
        [ObservableProperty]
        public partial string? Telephone { get; set; }
        [ObservableProperty]
        public partial string? Email { get; set; }
        [ObservableProperty]
        public partial string? Password { get; set; }
        [ObservableProperty]
        public partial int? NumberOfPoints { get; set; }
    }
}
