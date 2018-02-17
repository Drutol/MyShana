namespace MyShana.BusinessLogic

open MyShana.Models
open MyShana.Interfaces
open System.Net
open HtmlAgilityPack
open System
open System.Net.Http
open System.Collections.Generic

module ShanaCommunicator =

    let mutable private httpClient : HttpClient = null

    let AyncGetApiHttpContext logInfo logError username password =
        async {

            try
                if httpClient = null 
                then
                    httpClient <- new HttpClient(new HttpClientHandler(UseCookies = true, 
                                                                       AllowAutoRedirect = true),
                                                     BaseAddress=new Uri("http://www.shanaproject.com/"))
                    let! response = httpClient.GetAsync("/login") |> Async.AwaitTask
                    let! html = response.Content.ReadAsStringAsync() |> Async.AwaitTask

                    let doc = new HtmlDocument()
                    doc.LoadHtml(html)

                    let csrfToken = doc.DocumentNode.Descendants("input") 
                                    |> Seq.filter (fun node -> 
                                        node.Attributes.Contains("name") && 
                                        node.Attributes.["name"].Value = "csrfmiddlewaretoken")
                                    |> Seq.head
                                    |> fun node -> node.Attributes.["value"].Value

                    let contents = dict["csrfmiddlewaretoken", csrfToken;
                                        "next", "";
                                        "username", username;
                                        "password", password]
            
                    let! response = httpClient.PostAsync("/login", new FormUrlEncodedContent(contents)) 
                                    |> Async.AwaitTask

                    match response.IsSuccessStatusCode with
                    | true -> logInfo("Successfuly logged in.")
                    | false -> logError("Login fail.")
                               httpClient <- null
                               raise (Exception("Login fail"))
            with 
            | exn ->
                logError("Login exception")
                httpClient <- null
                raise (exn)

            return httpClient                          
        }

        

    let AsyncGetRecentEntries
        logInfo
        logError
        (page:int) : Async<Option<seq<SeriesEntry>>> =
            async {

                try
                    let! httpClient = AyncGetApiHttpContext logInfo logError "" ""
                    let uri = new Uri(String.Format("http://www.shanaproject.com/anime/{0}",page))
                    let! html = httpClient.GetStringAsync(uri) |> Async.AwaitTask

                    let doc = new HtmlDocument()
                    doc.LoadHtml(html)

                    let descendantsWithClass (htmlElement:string) (cssClass:string) (node:HtmlNode) =
                        node.Descendants(htmlElement) 
                        |> Seq.filter (fun node -> 
                            node.Attributes.Contains("class") && 
                            node.Attributes.["class"].Value = cssClass)
                    

                    return descendantsWithClass "div" "release_block" doc.DocumentNode
                        |> Seq.filter ( fun node -> node.Attributes.Contains("id") )
                        |> Seq.map ( fun node -> {  Id = node.Attributes.["id"].Value.Replace("rel","") 
                                                            |> int; 
                                                    Title = descendantsWithClass "div" "release_text_contents" node
                                                            |> Seq.head
                                                            |> fun n -> n.Descendants() 
                                                            |> Seq.head
                                                            |> fun n -> n.InnerText;
                                                    Tags = descendantsWithClass "div" "release_profile" node 
                                                            |> Seq.map ( fun n -> n.InnerText )
                                                            |> List.ofSeq;
                                                    Subber = descendantsWithClass "div" "release_subber" node 
                                                            |> fun n -> match Seq.tryHead n with
                                                                        | Some x -> x.InnerText
                                                                        | None -> "No Subber"})
                        |> fun items ->
                            logInfo(String.Format("Loaded {0} items.",[items |> Seq.length]))
                            items
                        |> Some
                with
                | exn -> 
                    logError(exn.ToString())
                    return None              
            }
        


        

        

        







