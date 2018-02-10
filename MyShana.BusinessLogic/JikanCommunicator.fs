namespace MyShana.BusinessLogic

open MyShana.Interfaces
open System.Net.Http
open System
open System.Net
open Newtonsoft.Json
open System.Collections.Generic
open MyShana.Models
open System.Threading.Tasks

type JikanCommunicator() =

    let httpClient = new HttpClient(BaseAddress = new Uri("https://api.jikan.me/"));
                            
                             
    interface IJikanCommunicator with
        member this.AsyncGetAnimeInfo name =
            async {
                let! json = httpClient.GetStringAsync(String.Format("search/anime/{0}/1",[WebUtility.UrlEncode(name)])) |> Async.AwaitTask

                let data = JsonConvert.DeserializeObject<JsonObjectWrapper>(json)

                return data.result |> Seq.head
            } |> Async.Catch


        member this.GetAnimeInfoAsync name =
            async {
                let! result = (this :> IJikanCommunicator).AsyncGetAnimeInfo(name)   
                
                return match result with
                       |  Choice1Of2 details -> details
                       |  Choice2Of2 exn -> raise (exn);
            } |> Async.StartAsTask
     

            





