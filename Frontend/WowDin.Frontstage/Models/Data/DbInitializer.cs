using WowDin.Frontstage.Models.Entity;

namespace WowDin.Frontstage.Models.Data
{
    public static class DbInitializer
    {
        public static void Initialize(WowdinDbContext context)
        {
            context.Database.EnsureCreated();
            //look for any carts

        }
    }
}
