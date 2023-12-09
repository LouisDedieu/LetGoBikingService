using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetGoBikingService.Models
{
    public class Itinary
    {
        public Metadata metadata {  get; set; }
        public List<Feature> Features { get; set; }
    }
    public class Metadata
    {
        public string uuid { get; set; }
        public Query query { get; set; }
    }

    public class Query
    {
        public string profile { get; set; }
    }

    public class Feature
    {
        public Properties Properties { get; set; }
        public Geometry geometry { get; set; }


    }

    public class Geometry
    {
        public List<List<double>> coordinates { get; set; }
    }

    public class Properties
    {
        public List<Segment> Segments { get; set; }
        public Summary Summary { get; set; }
    }

    public class Segment
    {
        public double Distance { get; set; }
        public double Duration { get; set; }
        public List<Step> Steps { get; set; }
    }

    public class Step
    {
        public double Distance { get; set; }
        public double Duration { get; set; }
        public string Instruction { get; set; }
        public string Name { get; set; }
        public List<int> way_points { get; set; }

        // Les champs Latitude et Longitude ne sont pas dans le JSON mais sont ajoutés séparément
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double EndLatitude { get; set; }
        public double EndLongitude { get; set; }
    }

    public class Summary
    {
        public double Distance { get; set; }
        public double Duration { get; set; }
    }


}