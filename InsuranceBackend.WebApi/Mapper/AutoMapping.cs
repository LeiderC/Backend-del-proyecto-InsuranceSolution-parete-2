using AutoMapper;
using InsuranceBackend.Models;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<Policy, PolicyBck>(); // means you want to map from User to UserDTO
    }
}