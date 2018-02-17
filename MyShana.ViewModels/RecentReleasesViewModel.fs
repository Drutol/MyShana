namespace MyShana.ViewModels

open GalaSoft.MvvmLight
open MyShana.Models
open System.Collections.ObjectModel
open System.Threading

type RecentReleasesViewModel() =
    inherit ViewModelBase()

    let mutable items : ObservableCollection<SeriesEntry> = null

    member x.Items 
        with get() = items
        and set(value) =
            items <- value
            x.RaisePropertyChanged()

    member x.NavigatedTo() =
        async {
            let! result = ShanaComposition.asyncGetRecentEntries 1
            match result with
                | Some entries -> x.Items <- new ObservableCollection<SeriesEntry>(entries) 
        } |> Async.StartImmediate

      
