using MvvmCross.WindowsCommon.Views;
using Windows.UI.Xaml.Navigation;

namespace Flights.WP.Views
{
    public sealed partial class MainPageView : MvxWindowsPage
    {
        public MainPageView()
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