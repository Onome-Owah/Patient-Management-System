using PMS.Data.Entities;

namespace PMS.Web.Models;

public class IssueSearchViewModel
{
    // result set
    public IList<Issue> Issues { get; set;} = new List<Issue>();



    // search options        

    public string FirstName {get; set;}

    public string Surname {get; set;}


    public string Query { get; set; } = string.Empty;
    public IssueRange Range { get; set; } = IssueRange.ALL;

    public string OrderBy { get; set; } = "id";
    public string Direction { get; set; } = "asc";
}