using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AppDbInitializer
    {
        public static void Seeder(IApplicationBuilder applicationBuilder)
        {
            using (var servicescope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = servicescope.ServiceProvider.GetService<ApplicationDbContext>();
                if (!context.Users.Any())
                {
                    context.Users.AddRange(new Contact()
                    {
                        FirstName = "Sodiq",
                        LastName = "Alabi",
                        Gender = "male",
                        StreetAddress = "Asajon",
                        City = "Ajah",
                        State = "Lagos",
                        Password = "A1234",
                        Email = "alabisdq@gmail.com",
                        DateOfBirth = DateTime.Parse("1994-09-03"),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Contact()
                    {
                        FirstName = "Faina",
                        LastName = "Gabbetis",
                        Gender = "Female",
                        StreetAddress = "Female",
                        City = "Seattle",
                        State = "Washington",
                        Password = "12345",
                        Email = "fgabbetis9@theatlantic.com",
                        DateOfBirth = DateTime.Parse("1988-05-08"),
                        CreatedAt = DateTime.Parse("2015-03-17"),
                        UpdatedAt = DateTime.Parse("2018-05-30")
                    },
                    new Contact()
                    {
                        FirstName = "Rex",
                        LastName = "Walkling",
                        Gender = "Male",
                        StreetAddress = "Female",
                        City = "Las Vegas",
                        State = "Nevada",
                        Password = "12345",
                        Email = "rwalklingd@delicious.com",
                        DateOfBirth = DateTime.Parse("1992-02-12"),
                        CreatedAt = DateTime.Parse("2016-03-18"),
                        UpdatedAt = DateTime.Parse("2019-12-16")
                    },
                    new Contact()
                    {
                        FirstName = "Yard",
                        LastName = "Grosvener",
                        Gender = "Female",
                        StreetAddress = "Genderfluid",
                        City = "Inglewood",
                        State = "California",
                        Password = "12345",
                        Email = "ygrosvenerc@google.ru",
                        DateOfBirth = DateTime.Parse("1981-07-04"),
                        CreatedAt = DateTime.Parse("2016-06-29"),
                        UpdatedAt = DateTime.Parse("2018-12-20")
         
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
