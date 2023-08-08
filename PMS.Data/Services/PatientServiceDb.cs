using Microsoft.EntityFrameworkCore;
using PMS.Data.Entities;
using PMS.Data.Repositories;

namespace PMS.Data.Services;

public class PatientServiceDb : IPatientService
{
    private readonly DatabaseContext db;

    public PatientServiceDb(DatabaseContext db)
    {
       this.db = db;
    }

    public void Initialise()
    {
        db.Initialise(); // recreate database
    }

    // ==================== Patient Management =======================

    // retrieve list of Patients
    public List<Patient> GetPatients(string order=null)
    {
        var query = db.Patients;
        var results = (order.ToLower()) switch
        {  
            "patientId"     => query.OrderBy(patient => patient.Id),
            "title"   => query.OrderBy(patient => patient.Title),
            "firstName"  => query.OrderBy(patient => patient.FirstName),
            "middleName"    => query.OrderByDescending(patient => patient.MiddleName),
            "surname" => query.OrderBy(patient => patient.Surname),
            "age" => query.OrderBy(patient => patient.Dob),
            "phoneNo" => query.OrderBy(patient => patient.PhoneNo),
            "sex" => query.OrderBy(patient => patient.Sex),
            "houseNo" => query.OrderBy(patient => patient.HouseNo),
            "streetName" => query.OrderBy(patient => patient.StreetName),
            "city" => query.OrderBy(patient => patient.City),
            "postCode" => query.OrderBy(patient => patient.Postcode),
            "ethnicity" => query.OrderBy(patient => patient.Ethnicity),
            "NIN" => query.OrderBy(patient => patient.NIN),
            "email" => query.OrderBy(patient => patient.Email),
            _        => query.OrderBy(issue => issue.Id)
        };
        return results.ToList();
    }
    
    // Retrieve patient by Id 
    public Patient GetPatient(int patientid)
    {
        return db.Patients
                 .Include(patient => patient.Issues)                                
                 .FirstOrDefault(patient => patient.Id == patientid);
    }

    // Add a new patient
    public Patient AddPatient(Patient patient)
    {
        // check if patient with email exists            
        var exists = GetPatientByEmail(patient.Email);
        if (exists != null)
        {
            return null;
        } 
        

        // create new patient
        var newPatient = new Patient
        {
            Title = patient.Title,
            FirstName = patient.FirstName,
            MiddleName = patient.MiddleName,
            Surname = patient.Surname,
            Dob = patient.Dob,
            PhoneNo = patient.PhoneNo,
            Sex = patient.Sex,
            HouseNo = patient.HouseNo,
            StreetName = patient.StreetName,
            City = patient.City,
            Postcode = patient.Postcode,
            Ethnicity = patient.Ethnicity,
            NIN = patient.NIN,
            Email = patient.Email,
            // Password = patient.Password
            
        };
        db.Patients.Add(newPatient); // add patient to the list
        db.SaveChanges();
        return newPatient; // return newly added patient
    }

    // Delete the patient identified by Id returning true if 
    // deleted and false if not found
    public bool DeletePatient(int patientid)
    {
        var deletePatient = GetPatient(patientid);
        if (deletePatient == null)
        {
            return false;
        }
        db.Patients.Remove(deletePatient);
        db.SaveChanges();
        return true;
    }

    // Update the patient with the details in updated 
    public Patient UpdatePatient(Patient update)
    {
        // verify the patient exists 
        var patient = GetPatient(update.Id);
        if (patient == null)
        {
            return null;
        }

        // verify email is still unique
        var patientexists = GetPatientByEmail(update.Email);
        if (patientexists != null && patientexists.Id != update.Id)
        {
            return null;
        }


        // update the details of the patient retrieved and save
        patient.Title = update.Title;
        patient.FirstName = update.FirstName;
        patient.MiddleName = update.MiddleName;
        patient.Surname = update.Surname;
        patient.Dob = update.Dob;
        patient.PhoneNo = update.PhoneNo;
        patient.Sex = update.Sex;
        patient.HouseNo = update.HouseNo;
        patient.StreetName = update.StreetName;
        patient.City = update.City;
        patient.Postcode = update.Postcode;
        patient.Ethnicity = update.Ethnicity;
        patient.NIN = update.NIN;
        patient.Email = update.Email;
        // patient.Password = patient.Password;
            

        db.SaveChanges();
        return patient;
    }

    public Patient GetPatientByEmail(string email)
    {
        return db.Patients.FirstOrDefault(patient => patient.Email == email);
    }

    // ===================== Issues Management ==========================

     public Issue CreateIssue(int patientid, string concern, string length, int sever, string photo, string sensitive, string med, string allergy, string whichAllergy, string contrap)
        {
            
            var patient = GetPatient(patientid);
            if (patient == null) return null;

            var issue = new Issue
            {
                // pId created by Database
                CurrentConcern = concern,     
                Length = length,
                Severity = sever,
                PhotoUrl = photo,
                Sensitivities = sensitive,
                WhichMedication = med,
                Allergies = allergy,
                WhichAllergies = whichAllergy,
                Contraceptions = contrap,
                Patient = patient

           
            };
            db.Issues.Add(issue);
            db.SaveChanges(); // write to database
            return issue;
        }

