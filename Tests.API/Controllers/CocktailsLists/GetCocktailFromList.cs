using API.Controllers;
using API.Dtos.Cocktails;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications.Cocktails;
using Core.Specifications.CocktailsLists;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace Tests.API.Controllers.CocktailsLists
{
    [TestFixture]
    public class GetCocktailFromList
    {
        private Mock<UserManager<AppUser>> _userManagerMock;
        private Mock<IGenericRepository<CocktailsList>> _cocktailsListsRepoMock;
        private Mock<IGenericRepository<Cocktail>> _cocktailsRepoMock;
        private Mock<IMapper> _mapperMock;

        private readonly string _username = "Username";

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
        public async Task ReturnNotFoundCode_WhenListNotFound()
        {
            // Arrange
            _cocktailsListsRepoMock.Setup(c => c.EntityExistsWithSpecAsync(It.IsAny<CocktailsListWithCocktailExists>())).ReturnsAsync(false);

            var controller = new CocktailsListsController(_cocktailsListsRepoMock.Object,
                                                          _userManagerMock.Object,
                                                          _mapperMock.Object,
                                                          _cocktailsRepoMock.Object);

            // Act
            var result = (await controller.GetCocktailFromList(_username, "slug", 1)).Result as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
        }

        [Test]
        public async Task ReturnNotFoundCode_When_CocktailNotFound()
        {
            // Arrange
            _cocktailsListsRepoMock.Setup(c => c.EntityExistsWithSpecAsync(It.IsAny<CocktailsListWithCocktailExists>())).ReturnsAsync(true);

            var controller = new CocktailsListsController(_cocktailsListsRepoMock.Object,
                                                          _userManagerMock.Object,
                                                          _mapperMock.Object,
                                                          _cocktailsRepoMock.Object);

            // Act
            var result = (await controller.GetCocktailFromList(_username, "slug", 1)).Result as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
        }

        [Test]
        public async Task ReturnMappedCocktail_When_CocktailIsFound()
        {
            // Arrange
            _userManagerMock.Setup(u => u.FindByNameAsync(_username)).ReturnsAsync(new AppUser { UserName = _username });
            _cocktailsListsRepoMock.Setup(c => c.EntityExistsWithSpecAsync(It.IsAny<CocktailsListWithCocktailExists>())).ReturnsAsync(true);
            _cocktailsRepoMock.Setup(c => c.GetEntityWithSpec(It.IsAny<CocktailWithIngredientsSpecification>())).ReturnsAsync(new Cocktail { Id = 1 });
            _mapperMock.Setup(m => m.Map<CocktailDetailsToReturnDto>(It.IsAny<Cocktail>())).Returns(new CocktailDetailsToReturnDto { Id = 1 });

            var controller = new CocktailsListsController(_cocktailsListsRepoMock.Object,
                                                          _userManagerMock.Object,
                                                          _mapperMock.Object,
                                                          _cocktailsRepoMock.Object);

            // Act
            var result = (await controller.GetCocktailFromList(_username, "slug", 1)).Result as ObjectResult;
            var mappedCocktail = result.Value as CocktailDetailsToReturnDto;

            // Assert
            Assert.That(mappedCocktail, Is.Not.Null);
            Assert.That(mappedCocktail, Is.InstanceOf<CocktailDetailsToReturnDto>());
            Assert.That(mappedCocktail, Has.Property("Id").EqualTo(1));
        }
    }
}
