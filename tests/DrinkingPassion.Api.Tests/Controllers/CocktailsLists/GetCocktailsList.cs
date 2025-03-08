using AutoMapper;
using DrinkingPassion.Api.Controllers;
using DrinkingPassion.Api.Core.Entities;
using DrinkingPassion.Api.Core.Entities.Identity;
using DrinkingPassion.Api.Core.Interfaces;
using DrinkingPassion.Api.Core.Specifications.CocktailsLists;
using DrinkingPassion.Api.Dtos.CocktailsLists;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrinkingPassion.Api.Tests.Controllers.CocktailsLists
{
    [TestFixture]
    public class GetCocktailsList
    {
        private Mock<UserManager<AppUser>> _userManagerMock;
        private Mock<IGenericRepository<CocktailsList>> _cocktailsListsRepoMock;
        private Mock<IGenericRepository<Cocktail>> _cocktailsRepoMock;
        private Mock<IMapper> _mapperMock;
        private static DateTime _currentDate = DateTime.Now;

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _userManagerMock = new(store.Object, null, null, null, null, null, null, null, null);
            _cocktailsListsRepoMock = new();
            _cocktailsRepoMock = new();
            _mapperMock = new();
        }

        [Test]
        public async Task ReturnsMappedList()
        {
            //Arrange
            _cocktailsListsRepoMock.Setup(c => c.ListAsync(It.IsAny<CocktailsListsForUser>())).ReturnsAsync(GetTestCocktailsLists());
            _mapperMock.Setup(m => m.Map<List<CocktailsListDto>>(It.IsAny<IEnumerable<CocktailsList>>())).Returns(GetMappedTestCocktailsLists());
            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new AppUser { Id = "123" });

            var controller = new CocktailsListsController(_cocktailsListsRepoMock.Object,
                                                          _userManagerMock.Object,
                                                          _mapperMock.Object,
                                                          _cocktailsRepoMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext() { User = Helpers.UserMockHelpers.GetMockUser() }
                }
            };

            //Act
            var result = (await controller.GetCocktailsLists()).Result as ObjectResult;
            var lists = result.Value as List<CocktailsListDto>;

            //Assert
            Assert.That(lists, Is.Not.Null);
            Assert.That(lists, Is.InstanceOf<IEnumerable<CocktailsListDto>>());
            Assert.That(lists, Has.Exactly(2).Items);
            Assert.That(lists[0], Has.Property("Id").EqualTo(1));
        }

        private static List<CocktailsList> GetTestCocktailsLists() =>
            new()
            {
                new CocktailsList { Id = 1 },
                new CocktailsList { Id = 2 },
            };

        private static List<CocktailsListDto> GetMappedTestCocktailsLists() =>
            new()
            {
                new CocktailsListDto { Id = 1, Name = "Test 1", Slug = string.Empty, CocktailsCount = 1, CreatedDate = _currentDate },
                new CocktailsListDto { Id = 2, Name = "Test 2", Slug = string.Empty, CocktailsCount = 1, CreatedDate = _currentDate },
            };
    }
}
