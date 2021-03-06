﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL_CV_Fiches.Models.Graph;
using DAL_CV_Fiches.Repositories.Graph;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCV_Fiches.Helpers;
using WebCV_Fiches.Models.CVViewModels;

namespace WebCV_Fiches.Controllers
{
    [Route("api/ResumeIntervention")]
    public class CVResumeInterventionsController : Controller
    {
        public UtilisateurGraphRepository utilisateurGraphRepository;
        public MandatGraphRepository mandatGraphRepository;

        public CVResumeInterventionsController()
        {
            utilisateurGraphRepository = new UtilisateurGraphRepository();
            mandatGraphRepository = new MandatGraphRepository();
        }

        [Route("{utilisateurId}/All")]
        [AllowAnonymous]
        public ActionResult All(string utilisateurId)
        {
             var utilisateur = utilisateurGraphRepository.GetOne(utilisateurId);
            var mandats = utilisateur.Conseiller?.Mandats?.Cast<GraphObject>().ToList();

         




            if (mandats == null)
                return Json(new List<ResumeInterventionViewModel>());

            var noeudModifie = new List<GraphObject>();
            noeudModifie.Add(utilisateur.Conseiller);
            noeudModifie.AddRange(mandats);
            var resumeInterventionViewModel = ViewModelFactory<Mandat, ResumeInterventionViewModel>.GetViewModels(
                utilisateurId: utilisateurId,
                noeudsModifie: noeudModifie,
                graphObjects: mandats,
                map: map);

            return Json(resumeInterventionViewModel);
        }

        private ViewModel map(GraphObject mandatModel)
        {
            try
            {
                var mandat = (Mandat)mandatModel;
                return new ResumeInterventionViewModel
                {
                    Annee = $"{mandat.DateDebut.Year}-{mandat.DateFin.Year}",
                    Client = mandat.Projet.Client.Nom,
                    Effors = mandat.Efforts,
                    Envergure = mandat.Projet.Envergure,
                    Fonction = mandat.Fonction.Description,
                    Nombre = mandat.Numero,
                    Projet = mandat.Titre,
                    GraphId = mandat.GraphKey
                };
            }
            catch (Exception)
            {

                return new ResumeInterventionViewModel();
            }
           
 
        }
    }
}