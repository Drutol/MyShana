namespace MyShana.BusinessLogic

open MyShana.Models
open MyShana.Interfaces
open System.Net
open HtmlAgilityPack
open System
open System.Net.Http
open System.Collections.Generic

type ApiCommunicator() =

    let httpClient = new HttpClient()

    interface IApiCommunicator with 
        member this.GetRecentEntriesAsync() =

            async {
                let uri = new Uri("http://www.shanaproject.com/")
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

            } |> Async.StartAsTask


        

        

        







