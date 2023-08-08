
using PMS.Data.Entities;

namespace PMS.Data.Services
{
    public static class Seeder
    {
        public static void Seed(IUserService userSvc, IPatientService patientSvc)
        {
            // only do this once
            patientSvc.Initialise();

            SeedUsers(userSvc);
            SeedPatients(patientSvc);
        }

        private static void SeedUsers(IUserService svc)
        {
            // Note: do not call Initialise here
            svc.AddUser("Administrator", "admin@mail.com", "admin", Role.admin);
            svc.AddUser("Manager", "manager@mail.com", "manager", Role.manager);
            svc.AddUser("Guest", "guest@mail.com", "guest", Role.guest);
        }


         public static void SeedPatients(IPatientService svc)
    {
        // Note: do not call initialise here
        
        var p1 = svc.AddPatient( new Patient {
            Title = "Miss", FirstName = "Lewis",  MiddleName = "Sam", Surname = "Capaldi",  Dob = new DateTime(1990,2,3), PhoneNo = "+440768643335", Sex = "F", 
            HouseNo = "4", StreetName = "Stay away", City = "Hull", Postcode = "BT5 05E", Ethnicity = "Black African",
            NIN = "098765432", Email = "lewis@mail.com" 
        });

         var p2 = svc.AddPatient( new Patient {
            Title = "Mr", FirstName = "Jame",  MiddleName = "Sam", Surname = "Alex",  Dob = new DateTime(1990,2,3), PhoneNo = "+440768643335", Sex = "F", 
            HouseNo = "4", StreetName = "Stay away", City = "Hull", Postcode = "BT5 05E", Ethnicity = "Black African",
            NIN = "098765432", Email = "alex@mail.com" 
        });

         var p3 = svc.AddPatient( new Patient {
            Title = "Miss", FirstName = "Esther",  MiddleName = "Sam", Surname = "Mark",  Dob = new DateTime(1990,2,3), PhoneNo = "+440768643335", Sex = "F", 
            HouseNo = "4", StreetName = "Stay away", City = "Hull", Postcode = "BT5 05E", Ethnicity = "Black African",
            NIN = "098765432", Email = "lippy@mail.com" 
        });

         var p4 = svc.AddPatient( new Patient {
            Title = "Miss", FirstName = "Fisher",  MiddleName = "Sam", Surname = "Steel",  Dob = new DateTime(1990,2,3), PhoneNo = "+440768643335", Sex = "F", 
            HouseNo = "4", StreetName = "Stay away", City = "Hull", Postcode = "BT5 05E", Ethnicity = "Black African",
            NIN = "098765432", Email = "steel@mail.com" 
        });
        


        // // seed Issues

        



        // add Issues for lewis
        var Issue1 = svc.CreateIssue(p1.Id, "Cannot feel my fingers", "5 days", 6, "https://static.wikia.nocookie.net/simpsons/images/b/bd/Homer_Simpson.png",
         "None", "Paracetamol", "dog hair", "phynol", "none" );

        var Issue2 = svc.CreateIssue(p1.Id, "Cannot feel my fingers", "5 days", 6, "https://static.wikia.nocookie.net/simpsons/images/b/bd/Homer_Simpson.png",
         "None", "Paracetamol", "dog hair", "phynol", "none" );

        var Issue3 = svc.CreateIssue(p1.Id, "Cannot feel my fingers", "5 days", 6, "https://static.wikia.nocookie.net/simpsons/images/b/bd/Homer_Simpson.png",
         "None", "Paracetamol", "dog hair", "phynol", "none" ); 
       

        // // add ticket for marge
        var Issue4 = svc.CreateIssue(p2.Id, "Cannot feel my fingers", "5 days", 6, "https://static.wikia.nocookie.net/simpsons/images/b/bd/Homer_Simpson.png",
         "None", "Paracetamol", "dog hair", "phynol", "none" );

        // // add ticket for bart
        var Issue5 = svc.CreateIssue(p3.Id, "Cannot feel my fingers", "5 days", 6, "https://static.wikia.nocookie.net/simpsons/images/b/bd/Homer_Simpson.png",
         "None", "Paracetamol", "dog hair", "phynol", "none" );
        
    }
    }
}