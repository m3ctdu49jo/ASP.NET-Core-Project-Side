using ShoppingMall.DTOs;

namespace ShoppingMall.Infrastructure.Services
{
    public interface IService<TModel, TModelDTO> where TModel : class where TModelDTO : class
    {
        IGenericService<TModel, TModelDTO> Generic {get;}
    }
}

