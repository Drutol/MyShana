namespace MyShana.Models

open Newtonsoft.Json


type SeriesEntry = {
    Id:int
    Title:string
    Subber:string 
    Tags:List<string>
}

type AnimeDetails = {
     Id:int
     Url:string 
     [<JsonProperty("image_url")>] 
     ImageUrl:string 
     Title:string  
     Description:string 
     [<JsonProperty("type")>]
     EntryType:string  
     Score:double  
     Episodes:int  
     Members:int  
}
