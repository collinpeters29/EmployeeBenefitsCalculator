using Api.DbContext;
using Api.Dtos.Dependent;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private MockDbContext _context;
    private readonly IMapper _mapper;

    public DependentsController(MockDbContext mockDbContext, IMapper mapper)
    {
        _mapper = mapper;
        _context = mockDbContext;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependentDto = _mapper.Map<GetDependentDto>(_context.GetDependent(id));

        if (dependentDto == null) { return NotFound(); }


        var result = new ApiResponse<GetDependentDto>
        {
            Data = dependentDto,
            Success = true
        };

        return Ok(result);
    }

    //It would be useful for an employee to Be able to see all of their direct dependents
    [SwaggerOperation(Summary = "Get all dependents associated by Employee Id")]
    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAllDependentsForEmployee(int employeeId)
    {
        var dependents = _mapper.Map<List<GetDependentDto>>(_context.GetDependentsUnderEmployee(employeeId));

        if (dependents == null)
        {
            return NotFound();
        }

        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents,
            Success = true
        };

        return Ok(result);
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = _mapper.Map<List<GetDependentDto>>(_context.GetDependents());

        if (dependents == null) { return NotFound(); }
        
        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents,
            Success = true
        };

        return Ok(result);
    }

    // Add Dependent to Employee
    // Employees should be able to add new children or New Spouses/Domestic Partners if the spouse divorce/passes away 
    [SwaggerOperation(Summary = "Add Dependent under Employee")]
    [HttpPost]
    public async Task<ActionResult<GetDependentDto>> AddDependent(int employeeId, GetDependentDto dependentDto)
    {
        var exisitingDependents = _mapper.Map<List<GetDependentDto>>(_context.GetDependentsUnderEmployee(employeeId));

        if (dependentDto.Relationship == Relationship.DomesticPartner || dependentDto.Relationship == Relationship.Spouse)
        {
            if (exisitingDependents.Any(e => e.Relationship == Relationship.Spouse || e.Relationship == Relationship.DomesticPartner))
            {
                return BadRequest("Only 1 Spouse or Domestic Partner Allowed");

            }
        }

        var dependent = _mapper.Map<Dependent>(dependentDto);
        dependent.EmployeeId = employeeId;
        _context.DependentList.Add(dependent);

        return CreatedAtAction("getDependentDto", new { id = dependent.Id }, dependentDto);
    }

    //Employees should be able to remove dependents if children move away and are no longer dependent etc.
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDependent(int id)
    {
        var dependent =  _context.DependentList.Where(e => e.Id == id).FirstOrDefault();
        if (dependent == null)
        {
            return NotFound();
        }

        _context.DependentList.Remove(dependent);
        

        return NoContent();
    }

}
