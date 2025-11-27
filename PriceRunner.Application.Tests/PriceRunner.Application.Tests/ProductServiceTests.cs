using PriceRunner.Application.Validation;
using Xunit;

namespace PriceRunner.Application.Tests
{
    public class BrandValidatorTests
    {
        private readonly BrandValidator _validator = new();

        [Fact]
        public void ValidateForCreate_ReturnsError_WhenNameIsEmpty()
        {
            // Arrange
            var name = "   ";

            // Act
            var errors = _validator.ValidateForCreate(name);

            // Assert
            Assert.Contains("Name is required.", errors);
        }

        [Fact]
        public void ValidateForCreate_ReturnsError_WhenNameTooLong()
        {
            var longName = new string('a', 256);

            var errors = _validator.ValidateForCreate(longName);

            Assert.Contains("Name must be at most 255 characters.", errors);
        }

        [Fact]
        public void ValidateForCreate_ReturnsNoErrors_WhenNameIsValid()
        {
            var errors = _validator.ValidateForCreate("Samsung");

            Assert.Empty(errors);
        }
    }

    public class UserValidatorTests
    {
        private readonly UserValidator _validator = new();

        [Fact]
        public void ValidateForCreate_ReturnsError_WhenUserNameEmpty()
        {
            var errors = _validator.ValidateForCreate("   ", "secret123", 1);

            Assert.Contains("UserName is required.", errors);
        }

        [Fact]
        public void ValidateForCreate_ReturnsError_WhenPasswordTooShort()
        {
            var errors = _validator.ValidateForCreate("john", "123", 1);

            Assert.Contains("Password must be at least 6 characters.", errors);
        }

        [Fact]
        public void ValidateForCreate_ReturnsError_WhenRoleIdInvalid()
        {
            var errors = _validator.ValidateForCreate("john", "secret123", 0);

            Assert.Contains("UserRoleId must be a positive integer when provided.", errors);
        }

        [Fact]
        public void ValidateForCreate_ReturnsNoErrors_WhenAllValid()
        {
            var errors = _validator.ValidateForCreate("john", "secret123", 1);

            Assert.Empty(errors);
        }

        [Fact]
        public void ValidateForUpdate_AllowsEmptyPassword()
        {
            var errors = _validator.ValidateForUpdate("john", null, 1);

            // Ingen password-fejl forventet
            Assert.DoesNotContain(errors, e => e.Contains("Password is required"));
        }
    }
}
