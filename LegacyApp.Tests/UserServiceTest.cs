using System;
using LegacyApp;
using NUnit.Framework;

namespace LegacyApp.Tests;

[TestFixture]
[TestOf(typeof(UserService))]
public class UserServiceTest
{

    [Test]
    public void AddUser_WithValidData_ReturnsTrue()
    {
        // Arrange
        var userService = new UserService();
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var dateOfBirth = new DateTime(1990, 1, 1);
        var clientId = 1;

        // Act
        var result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);

        // Assert
        Assert.Equals(result, true);
    }

    [Test]
    public void AddUser_WithInvalidFirstName_ReturnsFalse()
    {
        // Arrange
        var userService = new UserService();
        var firstName = "";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var dateOfBirth = new DateTime(1990, 1, 1);
        var clientId = 1;

        // Act
        var result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);

        // Assert
        Assert.Equals(result, false);
    }
    
    [Test]
    public void AddUser_UnderAge_ReturnsFalse()
    {
        // Arrange
        var userService = new UserService();
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var dateOfBirth = DateTime.Now.AddYears(-10); // Under 21
        var clientId = 1;

        // Act
        var result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);

        // Assert
        Assert.Equals(result, false);
    }
}