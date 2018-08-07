﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL_CV_Fiches.Models.Graph;
using DAL_CV_Fiches.Repositories.Graph;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebCV_Fiches.Helpers;
using WebCV_Fiches.Models.CVViewModels;

namespace WebCV_Fiches.Controllers
{
    [Route("api/Bio")]
    public class CVBioController : Controller
    {
        public EditionObjectGraphRepository editionObjectGraphRepository;
        public UtilisateurGraphRepository utilisateurGraphRepository;
        public FonctionGraphRepository fonctionGraphRepository;

        public CVBioController()
        {
            utilisateurGraphRepository = new UtilisateurGraphRepository();
            editionObjectGraphRepository = new EditionObjectGraphRepository();
            fonctionGraphRepository = new FonctionGraphRepository();
        }

        [Route("Detail/{utilisateurId}")]
	   // [Authorize("Bearer")]
        public ActionResult Detail(string utilisateurId)
        {
            var bioViewModel = new BioViewModel();

            var utilisateur = utilisateurGraphRepository.GetOne(utilisateurId);

            bioViewModel.Nom = utilisateur.Nom;
            bioViewModel.Prenom = utilisateur.Prenom;

            var cv = utilisateur?.Conseiller?.CVs?.First();

            if (cv == null)
                return Json(new { });

            bioViewModel.ResumeExperience = cv.ResumeExperience;

            var conseiller = utilisateur.Conseiller;
            bioViewModel.Fonction = conseiller.Fonction.GraphKey;

            var editions = new EditionObjectViewModelFactory<BioViewModel>();
            bioViewModel.editionObjecViewModels = editions.GetEditions(utilisateur, cv, conseiller);

            var returnJon = new
            {
            };
            
            return Json(bioViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("{utilisateurId}/Edit")]
        public ActionResult Edit(string utilisateurId, [FromBody]BioViewModel bio)
        {
            var utilisateur = utilisateurGraphRepository.GetOne(utilisateurId);
            var proprietesModifiees = new List<KeyValuePair<string, string>>();

            if (utilisateur.Prenom != bio.Prenom)
                proprietesModifiees.Add(new KeyValuePair<string, string>("Prenom", bio.Prenom));

            if (utilisateur.Nom != bio.Nom)
                proprietesModifiees.Add(new KeyValuePair<string, string>("Nom", bio.Nom));

            if (proprietesModifiees.Count() > 0)
                editionObjectGraphRepository.CreateOrUpdateProprieteEdition(proprietesModifiees, utilisateur);

            var cv = utilisateur.Conseiller.CVs.First();
            if (cv.ResumeExperience != bio.ResumeExperience)
            {
                proprietesModifiees.Clear();
                proprietesModifiees.Add(new KeyValuePair<string, string>("ResumeExperience", bio.ResumeExperience));
                editionObjectGraphRepository.CreateOrUpdateProprieteEdition(proprietesModifiees, cv);
            }

            var conseiller = utilisateur.Conseiller;
            if (conseiller.Fonction.GraphKey != bio.Fonction)
            {
                var newFonction = fonctionGraphRepository.GetOne(bio.Fonction);
                editionObjectGraphRepository.ChangerNoeud(
                    objetAjoute: newFonction,
                    objetsupprimeGraphKey: conseiller.Fonction.GraphKey,
                    noeudModifiePropriete: "Fonction",
                    noeudModifie: conseiller);
            }
            else
            {
                var edition = utilisateur.Conseiller.EditionObjects.Find(x => x.ViewModelProprieteNom == "Fonction");
                editionObjectGraphRepository.Delete(edition);
            }

            return Json(new { Status = "OK", Message = "ResumeExperience modifiée" });
        }
    }
}
