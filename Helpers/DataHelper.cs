using ContactPro.Data;
using Microsoft.EntityFrameworkCore;

namespace ContactPro.Helpers
{
    public static class DataHelper
    {
        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {
            //get an instance of the db Application Context
            ApplicationDbContext dbContectSvc = svcProvider.GetRequiredService<ApplicationDbContext>();

            //migration: this is equivalent to DB Update
            // we use this to create the db in Heroku or other Hosting Providers
            await dbContectSvc.Database.MigrateAsync();


        }

    }
}
