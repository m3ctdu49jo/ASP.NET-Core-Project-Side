using ShoppingMall.Web.DTOs;

namespace ShoppingMall.Web.Infrastructure.Services
{
    public interface IService<TModel, TModelDTO> where TModel : class where TModelDTO : class
    {
        IGenericService<TModel, TModelDTO> Generic {get;}
    }
}

