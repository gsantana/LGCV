﻿using DAL_CV_Fiches.Repositories.Graph.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_CV_Fiches.Models.Graph
{
    public class Formation : GraphObject
    {
        public string Description { get; set; }
        public int AnAcquisition { get; set; }
        public static string Discriminator = "Formation";

        [Edge("DuType",lazyLoad: false)]
        public Genre Type { get; set; }

        public static Formation CreateFormation(int annee, string description, string formationType)
        {
            return new Formation()
            {
                AnAcquisition = annee,
                Description = description,
                Type = new Genre { Descriminator = Formation.Discriminator, Description = formationType }
            };
        }
    }

    public static class FormationType
    {
        public static string Perfectionnement = "Perfectionnement";
        public static string Publication = "Publication";
        public static string Conference = "Conference";
        public static string Certification = "Certification";
    }
}
