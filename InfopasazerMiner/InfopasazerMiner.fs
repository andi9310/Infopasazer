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

    static member private GetTrainDocument id = 
        InfopasazerMiner.GetInfopasazerDocument("?p=train&id=" + id)

    static member GetStations stationNamePattern =
        (InfopasazerMiner.GetStationsDocument stationNamePattern).Descendants ["td"]
            |> Seq.choose (fun x ->
                x.TryGetAttribute "onclick"
                |> Option.map (fun a -> { Station.Name = x.InnerText().Trim(); Station.Id = Int32.Parse(a.Value().Replace("window.location='?p=station&id=", "").Replace("'", ""))})
            )

    static member private TrainFromRow (row:HtmlNode) =
        let fields = row.Descendants ["td"]
        {
            Train.Name = fields |> (Seq.nth 0) |> fun (a: HtmlNode) -> a.InnerText().Replace(System.Environment.NewLine, ""); 
            Train.Company = fields |> (Seq.nth 1) |> fun (a: HtmlNode) -> a.InnerText(); 
            Train.Id = Int32.Parse((((fields |> Seq.nth 0).Descendants ["a"] |> Seq.nth 0).AttributeValue "href") |> fun (a: string) -> a.Replace("?p=train&id=", ""));
            Train.CompanyUrl = ((fields |> Seq.nth 1).Descendants ["a"] |> Seq.nth 0).AttributeValue "href";
            Train.Date = fields |> (Seq.nth 2) |> fun (a: HtmlNode) -> a.InnerText(); 
            Train.Source = fields |> (Seq.nth 3) |> fun (a: HtmlNode) -> a.InnerText().Split([|'-'|]) |> Seq.nth 0 |> fun (s: string) -> s.Trim(); 
            Train.Destination = fields |> (Seq.nth 3) |> fun (a: HtmlNode) -> a.InnerText().Split([|'-'|]) |> Seq.nth 1 |> fun (s: string) -> s.Trim(); 
            Train.PlannedTime = fields |> (Seq.nth 4) |> fun (a: HtmlNode) -> a.InnerText(); 
            Train.Delay = fields |> (Seq.nth 5) |> fun (a: HtmlNode) -> a.InnerText(); 
        }       

    static member private TrainsFromRows rows =
        try
            rows |> Seq.map(fun item -> InfopasazerMiner.TrainFromRow item)
        with
        | ArumentNullException -> Seq.empty
    
    static member private GetTrainsRows mode tables =
        try
            (tables |> Seq.find(fun (item:HtmlNode) -> not (item.Descendants ["th"] |> Seq.where(fun i -> i.InnerText().Contains(mode)) |> Seq.isEmpty))).Descendants ["tr"] |> Seq.skip 1
        with
        | KeyNotFoundException -> Seq.empty

    static member GetTrainsForStation stationId =
        let tables = InfopasazerMiner.GetStationDocument(stationId.ToString()).Descendants ["table"]
        let arrivals = tables |> InfopasazerMiner.GetTrainsRows "Przyjazd" 
        let departures = tables |> InfopasazerMiner.GetTrainsRows "Odjazd" 
        {
            TrainGroup.Arrivals = arrivals |> InfopasazerMiner.TrainsFromRows;
            TrainGroup.Departures = departures |> InfopasazerMiner.TrainsFromRows;
        } 

    static member private TrainStationFromRow (row:HtmlNode) =
        let fields = row.Descendants ["td"]
        {
            TrainStation.Name = fields |> (Seq.nth 3) |> fun (a: HtmlNode) -> a.InnerText().Trim()
            TrainStation.PlanedArrival = fields |> (Seq.nth 4) |> fun (a: HtmlNode) -> a.InnerText()
            TrainStation.ArrivalDelay = fields |> (Seq.nth 5) |> fun (a: HtmlNode) -> a.InnerText()
            TrainStation.PlannedDeparture = fields |> (Seq.nth 6) |> fun (a: HtmlNode) -> a.InnerText()
            TrainStation.DepartureDelay = fields |> (Seq.nth 7) |> fun (a: HtmlNode) -> a.InnerText()
        }    

    static member GetStationForName name =
        try
           (InfopasazerMiner.GetStationsDocument name).Descendants ["td"] 
                |> Seq.choose (fun x ->
                x.TryGetAttribute "onclick"
                |> Option.map (fun a -> { Station.Name = x.InnerText().Trim(); Station.Id = Int32.Parse(a.Value().Replace("window.location='?p=station&id=", "").Replace("'", ""))})
            ) |> Seq.find (fun a -> a.Name.Equals name)
        with
        | KeyNotFoundException -> {Station.Id = -1; Station.Name = null;}

    static member GetTrainDetails trainId =
        let rows = (InfopasazerMiner.GetTrainDocument(trainId.ToString())).Descendants ["tr"] |> Seq.skip 1
        rows |> Seq.map(fun item -> InfopasazerMiner.TrainStationFromRow item)

