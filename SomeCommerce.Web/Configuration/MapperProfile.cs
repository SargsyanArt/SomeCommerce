using AutoMapper;
using SomeCommerce.Core.Entities;
using SomeCommerce.Web.Models;

namespace SomeCommerce.Web.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Agreement, AgreementModel>();
            CreateMap<Product, ProductModel>();
            CreateMap<ProductGroup, ProductGroupModel>();
            CreateMap<SomeUser, SomeUserModel>();
         
            
            CreateMap<ProductModel, Product>();
            CreateMap<ProductGroupModel, ProductGroup>();
            CreateMap<AgreementModel, Agreement>();
            CreateMap<SomeUserModel, SomeUser>();
        }
    }
}
