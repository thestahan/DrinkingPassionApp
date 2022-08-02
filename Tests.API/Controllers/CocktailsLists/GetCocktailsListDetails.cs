using AutoMapper;
using Core.Entities.Identity;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tests.API.Controllers.Helpers;
using System.Net;
using Core.Specifications.CocktailsLists;
using API.Dtos.CocktailsLists;

namespace Tests.API.Controllers.CocktailsLists
{
    [TestFixture]
    public class GetCocktailsListDetails
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
        public async Task ReturnNotFound_When_ListNotFound()
        {
            // Arrange
            //_cocktailsListsRepoMock.Setup(c => c.GetEntityWithSpec(It.IsAny<CocktailsListForUserByIdWithCocktails>())).ReturnsAsync(new CocktailsList { Id = 1 });
            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new AppUser { Id = "123" });

            var controller = new CocktailsListsController(_cocktailsListsRepoMock.Object,
                                                          _userManagerMock.Object,
                                                          _mapperMock.Object,
                                                          _cocktailsRepoMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext() { User = UserMockHelpers.GetMockUser() }
                }
            };

            //Act
            var result = (await controller.GetCocktailsListDetails(1)).Result as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
        }

        [Test]
        public async Task ReturnMappedList_When_NoErrors()
        {
            // Arrange
            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new AppUser { Id = "123" });
            _cocktailsListsRepoMock.Setup(c => c.GetEntityWithSpec(It.IsAny<CocktailsListForUserByIdWithCocktails>())).ReturnsAsync(new CocktailsList { Id = 1 });
            _mapperMock.Setup(m => m.Map<CocktailsListDetailsDto>(It.IsAny<CocktailsList>())).Returns(new CocktailsListDetailsDto { Id = 1 });

            var controller = new CocktailsListsController(_cocktailsListsRepoMock.Object,
                                                          _userManagerMock.Object,
                                                          _mapperMock.Object,
                                                          _cocktailsRepoMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext() { User = UserMockHelpers.GetMockUser() }
                }
            };

            //Act
            var result = (await controller.GetCocktailsListDetails(1)).Result as ObjectResult;
            var mappedList = result.Value as CocktailsListDetailsDto;

            // Assert
            Assert.That(mappedList, Is.Not.Null);
            Assert.That(mappedList, Is.InstanceOf<CocktailsListDetailsDto>());
            Assert.That(mappedList, Has.Property("Id").EqualTo(1));
        }
    }
}
