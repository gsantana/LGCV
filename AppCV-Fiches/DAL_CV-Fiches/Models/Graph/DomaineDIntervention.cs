﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CV_Fiches.Models.Graph
{
    [Serializable]
    public class DomaineDIntervention
    {
        public string Description { get; set; }

        private DomaineDIntervention()
        { }

        public static DomaineDIntervention CreateDomaineDIntervetion(string domaineDIntervention)
        {
            DomaineDIntervention dom = new DomaineDIntervention();
            dom.Description = domaineDIntervention;

            return dom;
        }
    }
}