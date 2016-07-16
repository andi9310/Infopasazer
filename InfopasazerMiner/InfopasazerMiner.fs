namespace InfopasazerMiner

open FSharp.Data
open System.Text
open System

type InfopasazerMiner() = 
    static member private baseUrl = "http://infopasazer.intercity.pl/"

    static member private GetHttpResponse url =
        url |> Http.RequestString |> (Encoding.GetEncoding "iso-8859-1").GetBytes |> Encoding.UTF8.GetString

    static member private GetHtmlDocument url =
        url |> InfopasazerMiner.GetHttpResponse |> HtmlDocument.Parse

    static member private GetInfopasazerDocument url =
        InfopasazerMiner.baseUrl + url |> InfopasazerMiner.GetHtmlDocument
    
    static member private GetStationsDocument stationNamePattern = 
        InfopasazerMiner.GetInfopasazerDocument("?p=stations&q=" + stationNamePattern)

    static member private GetStationDocument id = 
        InfopasazerMiner.GetInfopasazerDocument("?p=station&id=" + id)

    static member GetStations stationNamePattern =
        (InfopasazerMiner.GetStationsDocument stationNamePattern).Descendants ["td"]
            |> Seq.choose (fun x ->
                x.TryGetAttribute "onclick"
                |> Option.map (fun a -> { Station.Name = x.InnerText(); Station.Id = Int32.Parse(a.Value().Replace("window.location='?p=station&id=", "").Replace("'", ""))})
            )