using System;
using UCRSuite;
using Fleck; 
using Newtonsoft.Json.Linq;
using System.Diagnostics;

class Wrapper
{
    static void Main()
    {
        var server = new WebSocketServer("ws://127.0.0.1:8181");

        server.Start(socket =>
        {
            socket.OnOpen = () => Console.WriteLine("Open!");
            socket.OnClose = () => Console.WriteLine("Close!");

            socket.OnMessage = message =>
            {
                JObject j = JObject.Parse(message);
                int k = j.GetValue("k").ToObject<int>(); 
                JArray searchseries = (JArray) j["searchseries"];
                int dimensions = searchseries[0].ToObject<double[]>().Length;

                var sw = Stopwatch.StartNew();
                DTW ucrDtw = new DTW(dimensions,k);
                foreach (JArray datapoint in searchseries)
                {
                    ucrDtw.addDataItem(datapoint.ToObject<double[]>());

                }
                JArray snippets = (JArray)j["querysnippets"];
                DTWResult localresults; 

                JArray matches = new JArray();
                JArray distances = new JArray();
                JArray queryids = new JArray();
                int quid = 0; 

                foreach (JArray snippet in snippets)
                {
                    Query query = ucrDtw.Query();
                    foreach (JArray datapoint in snippet)
                    {
                        query.addQueryItem(datapoint.ToObject<double[]>());
                    }

                    localresults = ucrDtw.warp(query);
                    matches.Add(localresults.Locations);
                    distances.Add(localresults.Distances); 
                    foreach(int thatloc in localresults.Locations)
                    {
                        queryids.Add(quid); 
                    }
                    Console.WriteLine("Query " + quid + " finished."); 
                    quid = quid + 1; 
                }
                sw.Stop(); 

                JObject response = new JObject();
                response.Add("querytime", Math.Round(sw.Elapsed.TotalMilliseconds) / 1000.0); 
                response.Add("matchlocations", matches);
                response.Add("queryids", queryids); 
                response.Add("distances", distances);
                socket.Send(response.ToString());
            };
        });
        Console.WriteLine("Waiting for requests...");
        var name = Console.ReadLine();
    }
}

