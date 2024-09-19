using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using AutoMapper;

namespace Api.Mappers
{
    public class MapProfiles : Profile
    {
        public MapProfiles() { 
        
            CreateMap<Employee,GetEmployeeDto>();
            CreateMap<GetEmployeeDto, Employee>();
            CreateMap<Dependent, GetDependentDto>();
            CreateMap<GetDependentDto, Dependent>();
        }


    }
}
