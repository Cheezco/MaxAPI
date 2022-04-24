using MaxAPI.Controllers.Account;
using MaxAPI.Models.Accounts;
using MaxAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using MaxAPI.Models;
using System;

namespace MaxAPITests
{
    public class LoginControllerTest
    {
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly LoginController _controller;

        public LoginControllerTest()
        {
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            _mockAuthenticationService
                .Setup(x => x.AuthenticateUserAsync(It.IsAny<LoginUser>()))
                .ReturnsAsync((LoginUser user) =>
                {
                    if (string.IsNullOrEmpty(user.LoginName) ||
                        string.IsNullOrEmpty(user.Password)) return null;

                    return new User()
                    {
                        Email = "email",
                        FirstName = "firstName",
                        LastName = "lastName",
                        Password = Array.Empty<byte>(),
                        PersonalCode = "0000000000",
                        Username = "username"
                    };
                });

            _controller = new LoginController(_mockAuthenticationService.Object);
        }

        [Fact]
        public void Post_WhenCalled_ReturnsOkResult()
        {
            var user = new LoginUser()
            {
                LoginName = "username",
                Password = "password",
            };

            var result = _controller
                .Login(user)
                .GetAwaiter()
                .GetResult();

            result
                .Should()
                .NotBeNull();
            result
                .Result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<OkObjectResult>();
        }

        [Fact]
        public void Post_WhenCalled_Returns401Result()
        {
            var user = new LoginUser()
            {
                LoginName = "",
                Password = "",
            };

            var result = _controller
                .Login(user)
                .GetAwaiter()
                .GetResult();

            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<ActionResult<User>?>();

            result
                .Result
                .Should()
                .BeOfType<UnauthorizedResult>();

            result
                .Result
                .As<UnauthorizedResult>()
                .StatusCode
                .Should()
                .Be(401);
        }
    }
}
