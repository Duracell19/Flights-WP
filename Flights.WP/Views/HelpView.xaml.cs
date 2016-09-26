using MvvmCross.WindowsCommon.Views;
using Windows.Phone.UI.Input;

namespace Flights.WP.Views
{
    public sealed partial class HelpView : MvxWindowsPage
    {
        public HelpView()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }
    }
}
