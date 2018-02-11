namespace MyShana.BusinessLogic

open MyShana.Interfaces
open System.Net.Http
open System
open System.Net
open Newtonsoft.Json
open System.Collections.Generic
open MyShana.Models
open System.Threading.Tasks

module JikanCommunicator =

    let httpClient = new HttpClient(BaseAddress = new Uri("https://api.jikan.me/"));
                                                      
    let AsyncGetAnimeInfo name =
        async {
            let! json = httpClient.GetStringAsync(String.Format("search/anime/{0}/1",[WebUtility.UrlEncode(name)])) |> Async.AwaitTask

            let data = JsonConvert.DeserializeObject<JsonObjectWrapper>(json)

            return data.result |> Seq.head
        } |> Async.Catch


    let GetAnimeInfoAsync name =
        async {
            let! result = AsyncGetAnimeInfo(name)   
                
            return match result with
                    |  Choice1Of2 details -> details
                    |  Choice2Of2 exn -> raise (exn);
        } |> Async.StartAsTask
     

            





