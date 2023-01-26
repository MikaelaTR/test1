namespace AdvancedProjectMVC.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            context.SaveChanges();
        }
    }
}
