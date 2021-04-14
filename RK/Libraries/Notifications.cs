using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
namespace RK.Libraries
{
    public class Notifications
    {
        public static void Add(string to, string from, string subject, string body, int priority = 1)
        {
            rekursosEntities db = new rekursosEntities();

            notifications notification = new notifications();

            notification.from = from;
            notification.to = to;
            notification.subject = subject;
            notification.body = body;
            notification.created_on = DateTime.Now;
            notification.priority = priority;
            db.notifications.Add(notification);
            db.SaveChanges();
        }
    }
}