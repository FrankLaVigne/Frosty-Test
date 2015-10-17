
using System;
using Windows.Devices.Gpio;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Frosty_Test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int CLOCK_PIN = 5;
        private const int DATA_PIN = 6;

        private GpioPin _clockPin;
        private GpioPin _dataPin;

        private DispatcherTimer timer;


        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        private void Timer_Tick(object sender, object e)
        {
            StartMonitoring();
        }

        private void StartMonitoring()
        {

            this.txtOutput.Text = string.Empty;

            this.txtOutput.Text = DateTime.Now.Ticks.ToString() + "\n";

            var gpio = GpioController.GetDefault();

            //setPin = gpio.OpenPin(SET_PIN);
            _clockPin = gpio.OpenPin(CLOCK_PIN);
            _dataPin = gpio.OpenPin(DATA_PIN);

            //_clockPin.SetDriveMode(GpioPinDriveMode.Output);
            //_dataPin.SetDriveMode(GpioPinDriveMode.Input);

            //byte data[3];

            //// pulse the clock pin 24 times to read the data
            //for (byte j = 3; j--;)
            //{
            //    for (char i = 8; i--;)
            //    {
            //        digitalWrite(PD_SCK, HIGH);
            //        bitWrite(data[j], i, digitalRead(DOUT));
            //        digitalWrite(PD_SCK, LOW);
            //}

            //for (byte j = 3; j <= 0; j--)            {
            //    this.txtOutput.Text += "j " + j.ToString() + "\n";

            //}

            var c = 0;


            for (int j = 2; j >= 0; j--)
            {
                this.txtOutput.Text += "Byte:" + j + "\n";

                for (int i = 7; i >= 0; i--)
                {
                    _clockPin.Write(GpioPinValue.High);
                    var dataPinValue = _dataPin.Read();
                    _clockPin.Write(GpioPinValue.Low);

                    this.txtOutput.Text += dataPinValue + "\n";

                    c++;

                }
            }


            // Release the GPIO pins.
            if (_dataPin != null)
            {
                _dataPin.Dispose();
                _dataPin = null;
            }

            if (_clockPin != null)
            {
                _clockPin.Dispose();
                _clockPin = null;
            }


        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

            // Release the GPIO pins.
            if (_dataPin != null)
            {
                _dataPin.Dispose();
                _dataPin = null;
            }

            if (_clockPin != null)
            {
                _clockPin.Dispose();
                _clockPin = null;
            }


        }
    }
}
