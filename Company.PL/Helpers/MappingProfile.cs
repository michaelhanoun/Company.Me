using AutoMapper;
using Company.DAL.Entites;
using Company.PL.Dtos;

namespace Company.PL.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<DepartmentDto, Department>();
        }
    }
}
