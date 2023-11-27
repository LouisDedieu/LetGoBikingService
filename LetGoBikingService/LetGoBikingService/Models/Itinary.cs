using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetGoBikingService.Models
{
    public class Itinary
    {
        public List<Feature> Features { get; set; }
    }

    public class Feature
    {
        public Properties Properties { get; set; }
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
        public List<int> WayPoints { get; set; }
    }

    public class Summary
    {
        public double Distance { get; set; }
        public double Duration { get; set; }
    }

}