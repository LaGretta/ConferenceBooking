using AutoMapper;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Mapping;

public class HallMapping : Profile
{
    public HallMapping()
    {
        CreateMap<Hall, HallResponseDto>();
        CreateMap<RoomService, ServiceResponseDto>();
        CreateMap<CreateServiceDto, RoomService>();
    }
}