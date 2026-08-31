using AutoMapper;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Mapping;

public class BookingMapping : Profile
{
    public BookingMapping()
    {
        CreateMap<Booking, BookingResponseDto>()
            .ForMember(d => d.HallName, o => o.MapFrom(s => s.Hall.Name))
            .ForMember(d => d.Services, o => o.MapFrom(s =>
                s.BookingServices.Select(bs => bs.RoomService)));
    }
}