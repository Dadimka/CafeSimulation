﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text.Json;
using CafeSimulation.Models.Classes;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;

// using ReactiveUI;

namespace CafeSimulation.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private Config _windowConfig = new Config();

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


    public static List<double> GraphData { get; set; } = new List<double>();
    public static List<int> GraphDataPeople { get; set; } = new List<int>();

    public ISeries[] Series { get; set; } =
    {
        new ColumnSeries<double>
        {
            Values = GraphData,
            Fill = new SolidColorPaint(SKColors.Blue)
        }
    };
    
    public ISeries[] SeriesPeople { get; set; } =
    {
        new ColumnSeries<int>
        {
            Values = GraphDataPeople,
            Fill = new SolidColorPaint(SKColors.Purple)
        }
    };


    public ReactiveCommand<Unit, Unit> GenerateChart { get; }


    public MainWindowViewModel()
    {
        LoadConfig();
        GenerateChart = ReactiveCommand.Create(UpdateChart);
    }

    private void UpdateChart()
    {
        GraphData.Clear();
        GraphDataPeople.Clear();
        Restaurant restaurant =
            new Restaurant(TablesQuantity, WaitersQuantity, ChefsQuantity, BeginWork, EndWork, PeakTime, StdDev,
                GroupQuantity, MaxPeople, CapacityTable);
        List<Dictionary<string, List<int>>> statistics = restaurant.Simulate();

        for (int hour = 0; hour < 24; hour++)
        {
            long total_sum = 0;
            int total_sum_people = 0;
            long n = 1;
            for (int min = 0; min < 60; min++)
            {
                int tick = hour * 60 + min;

                if (statistics[tick] != null )
                {
                    if (statistics[tick].ContainsKey("waiting_time"))
                    {
                        n += statistics[tick]["waiting_time"].Count();
                        total_sum += statistics[tick]["waiting_time"].Sum();
                    }

                    if (statistics[tick].ContainsKey("people_left"))
                    {
                        total_sum_people += statistics[tick]["people_left"][0];
                    }
                }
                
            }

            GraphData.Add((double)total_sum / n);
            GraphDataPeople.Add(total_sum_people);
        }

        SaveConfig();
        Series = new ISeries[]
            { new ColumnSeries<double> { Values = GraphData, Fill = new SolidColorPaint(SKColors.Blue) } };
        this.RaisePropertyChanged(nameof(GraphData));
        this.RaisePropertyChanged(nameof(Series));
        SeriesPeople = new ISeries[]
            { new ColumnSeries<int> { Values = GraphDataPeople, Fill = new SolidColorPaint(SKColors.Purple) } };
        this.RaisePropertyChanged(nameof(GraphDataPeople));
        this.RaisePropertyChanged(nameof(SeriesPeople));
    }

    private void SaveConfig()
    {
        Config config = new Config(TablesQuantity,
            WaitersQuantity,
            ChefsQuantity,
            BeginWork,
            EndWork,
            PeakTime,
            StdDev,
            GroupQuantity,
            MaxPeople,
            CapacityTable,
            GraphData,
            GraphDataPeople);

        config.Save();
    }

    private void LoadConfig()
    {
        _windowConfig.Load();
        TablesQuantity = _windowConfig.TablesQuantity;
        WaitersQuantity = _windowConfig.WaitersQuantity;
        ChefsQuantity = _windowConfig.ChefsQuantity;
        BeginWork = _windowConfig.BeginWork;
        EndWork = _windowConfig.EndWork;
        PeakTime = _windowConfig.PeakTime;
        StdDev = _windowConfig.StdDev;
        GroupQuantity = _windowConfig.GroupQuantity;
        MaxPeople = _windowConfig.MaxPeople;
        CapacityTable = _windowConfig.CapacityTable;
        GraphData = _windowConfig.GraphData;
        GraphDataPeople = _windowConfig.GraphDataPeople;
        Series = new ISeries[]
            { new ColumnSeries<double> { Values = GraphData, Fill = new SolidColorPaint(SKColors.Blue) } };
        SeriesPeople = new ISeries[]
            { new ColumnSeries<int> { Values = GraphDataPeople, Fill = new SolidColorPaint(SKColors.Purple) } };
        ;
    }
}