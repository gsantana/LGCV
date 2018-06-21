﻿using DAL_CV_Fiches.Repositories.Graph.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CV_Fiches.Models.Graph
{
    public class Client : GraphObject
    {
        public int Numero { get; set; }
        public string Nom { get; set; }
        public int Code { get; set; }
    }
}
