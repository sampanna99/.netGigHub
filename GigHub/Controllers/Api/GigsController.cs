using GigHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        // private IUnitOfWork @object;// I added this because I get an error in test 
        // private ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public GigsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public GigsController(IUnitOfWork @object) // I added this whole coz I got an error. Could be wrong. if delete this as whole and up
        //{ don't worry about it, coz it was due to not using unitofwork in controller.. I was still using context. So it messed up.
        //    this.@object = @object;
        //}

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();

            var gig = _unitOfWork.Gigs.GetGigWithAttendees(id);
            //var gig = _context.Gigs
            //    .Single(g => g.ID == id && g.ArtistId == userId);

            // we got some knowledge from testing it what if the gig is null.

            if (gig == null || gig.IsCanceled)
                return NotFound();


            if (gig.ArtistId != userId)
                return Unauthorized();



            gig.Cancel(); // this is the canccel method in domain model..

            _unitOfWork.Complete();
            // _context.SaveChanges();

            return Ok();

        }




    }
}
