using MvvmCross.WindowsCommon.Views;
using Windows.UI.Xaml.Navigation;

namespace Flights.WP.Views
{
    public sealed partial class FlightsInfoView : MvxWindowsPage
    {
        public FlightsInfoView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
    }
}