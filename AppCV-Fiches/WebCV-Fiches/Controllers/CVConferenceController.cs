﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL_CV_Fiches.Models.Graph;
using DAL_CV_Fiches.Repositories.Graph;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCV_Fiches.Helpers;
using WebCV_Fiches.Models.CVViewModels;

namespace WebCV_Fiches.Controllers
{
    [Route("api/Conference")]
    public class CVConferenceController : CVFormation
    {
        [AllowAnonymous]
        [Route("{utilisateurId}/All")]
        public new ActionResult All(string utilisateurId)
        {
            var temp = base.All(utilisateurId);
            var orderBy = temp.Cast<ConferenceViewModel>().ToList().Where(f =>
                                            f.GraphId != null
                                             ).OrderByDescending(x => x.Annee);
            return Json(orderBy);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("{utilisateurId}/Add")]
        public ActionResult Add(string utilisateurId, [FromBody]ConferenceViewModel conference)
        {
            return Json(base.Add(utilisateurId, conference));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("{utilisateurId}/Edit")]
        public ActionResult Edit(string utilisateurId, [FromBody]ConferenceViewModel conference)
        {
            return Json(base.Edit(utilisateurId, conference));
        }

        // POST: Mandat/Delete/5
        [HttpPost]
        [AllowAnonymous]
        [Route("{utilisateurId}/Delete/{conferenceId}")]
        public new ActionResult Delete(string utilisateurId, string conferenceId)
        {
            return Json(base.Delete(utilisateurId, conferenceId));
        }

        public override GraphObject CreateGraphObject(ViewModel viewModel)
        {
            var conference = (ConferenceViewModel)viewModel;
            var conferenceModel = Formation.CreateFormation(conference.Annee, conference.Description, FormationType.Conference);
            formationGraphRepository.Add(conferenceModel);
            return conferenceModel;
        }

        public override List<GraphObject> GetGraphObjects(Utilisateur utilisateur)
        {
            return utilisateur.Conseiller.Conference().Cast<GraphObject>().ToList(); ;
        }

        public override List<ViewModel> GetViewModels(string utilisateurId, List<GraphObject> noeudsModifie, List<GraphObject> graphObjects, Func<GraphObject, ViewModel> map)
        {
            return ViewModelFactory<Formation, ConferenceViewModel>.GetViewModels(
                utilisateurId: utilisateurId,
                noeudsModifie: noeudsModifie,
                graphObjects: graphObjects,
                map: map);
        }

        public override ViewModel Map(GraphObject graphObject)
        {
            var conference = (Formation)graphObject;
            return new ConferenceViewModel
            {
                Annee = conference.AnAcquisition,
                Description = conference.Description,
                GraphId = conference.GraphKey,
                GraphIdGenre = conference.Type.GraphKey,
            };
        }

        public override void UpdateGraphObject(GraphObject graphObject, ViewModel viewModel)
        {
            var conferenceObject = (Formation)graphObject;
            var conferenceViewModel = (ConferenceViewModel)viewModel;
            conferenceObject.AnAcquisition = conferenceViewModel.Annee;
            conferenceObject.Description = conferenceViewModel.Description;
            formationGraphRepository.Update(conferenceObject);
        }

        public override void VerifierProprietesModifiees(GraphObject graphObject, ViewModel viewModel)
        {
            var conferenceObject = (Formation)graphObject;
            var conferenceViewModel = (ConferenceViewModel)viewModel;

            editionObjectGraphRepository.ChangerPropriete(
                viewModelPropriete: () => conferenceViewModel.Annee,
                graphModelPropriete: () => conferenceObject.AnAcquisition,
                noeudModifie: graphObject);

            editionObjectGraphRepository.ChangerPropriete(
                viewModelPropriete: () => conferenceViewModel.Description,
                graphModelPropriete: () => conferenceObject.Description,
                noeudModifie: graphObject);
        }
    }
}