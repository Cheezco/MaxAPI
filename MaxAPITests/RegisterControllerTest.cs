using MaxAPI.Controllers.Account;
using MaxAPI.Models.Accounts;
using MaxAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;

namespace MaxAPITests
{
    public class RegisterControllerTest
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly RegisterController _controller;

        public RegisterControllerTest()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new RegisterController(_mockUserService.Object);
        }

        [Fact]
        public void Post_WhenCalled_Returns204Result()
        {
            var user = new RegisterUser()
            {
                Email = "email",
                FirstName = "firstName",
                LastName = "lastName",
                Password = "password",
                PersonalCode = "0000000000",
                Username = "username"
            };

            var result = _controller
                .Register(user)
                .GetAwaiter()
                .GetResult();

            result
                .Should()
                .NotBeNull()
                .And
                .Subject
                .Should()
                .BeOfType<StatusCodeResult>()
                .And
                .Subject
                .As<StatusCodeResult>()
                .StatusCode
                .Should()
                .Be(204);
        }
    }
}