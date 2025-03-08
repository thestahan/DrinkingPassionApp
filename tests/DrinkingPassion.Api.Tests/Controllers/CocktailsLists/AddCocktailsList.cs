using AutoMapper;
using DrinkingPassion.Api.Controllers;
using DrinkingPassion.Api.Core.Entities;
using DrinkingPassion.Api.Core.Entities.Identity;
using DrinkingPassion.Api.Core.Interfaces;
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
    public class AddCocktailsList
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
        [TestCase("")]
        [TestCase(null)]
        public async Task ReturnsBadRequest_When_NameIsInvalid(string name)
        {
            // Arrange
            var dto = new CocktailsListToAddDto { Name = name, Cocktails = [] };

            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new AppUser { Id = "123" });

            var controller = new CocktailsListsController(_cocktailsListsRepoMock.Object,
                                                          _userManagerMock.Object,
                                                          _mapperMock.Object,
                                                          _cocktailsRepoMock.Object)
            {
                ControllerContext = Helpers.UserMockHelpers.GetControllerContextWithMockedUser()
            };

            // Act
            var result = (await controller.ManageCocktailsList(dto)).Result as ObjectResult;

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ReturnsBadRequest_When_CocktailsCountIsZero()
        {
            // Arrange
            var dto = new CocktailsListToAddDto { Name = "Name", Cocktails = [] };

            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new AppUser { Id = "123" });

            var controller = new CocktailsListsController(_cocktailsListsRepoMock.Object,
                                                          _userManagerMock.Object,
                                                          _mapperMock.Object,
                                                          _cocktailsRepoMock.Object)
            {
                ControllerContext = Helpers.UserMockHelpers.GetControllerContextWithMockedUser()
            };

            // Act
            var result = (await controller.ManageCocktailsList(dto)).Result as ObjectResult;

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }
    }
}
