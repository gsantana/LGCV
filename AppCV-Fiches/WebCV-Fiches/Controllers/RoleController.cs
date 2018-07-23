﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebCV_Fiches.Data;
using WebCV_Fiches.Models;
using WebCV_Fiches.Models.Admin;
using WebCV_Fiches.Models.AdminViewModels;

namespace WebCV_Fiches.Controllers
{

    //    [Authorize(Roles = "Administrateur")]
    //[Route("[controller]/[action]")]
    [Route("Role")]
    public class RoleController : Controller
    {
        private AdminViewModel adminViewModel;
        private RoleAdministrationViewModel roleAdministrationViewModel;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;

        public RoleController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // GET: Admin
        public ActionResult Index()
        {
            adminViewModel = new AdminViewModel();
            adminViewModel.Roles = roleManager.Roles.ToList();

            return View(adminViewModel);
        }

        // POST: Role/Create
        [HttpPost]
        [AllowAnonymous]
        [Route("Create")]
        public ActionResult Create([FromBody]ApplicationRole role)
        {
            try
            {
                role.ConcurrencyStamp = Guid.NewGuid().ToString();
                var result = roleManager.CreateAsync(role).Result;
                
                return Json(role);
            }
            catch
            {
                ErrorViewModel error = new ErrorViewModel();
                error.RequestId = "Le rôle n'a pas pu être créé";
                return Json(error);
            }
        }

        [AllowAnonymous]
        [Route("GetAll")]
        public ActionResult GetAll()
        {
            try
            {
                var result = roleManager.Roles.ToList();
                return Json(result);
            }
            catch
            {
                ErrorViewModel error = new ErrorViewModel();
                error.RequestId = "Une erreur s'est produite lors de la lecture de la liste des rôles";
                return Json(error);
            }
        }

        [AllowAnonymous]
        [Route("GetAllUsers")]
        public ActionResult getAllUsers()
        {
            try
            {
                var result = userManager.Users.Select(user => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { userName = user.UserName, id = user.Id }).AsEnumerable();
                return Json(result);
            }
            catch
            {
                ErrorViewModel error = new ErrorViewModel();
                error.RequestId = "Une erreur s'est produite lors de la lecture de la liste des rôles";
                return Json(error);
            }
        }

        public ActionResult Delete(string id)
        {
            try
            {
                roleManager.DeleteAsync(roleManager.FindByIdAsync(id).Result);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [AllowAnonymous]
        [Route("Details/{roleId}")]
        public ActionResult Details(string roleId)
        {
            roleAdministrationViewModel = new RoleAdministrationViewModel();
            roleAdministrationViewModel.Role = roleManager.FindByIdAsync(roleId).Result;
            roleAdministrationViewModel.Users = userManager.GetUsersInRoleAsync(roleAdministrationViewModel.Role.Name).Result.ToList();


            return Json(roleAdministrationViewModel);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(string id)
        {
            roleAdministrationViewModel = new RoleAdministrationViewModel();
            roleAdministrationViewModel.Role = roleManager.FindByIdAsync(id).Result;
            roleAdministrationViewModel.Users = userManager.GetUsersInRoleAsync(roleAdministrationViewModel.Role.Name).Result.ToList();

            ViewData["Utilisateurs"] = userManager.Users.Select(user => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = user.UserName, Value = user.Id }).AsEnumerable();
            return View(roleAdministrationViewModel);
        }

        [HttpPost]
        [Route("Edit")]
        [AllowAnonymous]
        public async Task<ActionResult> Edit([FromBody]RoleAdministrationViewModel roleAdministration)
        {
            try
            {
                ApplicationRole role = await roleManager.FindByIdAsync(roleAdministration.Role.Id);
                role.Name = roleAdministration.Role.Name;
                role.NormalizedName = role.Name.ToUpper();

                await roleManager.UpdateAsync(role);

                return Json(role);
            }
            catch
            {
                ErrorViewModel error = new ErrorViewModel();
                error.RequestId = "Le rôle n'a pas pu être modifié";
                return Json(error);
            }
        }

        [HttpPost]
        [Route("Admin/AddUserRole")]
        public async Task<ActionResult> AddUserRole(IFormCollection collection)
        {
            try
            {
                ApplicationUser user = userManager.FindByIdAsync(collection["Utilisateurs"]).Result;
                ApplicationRole role = roleManager.FindByIdAsync(collection["roleId"]).Result;

                await userManager.AddToRoleAsync(user, role.Name);

                return RedirectToAction(nameof(Edit), "Admin", new { id = role.Id });
            }
            catch
            {
                return View();
            }
        }

        [Route("Admin/DeleteUserRole/{roleId}/User/{userId}")]
        public async Task<ActionResult> DeleteUserRole(string roleId, string userId)
        {
            try
            {
                ApplicationUser user = userManager.FindByIdAsync(userId).Result;
                ApplicationRole role = roleManager.FindByIdAsync(roleId).Result;

                await userManager.RemoveFromRoleAsync(user, role.Name);

                return RedirectToAction(nameof(Edit), "Admin", new { id = roleId });
            }
            catch
            {
                return View();
            }
        }
    }
}