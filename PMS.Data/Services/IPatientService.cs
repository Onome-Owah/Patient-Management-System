using PMS.Data.Entities;
	
namespace PMS.Data.Services;

// This interface describes the operations that a StudentService class should implement
public interface IPatientService
{
    // Initialise the repository (database)  
    void Initialise();
    
    // ---------------- Patient Management --------------
    List<Patient> GetPatients(string order=null);
    Patient GetPatient(int patientid);
    Patient GetPatientByEmail(string email);
    Patient AddPatient(Patient patient);
    Patient UpdatePatient(Patient update);  
    bool DeletePatient(int patientid);
  

    // ---------------- Ticket Management ---------------
    Issue CreateIssue(int issueid, string concern, string length, int sever, string photo, string sensitive, string med, 
                        string allergy, string whichAllergy, string contrap);
    Issue GetIssue(int issueid);
    Issue UpdateIssue(int issueid, string concern, string length, int sever, string photo, string sensitive, string med, 
                        string allergy, string whichAllergy, string contrap);
    Issue CloseIssue(int issueid, string resolution, string concern, string length, int sever, string photo, string sensitive, string med, 
                        string allergy, string whichAllergy, string contrap);
    Issue OpenIssue(int issueid);
    bool   DeleteIssue(int issueid);
    IList<Issue> GetAllIssues();
    IList<Issue> GetOpenIssues();        
    IList<Issue> SearchIssues(IssueRange range, string query, string orderBy="issueid", string direction="asc");
    

}
    
