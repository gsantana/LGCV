﻿using DAL_CV_Fiches.Repositories.Graph.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_CV_Fiches.Models.Graph
{
    public class Instituition : GraphObject
    {
        public string Nom { get; set; }

        [Edge("LiesIn")]
        public Pays Pays { get; set; }
    }
}
