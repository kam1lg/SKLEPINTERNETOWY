using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SKLEP_INTERNETOWY.ViewModel;
using SKLEP_INTERNETOWY.Models;

namespace SKLEP_INTERNETOWY.Controllers
{
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index() => View(_roleManager.Roles.ToList());
        public IActionResult UserList() => View(_userManager.Users.ToList());

        public async Task<IActionResult> Edit(string userId)
        {
            // get the user
            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                //list of user roles
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();

                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // get the user 
            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                // list of user roles 
                var userRoles = await _userManager.GetRolesAsync(user);

                // get all roles
                var allRoles = _roleManager.Roles.ToList();

                // list of roles that have been added 
                var addedRoles = roles.Except(userRoles);

                // list of roles that have been removed
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }
            return NotFound();
        }
    }
}
