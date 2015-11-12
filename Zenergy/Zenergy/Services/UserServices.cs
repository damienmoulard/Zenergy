using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Zenergy.Models;

namespace Zenergy.Services
{
    public class UserServices
    {
        private ZenergyContext db;

        public UserServices (ZenergyContext context)
        {
            this.db = context;
        }

        public async Task<user[]> getAdmins()
        {
            return await db.user.SqlQuery("SELECT usr.* FROM [user] usr INNER JOIN [admin] role ON usr.userId = role.userId").ToArrayAsync();
        }

        public async Task<user[]> getManagers()
        {
            return await db.user.SqlQuery("SELECT usr.* FROM [user] usr INNER JOIN [manager] role ON usr.userId = role.userId").ToArrayAsync();
        }

        public async Task<user[]> getContributors()
        {
            return await db.user.SqlQuery("SELECT usr.* FROM [user] usr INNER JOIN [contributor] role ON usr.userId = role.userId").ToArrayAsync();
        }

        public async Task<user[]> getMembers()
        {
            return await db.user.SqlQuery("SELECT usr.* FROM [user] usr INNER JOIN [member] role ON usr.userId = role.userId").ToArrayAsync();
        }

        public async Task<user> findByMailAndPassword(string userMail, string userPass)
        {
            user user = await db.user.Where(u => u.mail == userMail && u.password == userPass).FirstAsync();
            return user;
        }

    }
}