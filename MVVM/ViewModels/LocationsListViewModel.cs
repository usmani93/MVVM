using MVVM.Helpers;
using MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace MVVM.ViewModels
{
    [QueryProperty(nameof(Address), nameof(Address))]
    public class LocationsListViewModel : BaseViewModel
    {
        #region Properties

        private string address;
        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        public ObservableCollection<Locations> ListOfLocations { get; }

        #endregion

        #region Methods

        public LocationsListViewModel()
        {
            ListOfLocations = new ObservableCollection<Locations>();

            GetLocations(Address);
        }

        private void GetLocations(string address)
        {
            try
            {
                ListOfLocations.Add(new Locations
                {
                    Name = "a",
                    Location = "a"
                });
            }
            catch (Exception e)
            {
                Utils.LogCrashlytics(e);
            }
        }

        #endregion
    }
}
