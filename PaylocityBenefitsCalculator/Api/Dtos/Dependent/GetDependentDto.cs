using Api.Helpers;
using Api.Models;

namespace Api.Dtos.Dependent;

public class GetDependentDto
{

    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Relationship Relationship { get; set; }
    public bool IsOverDependentCutoff
    {
        get { return DataHelper.isOverAgeCutoff(DateOfBirth, Config.DependentAgeCutoff); }
    }
}
