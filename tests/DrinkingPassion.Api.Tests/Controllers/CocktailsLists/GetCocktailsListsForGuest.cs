using AutoMapper;
using DrinkingPassion.Api.Controllers;
using DrinkingPassion.Api.Core.Entities;
using DrinkingPassion.Api.Core.Entities.Identity;
using DrinkingPassion.Api.Core.Interfaces;
using DrinkingPassion.Api.Core.Specifications.CocktailsLists;
using DrinkingPassion.Api.Dtos.CocktailsLists;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace DrinkingPassion.Api.Tests.Controllers.CocktailsLists
{
    [TestFixture]
    public class GetCocktailsListsForGuest
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
        public async Task ReturnsNotFoundCode_When_UsernameIsIncorrect()
        {
            //Arrange
            var controller = new CocktailsListsController(_cocktailsListsRepoMock.Object,
                                                          _userManagerMock.Object,
                                                          _mapperMock.Object,
                                                          _cocktailsRepoMock.Object);

            //Act
            var result = (await controller.GetCocktailsListForGuest("incorrect", "slug")).Result as ObjectResult;

            //Assert
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
        }

        [Test]
        public async Task ReturnsNotFoundCode_When_SlugIsIncorrect()
        {
            //Arrange
            string userName = "username";

            _userManagerMock.Setup(x => x.FindByNameAsync(userName)).ReturnsAsync(new AppUser());

            var controller = new CocktailsListsController(_cocktailsListsRepoMock.Object,
                                                          _userManagerMock.Object,
                                                          _mapperMock.Object,
                                                          _cocktailsRepoMock.Object);

            //Act
            var result = (await controller.GetCocktailsListForGuest(userName, "incorrectSlug")).Result as ObjectResult;

            //Assert
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
        }

        [Test]
        [TestCase("username", "slug")]
        public async Task ReturnsListWithSameId(string username, string slug)
        {
            //Arrange
            _userManagerMock.Setup(u => u.FindByNameAsync(username)).ReturnsAsync(new AppUser());
            _cocktailsListsRepoMock.Setup(r => r.GetEntityWithSpec(It.IsAny<CocktailsListByUserIdAndSlug>())).ReturnsAsync(new CocktailsList { Id = 1 });
            _mapperMock.Setup(m => m.Map<CocktailsListDetailsToReturnDto>(It.IsAny<CocktailsList>())).Returns(new CocktailsListDetailsToReturnDto { Id = 1 });

            var controller = new CocktailsListsController(_cocktailsListsRepoMock.Object,
                                                          _userManagerMock.Object,
                                                          _mapperMock.Object,
                                                          _cocktailsRepoMock.Object);

            //Act
            var result = (await controller.GetCocktailsListForGuest(username, slug)).Result as ObjectResult;
            var list = result.Value as CocktailsListDetailsToReturnDto;

            //Assert
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(list, Has.Property("Id").EqualTo(1));
        }
    }
}
