﻿using FluentAssertions;
using GigHub.Models;
using GigHub.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Entity;

namespace GigHub.Tests.Persistence.Repositories
{

    [TestClass]
    public class GigRepositoryTests
    {
        private GigRepository _repository;
        private Mock<DbSet<Gig>> _mockGigs;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockGigs = new Mock<DbSet<Gig>>();
            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Gigs).Returns(_mockGigs.Object);

            _repository = new GigRepository(mockContext.Object);
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_ShouldNotBeReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(-1), ArtistId = "1" };

            //  _mockGigs.SetSource(new List<Gig> { gig }); or you could write the code below.
            _mockGigs.SetSource(new[] { gig });
            var gigs = _repository.GetUpcomingGigsByArtist("1");
            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsCanceled_ShouldnotBeReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };
            gig.Cancel();

            _mockGigs.SetSource(new[] { gig });
            var gigs = _repository.GetUpcomingGigsByArtist("1");
            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsForADiffrentArtist_ShouldNotReturn()
        {

            var gig = new Gig { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };

            _mockGigs.SetSource(new[] { gig });
            var gigs = _repository.GetUpcomingGigsByArtist(gig.ArtistId + "-");
            gigs.Should().BeEmpty();

        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsForTheGivenArtistAndIsInTheFuture_ShouldBeReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };

            //  _mockGigs.SetSource(new List<Gig> { gig }); or you could write the code below.
            _mockGigs.SetSource(new[] { gig });
            var gigs = _repository.GetUpcomingGigsByArtist("1");
            gigs.Should().Contain(gig);
        }
    }
}
