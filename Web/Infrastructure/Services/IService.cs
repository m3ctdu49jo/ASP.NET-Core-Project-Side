using ShoppingMall.DTOs;

namespace ShoppingMall.Infrastructure.Services
{
    public interface IService<T> where T : class
    {
        IGenericService<T> Generic {get;}
    }
}

