using AutoMapper;
using Makeup.Models;
using Makeup.ViewModel;

namespace Makeup.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Brand, BrandVM>().ReverseMap();
			CreateMap<Brand, BrandFormVM>().ReverseMap();
			CreateMap<Category, CategoryVM>().ReverseMap();
			CreateMap<Category, CategoryFormVM>().ReverseMap();
            CreateMap<Product, ProductVm>()
				.ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ReverseMap();
            CreateMap<Product, ProductFormVM>()
				.ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ReverseMap();
			CreateMap<Order, OrderVM>().ReverseMap();
			CreateMap<Order, OrderFormVM>().ReverseMap();
			CreateMap<Product, OrderCreateVM>().ReverseMap();
			CreateMap<ApplicationUser, ApplicationUserVM>().ReverseMap();
			CreateMap<ApplicationUser, ApplicationUserCreateVM>().ReverseMap();

		}
	}
}
