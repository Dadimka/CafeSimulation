using System.Collections.Generic;
using System.IO;

namespace CafeSimulation.Models.Classes;

using System.Text.Json;

public class Config
{
    private string _configPath = "/Users/dadimka/RiderProjects/CafeSimulation/CafeSimulation/Assets/WindowConfig.json";
    public int TablesQuantity { get; set; } = 5;
    public int WaitersQuantity { get; set; } = 1;
    public int ChefsQuantity { get; set; } = 5;
    public int BeginWork { get; set; } = 8;
    public int EndWork { get; set; } = 24;
    public int PeakTime { get; set; } = 18;
    public int StdDev { get; set; } = 4;
    public int GroupQuantity { get; set; } = 20;
    public int MaxPeople { get; set; } = 6;
    public int CapacityTable { get; set; } = 6;

    public List<double> GraphData { get; set; } = [];


    public Config()
    {
        // Пустой конструктор
    }

    public Config(int tablesQuantity, int waitersQuantity, int chefsQuantity, int beginWork, int endWork, int peakTime,
        int stdDev, int groupQuantity, int maxPeople, int capacityTable, List<double> graphData)
    {
        TablesQuantity = tablesQuantity;
        ChefsQuantity = chefsQuantity;
        BeginWork = beginWork;
        EndWork = endWork;
        PeakTime = peakTime;
        StdDev = stdDev;
        GroupQuantity = groupQuantity;
        MaxPeople = maxPeople;
        CapacityTable = capacityTable;
        GraphData = graphData;
    }

    public void Save()
    {
        File.WriteAllText(_configPath, JsonSerializer.Serialize(this));
    }

    public void Load()
    {
        string str = File.ReadAllText(_configPath);
        var config = JsonSerializer.Deserialize<Config>(str);
        TablesQuantity = config.TablesQuantity;
        WaitersQuantity = config.WaitersQuantity;
        ChefsQuantity = config.ChefsQuantity;
        BeginWork = config.BeginWork;
        EndWork = config.EndWork;
        PeakTime = config.PeakTime;
        StdDev = config.StdDev;
        GroupQuantity = config.GroupQuantity;
        MaxPeople = config.MaxPeople;
        CapacityTable = config.CapacityTable;
        GraphData = config.GraphData;
    }
}