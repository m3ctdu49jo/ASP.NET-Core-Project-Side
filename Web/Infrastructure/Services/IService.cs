using ShoppingMall.Web.DTOs;

namespace ShoppingMall.Web.Infrastructure.Services
{
    public interface IService<TModel> where TModel : class
    {
        IGenericService<TModel> Generic {get;}
    }
}

