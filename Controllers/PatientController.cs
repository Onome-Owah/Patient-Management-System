using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PMS.Data.Entities;
using PMS.Data.Services;

namespace PMS.Web.Controllers;

[Authorize]
public class PatientController : BaseController
{
    private IPatientService svc;
    
    public PatientController(IPatientService _svc)
    { 
        svc = _svc;
    }

    // GET /patient
    public IActionResult Index(string order="patientid", string direction="asc")
    {
        // load patients using service and pass to view
        var data = svc.GetPatients(order);
        
        return View(data);
    }

    // GET /patient/details/{id}
    public IActionResult Details(int id)
    {
        var patient = svc.GetPatient(id);
      
        // check if patient is null and alert/redirect 
        if (patient is null) {
            Alert("Patient not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }
        return View(patient);
    }

    // GET: /patient/create
    [Authorize(Roles="admin,support,guest")]
    public IActionResult Create()
    {
        // display blank form to create a patient
        return View();
    }

    // POST /patient/create
    [Authorize(Roles="admin,support,guest")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult Create([Bind("Title, FirstName, MiddleName, Surname, Dob, PhoneNo, Sex, HouseNo, StreetName, City, Postcode, Ethnicity, NIN, Email")] Patient p)
    {   
        // validate email is unique
        if (svc.GetPatientByEmail(p.Email) != null)
        {
            ModelState.AddModelError(nameof(p.Email), "The email address is already in use");
        } 

        // complete POST action to add patient 
        if (ModelState.IsValid)
        {
            // call service AddPatient method using data in p
            var patient = svc.AddPatient(p);
            if (patient is null) 
            {
                Alert("Issue creating the patient", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Details), new { pId = patient.Id});   
        }
        
        // redisplay the form for editing as there are validation errors
        return View(p);
    }

    // GET /patient/edit/{id}
    [Authorize(Roles="admin,support,guest")]
    public IActionResult Edit(int id)
    {
        // load the patient using the service
        var patient = svc.GetPatient(id);

        // check if patient is null and Alert/Redirect
        if (patient is null)
        {
            Alert("patient not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }  

        // pass patient to view for editing
        return View(patient);
    }

    // POST /patient/edit/{patientid}
    [ValidateAntiForgeryToken]
    [Authorize(Roles="admin,support,guest")]
    [HttpPost]
    public IActionResult Edit(int id, Patient p)
    {
        // check if email exists and is not owned by patient being edited 
        var existing = svc.GetPatientByEmail(p.Email);
        if (existing != null && p.Id != existing.Id) 
        {
           ModelState.AddModelError(nameof(p.Email), "The email address is already in use");
        } 

        // complete POST action to save patient changes
        if (ModelState.IsValid)
        {            
            var patient = svc.UpdatePatient(p);
            if (patient is null) 
            {
                Alert("Issue updating the patient", AlertType.warning);
            }

            // redirect back to view the patient details
            return RedirectToAction(nameof(Details), new { id = p.Id });
        }

        // redisplay the form for editing as validation errors
        return View(p);
    }

    // GET / patient/delete/{patientid}
    [Authorize(Roles="admin")]      
    public IActionResult Delete(int id)
    {
        // load the patient using the service
        var patient = svc.GetPatient(id);
        // check the returned patient is not null and if so return NotFound()
        if (patient == null)
        {
            Alert("Patient not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }     
        
        // pass patient to view for deletion confirmation
        return View(patient);
    }

    // POST /patient/delete/{patientid}
    [HttpPost]
    [Authorize(Roles="admin")]
    [ValidateAntiForgeryToken]   
    public IActionResult DeleteConfirm(int id)
    {
        // delete patient via service
        var deleted = svc.DeletePatient(id);
        if (deleted)
        {
            Alert("Patient deleted", AlertType.success);            
        }
        else
        {
            Alert("Patient could not  be deleted", AlertType.warning);           
        }
        
        // redirect to the index view
        return RedirectToAction(nameof(Index));
    }

     // ============== Patient Issue management ==============

    // GET /patient/Issuecreate/{issueid}
    public IActionResult IssueCreate(int issueid)
    {
        var patient = svc.GetPatient(issueid);
        if (patient == null)
        {
            Alert("Patient not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }

        // create a Issue view model and set foreign key
        var issue = new Issue { PatientId = issueid }; 
        // render blank form
        return View( issue );
    }

    // POST /patient/Issuecreate
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult IssueCreate([Bind("PatientId, CurrentConcern, Length, Severity, PhotoUrl, Sensitivities, WhichMedication, Allergies, WhichAllergies, Contraceptions")] Issue i)
    {
        if (ModelState.IsValid)
        {                
            var issue = svc.CreateIssue(i.PatientId, i.CurrentConcern, i.Length, i.Severity, i.PhotoUrl, i.Sensitivities, i.WhichMedication, i.Allergies, i.WhichAllergies, i.Contraceptions);
            Alert($"Issue created successfully for student {i.IssueId}", AlertType.info);            
            // redirect to display patient - note how Id is passed
            return RedirectToAction(
                nameof(Details), new { issueId = issue.IssueId }
            );
        }
        // redisplay the form for editing
        return View(i);
    }

     // GET /patient/Issueedit/{issueid}
    public IActionResult IssueEdit(int issueid)
    {
        var patient = svc.GetIssue(issueid);
        if (patient == null)
        {
            Alert($"Issue {issueid} not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }        
        return View( patient );
    }

    // POST /patient/issueedit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult IssueEdit(int issueid, [Bind("IssueId, CurrentConcern, Length, Severity, PhotoUrl, Sensitivities, WhichMedication, Allergies, WhichAllergies, Contraceptions")] Issue i)
    {
        if (ModelState.IsValid)
        {                
            var issue = svc.UpdateIssue(issueid, i.CurrentConcern, i.Length, i.Severity, i.PhotoUrl, i.Sensitivities, i.WhichMedication, i.Allergies, i.WhichAllergies, i.Contraceptions);
            return RedirectToAction(
                nameof(Details), new { iId = i.IssueId }
            );
        }
        // redisplay the form for editing
        return View(i);
    }

    // GET /patient/issuedelete/{issueid}
    public IActionResult IssueDelete(int issueid)
    {
        // load the issue using the service
        var issue = svc.GetIssue(issueid);
        // check the returned Issue is not null and if so return NotFound()
        if (issue == null)
        {
            Alert("Issue not found", AlertType.warning);
            return RedirectToAction(nameof(Index));;
        }     
        
        // pass issue to view for deletion confirmation
        return View(issue);
    }

    // POST /student/ticketdeleteconfirm/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult IssueDeleteConfirm(int issueid, int patientId)
    {
        // delete issue via service
        var deleted = svc.DeleteIssue(issueid);
        if (deleted)
        {
            Alert("Issue deleted", AlertType.success);            
        }
        else
        {
            Alert("Issue could not  be deleted", AlertType.warning);           
        }

        // redirect to the student details view
        return RedirectToAction(nameof(Details), new { iId = patientId });
    }

}