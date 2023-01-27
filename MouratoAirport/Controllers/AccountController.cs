using MouratoAirport.Helpers;
using MouratoAirport.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MouratoAirport.Data.Entities;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MouratoAirport.Data;

namespace MouratoAirport.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserHelper _userHelper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;
        private readonly DataContext _context;
        private readonly IImageHelper _imageHelper;

        public AccountController(UserManager<User> userManager, IUserHelper userHelper, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMailHelper mailHelper,DataContext context, IImageHelper imageHelper)
        {
            _userManager = userManager;
            _userHelper = userHelper;
            _roleManager = roleManager;
            _configuration = configuration;
            _mailHelper = mailHelper;
            _context = context;
            _imageHelper = imageHelper;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        [Authorize (Roles ="Admin")]
        public IActionResult Admin()
        {
            IndexAdminViewModel model = new IndexAdminViewModel
            {
                User = (from userroles in _context.UserRoles
                        join user in _context.Users
                         on userroles.UserId equals user.Id
                        join roles in _context.Roles
                        on userroles.RoleId equals roles.Id
                        select new
                        {
                            FullName = user.FirstName + " " + user.LastName,
                            UserId = user.Id,
                            Email = user.Email,
                            Role = roles.Name
                        }).Where(p => p.Role == "Employee" || p.Role == "Admin").ToList().Select(p => new UserWithRole
                        {
                            UserId = p.UserId,
                            Email = p.Email,
                            Role = p.Role,
                            FullName = p.FullName

                        })
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]

        public IActionResult AdminAdd()
        {
            return View();
        }





        public async Task<IActionResult> EditProfile()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new EditProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(string id, EditProfileViewModel model)
        {

            if(id == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var imageId = string.Empty;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                imageId = await _imageHelper.UploadImageAsync(model.ImageFile, "Airplane");
            }

            var user = await _userHelper.GetUserByIdAsync(id);
            user.FirstName= model.FirstName;
            user.LastName= model.LastName;
            user.ImageUrl = imageId;

            try
            {
                await _userHelper.UpdateUserAsync(user);
            }
            catch
            {

            }

            return RedirectToAction("Index","Home");

        }



        [HttpPost]
        public async Task<IActionResult> AdminAdd(AddEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Email,
                        Email = model.Email,
                        Password = model.Password
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    await _userHelper.AddUserToRoleAsync(user, "Employee");


                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "Error when creating account");
                        return View(model);
                    }

                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);
                    Response response = _mailHelper.SendEmail(user.Email, "Email confirmation", $"To confirm your email please click in this link :D <a href='{tokenLink}'>Link</a>");


                    if (response.IsSuccess)
                    {

                        return RedirectToAction("Admin");

                    }
                }
            }
            return RedirectToAction("Admin");

        }

        public async Task<IActionResult> Delete(string id)
        {
            var employee = await _userHelper.GetUserByIdAsync(id);
            await _userManager.DeleteAsync(employee);

            return RedirectToAction("Admin");
        }

        public IActionResult ChangeUser()
        {
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Admin", "Account");
                    }

                    if (await _userHelper.IsUserInRoleAsync(user, "Employee"))
                    {
                        return RedirectToAction("Index", "Flights");
                    }
                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login");
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Username,
                        Password = model.Password,
                        Email = model.Username
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    await _userHelper.AddUserToRoleAsync(user, "Client");


                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "Error when creating account");
                        return View(model);
                    }

                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);



                    Response response = _mailHelper.SendEmail(user.Email, "Email confirmation", $"To confirm your email please click in this link :D <a href='{tokenLink}'>Link</a>");


                    if (response.IsSuccess)
                    {

                        return RedirectToAction("Login");
                    }


                }

            }
            else { return View(model); }
            return View(model);
        }



        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {

            }

            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);

                    }
                }
            }

            return BadRequest();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {


                        Response response = _mailHelper.SendEmail(user.Email, "Mourato Airport Password Reset", "Your password was been changed");

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return this.View(model);
        }


        public IActionResult RecoverPassword()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    this.ViewBag.Message = "The email doesn't correspont to a registered user.";
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(user.Email, "Mourato Airport Password Reset", $" To recover your password click this <a href='{link}'>Link</a>");

                if (response.IsSuccess)
                {
                    ViewBag.Message = "The instructions to recover your password has been sent to email.";
                }

                return RedirectToAction("Login");

            }
            ViewBag.Message = "Error while changing the password.";
            return View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Username);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";


                    Response response = _mailHelper.SendEmail(user.Email, "Mourato Airport Password Reset", $"");
                    ViewBag.Message = "Password Successfuly Changed !";
                    return View();
                }

                ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
