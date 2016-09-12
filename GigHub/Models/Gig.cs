using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace GigHub.Models
{
    public class Gig
    {
        public int ID { get; set; }

        public bool IsCanceled { get; private set; }

        public ApplicationUser Artist { get; set; }

        [Required]
        public string ArtistId { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }


        public Genre Genre { get; set; }

        [Required]
        public byte GenreId { get; set; }

        public ICollection<Attendance> Attendances { get; private set; }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        public void Cancel()
        {
            this.IsCanceled = true; // this. is greyed out since we don't need it.



            var notification = Notification.GigCanceled(this);

            //  var attendes = _context.Attendances.Where(a => a.GigId == gig.ID).Select(a => a.Attendee).ToList();
            //we did the attendes in var gig with the .include method. 

            foreach (var attendee in this.Attendances.Select(a => a.Attendee)) // this is greyed out coz we don't need it.
            {

                attendee.Notify(notification);

            }
        }

        public void Modify(DateTime dateTime, string venue, byte genre)
        {
            var notification = Notification.GigUpdated(this, DateTime, Venue); // do not write new since it is a static method not an instance.

            Venue = venue;
            DateTime = dateTime;
            GenreId = genre;

            foreach (var attendee in Attendances.Select(a => a.Attendee))
                attendee.Notify(notification);


        }
    }


}