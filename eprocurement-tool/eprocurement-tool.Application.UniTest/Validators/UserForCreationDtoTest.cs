using EGPS.Application.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.UniTest.Validators
{
    [TestFixture]
    public class UserForCreationDtoTest
    {
        private UserForCreationDtoValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new UserForCreationDtoValidator();
        }

        [Test]
        public void Should_have_error_when_firstname_is_null()
        {
            validator.ShouldHaveValidationErrorFor(user => user.FirstName, null as string);
        }

        [Test]
        public void Should_have_error_when_lastname_is_null()
        {
            validator.ShouldHaveValidationErrorFor(user => user.LastName, null as string);
        }

        [Test]
        public void Should_not_have_error_when_email_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(user => user.Email, "gofaniyi@gmail.com");
        }

        [Test]
        public void Should_have_error_when_email_is_invalid()
        {
            validator.ShouldHaveValidationErrorFor(user => user.Email, "gofaniyi@gmail");
        }

        [Test]
        public void Should_not_have_error_when_jobtitle_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(user => user.Phone, "Software Developer");
        }

        [Test]
        public void Should_have_error_when_password_is_lessthan_6()
        {
            validator.ShouldHaveValidationErrorFor(user => user.Password, "gofa");
        }
    }
}
