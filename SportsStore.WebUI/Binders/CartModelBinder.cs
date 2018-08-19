using SportsStore.Domain.Entities;
using System.Web.Mvc;

namespace SportsStore.WebUI.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Получаем Cart из сессии
            Cart cart = (Cart)controllerContext.HttpContext.Session[sessionKey];

            // создаем Cart, если в сессии нет записи.
            if (cart == null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session[sessionKey] = cart;
            }

            // возвращаем cart
            return cart;
        }
    }
}