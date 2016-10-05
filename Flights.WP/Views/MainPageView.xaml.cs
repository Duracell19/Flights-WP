using Flights.Infrastructure;
using Flights.Models;
using MvvmCross.Platform;
using MvvmCross.Plugins.File;
using MvvmCross.WindowsCommon.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Flights.WP.Views
{
    public sealed partial class MainPageView : MvxWindowsPage
    {
        private ObservableCollection<FavoriteModel> _favorite;

        public ObservableCollection<FavoriteModel> FavoriteList
        {
            get { return _favorite; }
            set { _favorite = value; }
        }

        public MainPageView()
        {
            this.InitializeComponent();
        }

        private void ShowMenuOfFlight(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void DeleteCommand(object sender, RoutedEventArgs e)
        {
            var item = (e.OriginalSource as FrameworkElement).DataContext;
            var _fileStore = Mvx.Resolve<IMvxFileStore>();
            FavoriteList = (ObservableCollection<FavoriteModel>)favoriteList.ItemsSource;
            FavoriteList.Remove((FavoriteModel)item);
            _fileStore.WriteFile(Defines.FAVORITE_LIST_FILE_NAME, JsonConvert.SerializeObject(FavoriteList));
        }
    }
}