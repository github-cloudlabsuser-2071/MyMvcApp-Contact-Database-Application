using Xunit;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MyMvcApp.Tests
{
    public class UserControllerTests
    {
        public UserControllerTests()
        {
            // Clear static userlist before each test
            UserController.userlist.Clear();
        }

        [Fact]
        public void Index_ReturnsViewWithUserList()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Test", Email = "test@test.com", Phone = "123" });
            var controller = new UserController();

            // Act
            var result = controller.Index(null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<System.Collections.Generic.List<User>>(result.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Index_SearchByName_ReturnsFilteredList()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Alice", Email = "alice@test.com", Phone = "123" });
            UserController.userlist.Add(new User { Id = 2, Name = "Bob", Email = "bob@test.com", Phone = "456" });
            var controller = new UserController();

            // Act
            var result = controller.Index("Alice") as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<System.Collections.Generic.List<User>>(result.Model);
            Assert.Single(model);
            Assert.Equal("Alice", model[0].Name);
        }

        [Fact]
        public void Details_ReturnsViewWithUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test", Email = "test@test.com", Phone = "123" };
            UserController.userlist.Add(user);
            var controller = new UserController();

            // Act
            var result = controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user, result.Model);
        }

        [Fact]
        public void Details_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var controller = new UserController();
            var result = controller.Details(99);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_Post_AddsUserAndRedirects()
        {
            var controller = new UserController();
            var user = new User { Name = "Test", Email = "test@test.com", Phone = "123" };
            var result = controller.Create(user) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Single(UserController.userlist);
        }

        [Fact]
        public void Edit_Get_ReturnsViewWithUser_WhenUserExists()
        {
            var user = new User { Id = 1, Name = "Test", Email = "test@test.com", Phone = "123" };
            UserController.userlist.Add(user);
            var controller = new UserController();
            var result = controller.Edit(1) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(user, result.Model);
        }

        [Fact]
        public void Edit_Get_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var controller = new UserController();
            var result = controller.Edit(99);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Post_UpdatesUserAndRedirects()
        {
            var user = new User { Id = 1, Name = "Old", Email = "old@test.com", Phone = "111" };
            UserController.userlist.Add(user);
            var controller = new UserController();
            var updated = new User { Name = "New", Email = "new@test.com", Phone = "222" };
            var result = controller.Edit(1, updated) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("New", user.Name);
            Assert.Equal("new@test.com", user.Email);
            Assert.Equal("222", user.Phone);
        }

        [Fact]
        public void Edit_Post_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var controller = new UserController();
            var updated = new User { Name = "New", Email = "new@test.com", Phone = "222" };
            var result = controller.Edit(99, updated);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_Get_ReturnsViewWithUser_WhenUserExists()
        {
            var user = new User { Id = 1, Name = "Test", Email = "test@test.com", Phone = "123" };
            UserController.userlist.Add(user);
            var controller = new UserController();
            var result = controller.Delete(1) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(user, result.Model);
        }

        [Fact]
        public void Delete_Get_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var controller = new UserController();
            var result = controller.Delete(99);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_Post_RemovesUserAndRedirects()
        {
            var user = new User { Id = 1, Name = "Test", Email = "test@test.com", Phone = "123" };
            UserController.userlist.Add(user);
            var controller = new UserController();
            var result = controller.Delete(1, new Microsoft.AspNetCore.Http.FormCollection(new System.Collections.Generic.Dictionary<string, Microsoft.Extensions.Primitives.StringValues>())) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Empty(UserController.userlist);
        }

        [Fact]
        public void Delete_Post_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var controller = new UserController();
            var result = controller.Delete(99, new Microsoft.AspNetCore.Http.FormCollection(new System.Collections.Generic.Dictionary<string, Microsoft.Extensions.Primitives.StringValues>()));
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
