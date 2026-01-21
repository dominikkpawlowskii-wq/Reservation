
using System.Globalization;

namespace Reservations.modelViews
{
    public partial class PedometrViewModel : BaseViewModel
    {
        [ObservableProperty]
        public partial string? Date {  get; set; }
        [ObservableProperty]
        public partial ImageSource? ImageSource { get; set; } = ImageSource.FromFile("walk_icon.png");
        [ObservableProperty]
        public partial string? Text {  get; set; }
        [ObservableProperty]
        public partial bool IsSwitch {  get; set; }
        [ObservableProperty]
        public partial int NumberOfStep {  get; set; }

        private double lastMagnitude;
        private const double treshold = 0.8;

        public ILoginService UserService { get; set; }
        public PedometrViewModel(ILoginService userService)
        {
            Title = "Krokomierz";
            Date = DateTime.Now.ToString("ddd dd MMM", new CultureInfo("pl-PL"));
            Text = "start";
            IsSwitch = true;
            NumberOfStep = 0;
            UserService = userService;
        }
        [RelayCommand]
        private void StartPedometr()
        {
            if(IsSwitch)
            {
                ImageSource = ImageSource.FromFile("standing_icon.png");
                IsSwitch = false;
                Text = "stop";

                if(Accelerometer.IsSupported)
                {
                    if(!Accelerometer.IsMonitoring)
                    {
                        Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                        Accelerometer.Start(SensorSpeed.Game);
                    }
                }
            }
            else
            {
                ImageSource = ImageSource.FromFile("walk_icon.png");
                Text = "start";
                IsSwitch=true;
                Accelerometer.Stop();
                Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            }
        }

        private void Accelerometer_ReadingChanged(object? sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;

            double magnitude = Math.Sqrt(data.Acceleration.X * data.Acceleration.X +
                                         data.Acceleration.Y * data.Acceleration.Y +
                                         data.Acceleration.Z * data.Acceleration.Z);

            double delta = Math.Abs(magnitude - lastMagnitude);
            lastMagnitude = magnitude;

            if(delta > treshold)
            {
                NumberOfStep++;
            }
        }

        public void Switch()
        {
            IsSwitch = true;
            Text = "start";
            ImageSource = ImageSource.FromFile("walk_icon.png");
        }
    }
}
