using Microsoft.EntityFrameworkCore;

namespace Grocery.Models
{
    public interface AdminService
    {

        List <Products> Product();

        List <Categories> Categories();
    }

    public class Services : AdminService
    {
        private readonly GroceryDBContext context;

        public Services(GroceryDBContext context)
        {
            this.context = context;
        }

        public List<Products> Product()
        {
            return this.context.Products.ToList();
        }

        public List<Categories> Categories()
        {
            return this.context.Categories.ToList();
        }
    }
}
