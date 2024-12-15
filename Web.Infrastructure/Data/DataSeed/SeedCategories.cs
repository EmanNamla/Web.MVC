using Web.Core.Entities;

namespace Web.Infrastructure.Data.DataSeed
{
    public static class SeedCategories
    {
        public static void SeedAllCategories(ApplicationDbContext context)
        {
            if (!context.Categorys.Any())
            {
                var categories = new List<Category>
            {
                new Category { Name = "Electronics" },
                new Category { Name = "Books" },
                new Category { Name = "Clothing" },
                new Category { Name = "Home Appliances" },
                new Category { Name = "Sports" }
            };

                context.Categorys.AddRange(categories);
                context.SaveChanges();
            }
        }
    }
}
