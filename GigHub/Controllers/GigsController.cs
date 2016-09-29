using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private ApplicationDbContext _context;
        public GigsController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Gigs
        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Gigs
                .Where(g => g.ArtistId == userId && g.DateTime > DateTime.Now && !g.IsCanceled)
                .Include(g => g.Genre)
                .ToList();

            return View(gigs);
        }


        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Attendances.
                Where(a => a.AttendeeId == userId).
                Select(a => a.Gig).
                Include(g => g.Artist).
                Include(g => g.Genre).
                ToList();

            var attendances = _context.Attendances
                .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                .ToList().ToLookup(a => a.GigId);

            var viewModel = new HomeViewModel()
            {
                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending",
                SearchTerm = "",
                Attendances = attendances

            };

            return View("Gigs", viewModel);
        }

        public ActionResult Search(HomeViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });

        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList(),
                Heading = "Add a Gig"
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(g => g.ID == id && g.ArtistId == userId);
            var viewModel = new GigFormViewModel
            {
                Id = gig.ID,
                Genres = _context.Genres.ToList(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue,
                Heading = "Edit a gig"
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };
            _context.Gigs.Add(gig);
            _context.SaveChanges();
            return RedirectToAction("Mine", "Gigs");
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs.Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.ID == viewModel.Id && g.ArtistId == userId);
            /*gig.Venue = viewModel.Venue;
            gig.DateTime = viewModel.GetDateTime();
            gig.GenreId = viewModel.Genre;*/
            // instead of the one above we define a modify method and added three properties to refactor the code.

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);

            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        public ActionResult Details(int id)
        {

            var gig = _context.Gigs.Include(g => g.Artist).Include(g => g.Genre).SingleOrDefault(g => g.ID == id);

            if (gig == null)
                return HttpNotFound();

            var viewModel = new GigDetailsViewModel
            {
                Gig = gig,
            };

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                viewModel.IsAttending = _context.Attendances.Any(a => a.GigId == gig.ID && a.AttendeeId == userId);

                viewModel.IsFollowing = _context.Followings.Any(f => f.FolloweeId == gig.ArtistId && f.FollowerId == userId);

            }

            return View("Details", viewModel);

        }


    }
}