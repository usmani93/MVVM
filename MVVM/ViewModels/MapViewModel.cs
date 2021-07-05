using MVVM.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MVVM.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        #region Properties

        private string address;
        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        public Command GetLocationsNearBy { get; }

        #endregion

        #region Methods

        public MapViewModel()
        {
            GetLocationsNearBy = new Command(async () => await GetLocationsNearByAsync());
        }

        public async Task GetLocationsNearByAsync()
        {
            try
            {
                var route = $"{nameof(LocationsListPage)}?Address={Address}";
                await Shell.Current.GoToAsync(route);
            }
            catch(Exception e)
            {

            }
        }

        #endregion
    }
}
