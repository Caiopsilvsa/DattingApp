using DattingApp.Data;
using System.Collections.Generic;
using System;
using System.Linq;
using DattingApp.Entities;

namespace DattingApp
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.Users.Any())
            {
                var firstUser = new AppUser()
                {
                    UserName = "Joao"
                };
                dataContext.Users.Add(firstUser);

                var secondUser = new AppUser()
                {
                    UserName = "Maria"
                };
                dataContext.Users.Add(secondUser);
                dataContext.SaveChanges();
            }
            
        }
    }
}
