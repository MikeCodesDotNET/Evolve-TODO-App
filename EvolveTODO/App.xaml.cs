using EvolveTODO.Helpers;
using EvolveTODO.Services;
using Xamarin.Forms;

namespace EvolveTODO
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            bool useMock = true;

            if (useMock)
                ServiceLocator.Instance.Add<IService, MockService>();
            else
                ServiceLocator.Instance.Add<IService, AzureService>();

            MainPage = new NavigationPage(new Pages.ToDosPage())
            {
                BarBackgroundColor = (Color)Current.Resources["primaryBlue"],
                BarTextColor = Color.White
            };
        }
    }
}