using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Zenergy.Models;

namespace Zenergy.Services
{
    public class ActivityService
    {
        private ZenergyContext db;

        public ActivityService(ZenergyContext context)
        {
            this.db = context;
        }

        public async Task<activity[]> findActivitiesByManagerId(int managerId)
        {
            return await db.activity.SqlQuery("SELECT * FROM Activity WHERE managerId = {0}", managerId).ToArrayAsync();
        }
    }
}