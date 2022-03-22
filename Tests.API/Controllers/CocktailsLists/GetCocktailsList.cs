using API.Controllers;
using API.Dtos.CocktailsLists;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications.CocktailsLists;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Tests.API.Controllers.CocktailsLists
{
    [TestFixture]
    public class GetCocktailsList
    {
        private Mock<UserManager<AppUser>> _userManagerMock;
        private Mock<IGenericRepository<CocktailsList>> _cocktailsListsRepoMock;
        private Mock<IGenericRepository<Cocktail>> _cocktailsRepoMock;
        private Mock<IMapper> _mapperMock;

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
                    HttpContext = new DefaultHttpContext() { User = GetMockUser() }
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
                new CocktailsListDto { Id = 1 },
                new CocktailsListDto { Id = 2 },
            };

        private static ClaimsPrincipal GetMockUser() =>
            new(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, "mockEmail")
            }));
    }
}
