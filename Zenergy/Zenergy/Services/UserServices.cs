﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Zenergy.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

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


        /// <summary>
        /// Find user by mail and password.
        /// </summary>
        /// <param name="userMail"></param>
        /// <param name="userPass"></param>
        /// <returns></returns>
        public async Task<user> findByMailAndPassword(string userMail, string userPass)
        {
            try
            {
                user user = await db.user.Where(u => u.mail == userMail && u.password == userPass).FirstAsync();
                return user;
            }
            catch (InvalidOperationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                return null;
            }
        }

        public async Task<user> findByMail(string userMail)
        {
            try
            {
                user user = await db.user.Where(u => u.mail == userMail).FirstAsync();
                return user;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                return null;
            }
        }

        public async Task CreateUser(user u)
        {
            db.user.Add(u);
            await db.SaveChangesAsync();
        }

    }
}