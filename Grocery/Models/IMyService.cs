using Microsoft.EntityFrameworkCore;

namespace Grocery.Models
{

    public interface IMyService
    {
        List<Categories> Categories();

        int CountCart(int userid);
    }

    public class MyService : IMyService
    {
        private readonly GroceryDBContext context;
        public MyService(GroceryDBContext context)
        {
            this.context = context;
        }

        public List<Categories> Categories() 
        {
            return this.context.Categories.ToList();
        }

        public int CountCart(int userid) 
        { 

            var cart = this.context.Cart.FirstOrDefault(x => x.UserID == userid);

            if (userid == 0 || cart == null)
            {
                return 0;
            }

            return this.context.CartItems.Where(x => x.CartID == cart.CartID).Count();
        }

    }
}
