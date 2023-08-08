using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

using PMS.Data.Entities;
using PMS.Data.Services;

using PMS.Web.Models;

namespace PMS.Web.Controllers;

[Authorize]
public class IssueController : BaseController
{
    private readonly IPatientService svc;

    public IssueController(IPatientService patientService)
    {
        svc = patientService;
    }
     
    // GET/POST /Issue/index
    public IActionResult Index(IssueSearchViewModel search)
    {                  
        // set the viewmodel Issues property by calling service method 
        // using the range and query values from the viewmodel 
        search.Issues = svc.SearchIssues(search.Range, search.Query, search.OrderBy, search.Direction);

        // build custom alert message                
        var alert = $"{search.Issues.Count} result(s) found searching '{search.Range}' Issues";
        if (search.Query != null )
        {
            alert += $" for '{search.Query}'"; 
        }
        // display alert
        Alert(alert, AlertType.info);         
        
        return View(search);
    } 

    // display page containg JS query 
    public IActionResult Search()
    {
        return View();
    }

    //[AllowAnonymous]

   // GET /api/ticket/search
[HttpGet("api/ticket/search")]
public IActionResult Search(string query, IssueRange range = IssueRange.ALL)
{
    // Search for issues
    var issues = svc.SearchIssues(range, query);

    // Map issues to custom DTO object
    var data = issues.Select(i => new 
    {
        IssueId = i.IssueId,
        CurrentConcern = i.CurrentConcern,
        Length = i.Length,
        Severity = i.Severity,
        PhotoUrl = i.PhotoUrl,
        Sensitivities = i.Sensitivities,
        WhichMedication = i.WhichMedication,
        Allergies = i.Allergies,
        WhichAllergies = i.WhichAllergies,
        Contraceptions = i.Contraceptions,
        CreatedOn = i.CreatedOn,
        ResolvedOn = i.ResolvedOn,
        Active = i.Active,
        firstname = i.Patient.FirstName,
        surname = i.Patient.Surname
    });

    // Return JSON containing custom issues list
    return Ok(data);
}

                    
    // GET/issue/{issueid}
    public IActionResult Details(int issueid)
    {
        var issue = svc.GetIssue(issueid);
        if (issue == null)
        {
            Alert("Issue Not Found", AlertType.warning);  
            return RedirectToAction(nameof(Index));             
        }

        return View("Details",issue);
    }

    [HttpPost]
    [Authorize(Roles="admin,support")]
    public IActionResult Open(int issueid)
    {
        svc.OpenIssue(issueid);
        return RedirectToAction(nameof(Details), new { iId = issueid });
    }
    
    // POST /ticket/close/{id}
    [HttpPost]
    [Authorize(Roles="admin,support")]
    public IActionResult Close([Bind("IssueId, Resolution, CurrentConcern, Length, Severity, PhotoUrl, Sensitivities, WhichMedication, Allergies, WhichAllergies, Contraceptions")] Issue i)
    {
        // close ticket via service
        var issue = svc.CloseIssue(i.IssueId, i.Resolution, i.CurrentConcern, i.Length, i.Severity, i.PhotoUrl, i.Sensitivities, i.WhichMedication, i.Allergies, i.WhichAllergies, i.Contraceptions);
        if (issue == null)
        {
            Alert("Issue Not Found", AlertType.warning);                               
        }
        else
        {
            Alert($"Issue {i.IssueId } closed", AlertType.info);  
        }

        // redirect to the index view
        return RedirectToAction(nameof(Details), new { iId = i.IssueId });
    }
    
    // GET /issue/create
   [Authorize(Roles = "admin,support")]
    public IActionResult Create()
    {
        var patients = svc.GetPatients();
        // Populate view model select list property
        var Ivm = new IssueCreateViewModel(new SelectList(patients, "PatientId", "FirstName", "Surname"));
    
        // Render blank form
        return View(Ivm);
    }

    // POST /ticket/create
    [HttpPost]
    [Authorize(Roles="admin,support")]
    public IActionResult Create(IssueCreateViewModel Ivm)
    {
        if (ModelState.IsValid)
        {
            svc.CreateIssue(Ivm.PatientId, Ivm.CurrentConcern, Ivm.Length, Ivm.Severity, Ivm.PhotoUrl, Ivm.Sensitivities, Ivm.WhichMedication, Ivm.Allergies, Ivm.WhichAllergies, Ivm.Contraceptions);
    
            Alert($"Issue Created", AlertType.info);  
            return RedirectToAction(nameof(Index));
        }
        
        // redisplay the form for editing
        return View(Ivm);
    }

}

