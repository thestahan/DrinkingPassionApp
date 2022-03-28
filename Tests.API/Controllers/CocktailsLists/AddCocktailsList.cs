using AutoMapper;
using Core.Entities.Identity;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Tests.API.Controllers.Helpers;
using API.Dtos.CocktailsLists;
using System.Net;

namespace Tests.API.Controllers.CocktailsLists
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
        public async Task ReturnsBadRequest_When_CocktailsCountIsZero()
        {
            // Arrange
            var dto = new CocktailsListToAddDto();

            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new AppUser { Id = "123" });

            var controller = new CocktailsListsController(_cocktailsListsRepoMock.Object,
                                                          _userManagerMock.Object,
                                                          _mapperMock.Object,
                                                          _cocktailsRepoMock.Object)
            {
                ControllerContext = UserMockHelpers.GetControllerContextWithMockedUser()
            };

            // Act
            var result = (await controller.ManageCocktailsList(dto)).Result as ObjectResult;

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }
    }
}
