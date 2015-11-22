using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Zenergy.Models;

namespace Zenergy.Services
{
    public class EventService
    {
        private ZenergyContext db;

        public EventService(ZenergyContext context)
        {
            this.db = context;
        }

        public async Task<ponctualEvent[]> findPonctualEventsByManagerId(int managerId)
        {
            return await db.ponctualEvent.SqlQuery("SELECT pe.* FROM ponctualEvent pe, Event e, Activity act WHERE pe.eventId=e.eventId AND e.activityId=act.activityId AND act.managerId = {0}", managerId).ToArrayAsync();
        }

        public void updateEvent (@event event1) {
            db.ponctualEvent.SqlQuery("UPDATE event SET roomId={0}, activityId={1}, eventName={2}, eventPrice={3}, eventDurationHours={4}, eventMaxPeople={5}, eventDescription={6}, timeBegin={7} WHERE eventId={8}", event1.roomId, event1.activityId, event1.eventName, event1.eventPrice, event1.eventDurationHours, event1.eventMaxPeople, event1.eventDescription, event1.timeBegin, event1.eventId);
            //db.Entry(event1).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
    }
}