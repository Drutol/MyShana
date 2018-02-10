using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using MyShana.BusinessLogic;
using MyShana.Interfaces;
using MyShana.Models;

namespace MyShana.ViewModels
{
    public class RecentReleasesPageViewModel : ViewModelBase
    {
        private ObservableCollection<SeriesEntry> _entries;

        public async void NavigatedTo()
        {
            var communicator = new ApiCommunicator() as IApiCommunicator;
            var jcommunicator = new JikanCommunicator() as IJikanCommunicator;

            //Entries = new ObservableCollection<SeriesEntry>(await communicator.GetRecentEntriesAsync());

            var details = await jcommunicator.GetAnimeInfoAsync("Darling in the FRANXX");
            
        }

        public ObservableCollection<SeriesEntry> Entries
        {
            get { return _entries; }
            set
            {
                _entries = value;
                RaisePropertyChanged();
            }
        }
    }
}
