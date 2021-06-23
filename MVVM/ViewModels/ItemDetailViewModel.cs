using MVVM.Models;
using MVVM.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MVVM.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        #region Properties 

        private string itemId;
        private string text;
        private string description;
        public string Id { get; set; }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        #endregion

        #region Methods

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await ItemsService.GetItemById(itemId).Result;
                if (item != null)
                {
                    Id = item.Id.ToString();
                    Text = item.Text;
                    Description = item.Description;
                }
                else
                {
                    //no item found
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        #endregion
    }
}
