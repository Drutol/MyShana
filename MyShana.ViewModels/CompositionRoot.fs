namespace MyShana.ViewModels
    
open MyShana.BusinessLogic

module CompositionRoot =
    open System.Diagnostics

    let logError (message:string) =
        Debug.WriteLine("[Error] " + message)
    
    let logInfo (message:string) =
        Debug.WriteLine("[Info] " + message)


module JikanComposition =

    let asyncGetAnimeInfo =
        JikanCommunicator.AsyncGetAnimeInfo

module ShanaComposition = 

    let asyncGetRecentEntries = 
        ShanaCommunicator.AsyncGetRecentEntries 
            CompositionRoot.logInfo 
            CompositionRoot.logError