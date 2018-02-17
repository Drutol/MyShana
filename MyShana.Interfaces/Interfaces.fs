namespace MyShana.Interfaces

open System.Collections.Generic
open MyShana.Models
open System.Threading.Tasks
open System.Net.Http

//Shana
type AyncGetApiHttpContext = Username * Password -> Async<HttpClient>
type AsyncGetRecentEntries = unit -> Async<Option<SeriesEntry>>

//Jikan
type AsyncGetAnimeInfo = string -> Async<Option<AnimeDetails>>

//Logger
type LogInfo = string -> unit
type LogError = string -> unit