        public Issue GetIssue(int issueid)
        {
            // return issue and related patient or null if not found
            return db.Issues
                     .Include(i => i.Patient)
                     .FirstOrDefault(i => i.IssueId == issueid);
        }

        public Issue UpdateIssue(int issueid, string concern, string length, int sever, string photo, string sensitive, string med, string allergy, string whichAllergy, string contrap)
        {
            var issue = GetIssue(issueid);
            // cannot update a non-existent or inactive ticket
           // if (patient == null || !ticket.Active) return null;

            issue.CurrentConcern = concern;
            issue.Length = length;
            issue.Severity = sever;
            issue.PhotoUrl = photo;
            issue.Sensitivities = sensitive;
            issue.WhichMedication = med;
            issue.Allergies = allergy;
            issue.WhichAllergies = whichAllergy;
            issue.Contraceptions = contrap;    
               
                        
            db.Issues.Update(issue);
            db.SaveChanges(); // write to database
            return issue;
        }


        public Issue OpenIssue(int issueid)
        {
            // return Issue or null if not found
            var issue = GetIssue(issueid);

            //if Issue does not exist or is already active return null
            if (issue == null || issue.Active) return null;
            
            // Issue exists and is inactive so reopen
            issue.Active = true;
            issue.ResolvedOn = DateTime.Now;
           
            db.SaveChanges(); // write to database
            return issue;
        }
        
        public Issue CloseIssue(int issueid, string resolution, string concern, string length, int sever, string photo, string sensitive, string med, 
                        string allergy, string whichAllergy, string contrap)
        {
            var issue = GetIssue(issueid);
            // if ticket does not exist or is already closed return null
            if (issue == null || !issue.Active) return null;
            
            // issue exists and is active so close
            issue.Active = false;
            issue.Resolution = resolution;
            issue.ResolvedOn = DateTime.Now;
            issue.CurrentConcern = concern;
            issue.Length = length;
            issue.Severity = sever;
            issue.PhotoUrl = photo;
            issue.Sensitivities = sensitive;
            issue.WhichMedication = med;
            issue.Allergies = allergy;
            issue.WhichAllergies = whichAllergy;
            issue.Contraceptions = contrap;
           
            db.SaveChanges(); // write to database
            return issue;
        }

        public bool DeleteIssue(int issueid)
        {
            // find issue
            var issue = GetIssue(issueid);
            if (issue == null) return false;
            
            // remove issue
            var result = db.Issues.Remove(issue);
            
            db.SaveChanges();
            return true;
        }

        // Retrieve all Issues and the pateint associated with the issue
        public IList<Issue> GetAllIssues()
        {
            return db.Issues
                     .Include(i => i.Patient)
                     .ToList();
        }

        // Retrieve all open issues (Active)
        public IList<Issue> GetOpenIssues()
        {
            // return open issues with associated patients
            return db.Issues
                     .Include(i => i.Patient) 
                     .Where(i => i.Active)
                     .ToList();
        } 

        // perform a search of the tickets based on a query and
        // an active range 'ALL', 'OPEN', 'CLOSED'
        public IList<Issue> SearchIssues(IssueRange range, string query, string orderBy="issueid", string direction="asc") 
        {
            // ensure query is not null    
            query = query == null ? "" : query.ToLower();

            // search issue, active status and Patient name
            // var search = db.Issues
            //                 .Include(i => i.Patient)
            //                 .OrderBy(i=> i.CurrentConcern)
            //                 .Where(i => (i.WhichAllergies.ToLower().Contains(query) || 
            //                              i.Length.ToLower().Contains(query)
            //                             ) &&
            //                             (range == IssueRange.OPEN && i.Active ||
            //                              range == IssueRange.CLOSED && !i.Active ||
            //                              range == IssueRange.ALL
            //                              ) 
                            
            //                 );
             var search = db.Issues
                            .Include(i => i.Patient).ToList();
                            return search;
            //  return Ordered(search, orderBy, direction).ToList();           
        }

        private IQueryable<Issue> Ordered(IQueryable<Issue> query, string orderby, string direction)
        {
            query = (orderby,direction) switch {
                ("patientid", "asc") => query.OrderBy(i => i.IssueId),
                ("patientid", "desc") => query.OrderByDescending(i => i.IssueId),
                ("name", "asc") => query.OrderBy(i => i.Allergies),
                ("name", "desc") => query.OrderByDescending(i => i.Allergies),
                ("createdon", "asc") => query.OrderBy(i => i.CreatedOn),
                ("createdon", "desc") => query.OrderByDescending(i => i.CreatedOn),                
                _ => query
            };
            return query;
        }
}
