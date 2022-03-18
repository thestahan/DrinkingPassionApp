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
using System.Net;
using Core.Specifications.CocktailsLists;

namespace Tests.API.Controllers.CocktailsLists
{
    [TestFixture]
    public class DeleteCocktailsList
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
        public async Task ReturnNotFoundCode_When_IdIsIncorrect()
        {
            // Arrange
            var controller = new CocktailsListsController(_cocktailsListsRepoMock.Object,
                                                          _userManagerMock.Object,
                                                          _mapperMock.Object,
                                                          _cocktailsRepoMock.Object);

            //Act
            var result = (await controller.DeleteCocktailsList(1)) as ObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task ReturnNoContentCode_When_NoErrorsThrown()
        {
            // Arrange
            _cocktailsListsRepoMock.Setup(c => c.GetEntityWithSpec(It.IsAny<CocktailsListForUserById>())).ReturnsAsync(new CocktailsList());

            var controller = new CocktailsListsController(_cocktailsListsRepoMock.Object,
                                                          _userManagerMock.Object,
                                                          _mapperMock.Object,
                                                          _cocktailsRepoMock.Object);

            //Act
            var result = (await controller.DeleteCocktailsList(1)) as StatusCodeResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }
    }
}
