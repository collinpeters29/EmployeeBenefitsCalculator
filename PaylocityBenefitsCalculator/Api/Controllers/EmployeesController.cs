using Api.DbContext;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Service;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{

    private MockDbContext _context;
    private readonly IMapper _mapper;

    public EmployeesController(MockDbContext mockDbContext, IMapper mapper)
    {
        _mapper = mapper;
        _context = mockDbContext;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employeeDto = _mapper.Map<GetEmployeeDto>(_context.GetEmployee(id)); 

        if (employeeDto == null) { return NotFound(); }
        var dependents = _mapper.Map<List<GetDependentDto>>(_context.GetDependentsUnderEmployee(employeeDto.Id));

        employeeDto.Dependents = dependents;
        
        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = employeeDto,
            Success = true
        };

        return Ok(result);
    }

    

    [SwaggerOperation(Summary = "Get employee paychecks by id")]
    [HttpGet("{id}/paycheck")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> GetPaycheck(int id)
    {
        var _checkService = new PayCheckService();

        var employeeDto = _mapper.Map<GetEmployeeDto>(_context.GetEmployee(id));

        if (employeeDto == null) { return NotFound(); }
        var dependents = _mapper.Map<List<GetDependentDto>>(_context.GetDependentsUnderEmployee(employeeDto.Id));

        employeeDto.Dependents = dependents;

        employeeDto.Paycheck = _checkService.GetGrossPay(employeeDto.Salary);

        _checkService.RunDeductions(employeeDto);

        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = employeeDto,
            Success = true
        };

        return Ok(result);
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        //task: use a more realistic production approach
        // Initial "Database" seeding moved to the Mock DbContext using a DataFactory on StartUp
        //Automapper is used to convert our Models from the Database Model to our DTO Objects

        var employeesDto = _mapper.Map<List<GetEmployeeDto>>(_context.GetEmployees());
        if (employeesDto == null)
        {
            return NotFound();
        }

        foreach (var employeeDto in employeesDto)
        {
            var dependents = _mapper.Map<List<GetDependentDto>>(_context.GetDependentsUnderEmployee(employeeDto.Id));

            employeeDto.Dependents = dependents;
        }

        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employeesDto.ToList(),
            Success = true
        };

        return Ok(result);
    }
}
