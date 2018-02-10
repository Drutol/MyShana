namespace MyShana.Interfaces

open System.Collections.Generic
open MyShana.Models
open System.Threading.Tasks

type IApiCommunicator = 
    abstract member GetRecentEntriesAsync: unit -> Task<IEnumerable<SeriesEntry>>
    
type IJikanCommunicator = 
    abstract member AsyncGetAnimeInfo: string -> Async<Choice<AnimeDetails,exn>>
    abstract member GetAnimeInfoAsync: string -> Task<AnimeDetails>
