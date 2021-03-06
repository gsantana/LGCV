﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CV_Fiches.Models.Graph
{
    [Serializable]
    public class Association : GraphObject
    {
        public string Description { get; set; }

        public Association()
        { }

        public static Association CreateAssociation(string association)
        {
            Association assoc = new Association();
            assoc.Description = association;

            return assoc;
        }
    }
}
