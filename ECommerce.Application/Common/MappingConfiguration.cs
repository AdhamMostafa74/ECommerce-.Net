using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Products.DTOs;
using ECommerce.Application.Types.DTOs;
using ECommerce.Domain.Entities;
using Mapster;

namespace ECommerce.Application.Common;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, GetAllProductResponse>()
            .Map(dest => dest.ProductBrand, src => src.ProductBrand.Name)
            .Map(dest => dest.ProductType, src => src.ProductType.Name);

        config.NewConfig<Product, GetByIdProductResponse>()
            .Map(dest => dest.ProductBrand, src => src.ProductBrand.Name)
            .Map(dest => dest.ProductType, src => src.ProductType.Name);

        config.NewConfig<ProductBrand, GetAllBrandsResponse>();

        config.NewConfig<ProductBrand, GetByIdBrandResponse>();

        config.NewConfig<ProductType, GetAllTypesResponse>();

        config.NewConfig<ProductType, GetByIdTypeResponse>();
    }
}
