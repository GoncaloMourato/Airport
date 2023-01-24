using MouratoAirport.Data.Entities;
using MouratoAirport.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MouratoAirport.Data;
using System;
using System.Threading.Tasks;

namespace MouratoAirport.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;


        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }


        public async Task SeedAsync()
        {

            await _context.Database.MigrateAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Client");
            await _userHelper.CheckRoleAsync("Employee");
            //criar mais roles 

            var user = await _userHelper.GetUserByEmailAsync("goncalomourato1@gmail.com");
            if (user == null)
            {

                user = new User
                {
                    UserName = "goncalomourato1@gmail.com",
                    FirstName = "Gonçalo",
                    LastName = "Mourato",
                    Password = "123456",
                    Email = "goncalomourato1@gmail.com"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

                await _userHelper.ConfirmEmailAsync(user, token);



                await _context.SaveChangesAsync();
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

        }
    }
}
