﻿using AuctionService.DTOs;
using AuctionService.Entites;
using AutoMapper;
using Contracts;

namespace AuctionService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        CreateMap<Item, AuctionDto>();
        CreateMap<CreateAuctionDto, Auction>()
            .ForMember(d => d.Item, o => o.MapFrom(s => s));
           CreateMap<CreateAuctionDto, Item>();

        //? AuctionDto cannot be know by the     
        CreateMap<AuctionDto, AuctionCreated>();   

   }
}