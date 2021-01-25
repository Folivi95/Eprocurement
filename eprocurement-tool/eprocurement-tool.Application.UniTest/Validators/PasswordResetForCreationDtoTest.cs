using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace EGPS.Application.UniTest.Validators
{
    [TestFixture]
    public class PasswordResetForCreationDtoTest
    {
        private PasswordResetForCreationDtoValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new PasswordResetForCreationDtoValidator();
        }

        [Test]
        public void Should_not_have_error_when_token_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(p => p.Token, "Token");
        }

        [Test]
        public void Should_have_error_when_token_is_null()
        {
            validator.ShouldHaveValidationErrorFor(p => p.Token, null as string);
        }

        [Test]
        public void Should_not_have_error_when_email_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(p => p.Email, "test@gmail.com");
        }

        [Test]
        public void Should_have_error_when_email_is_invalid()
        {
            validator.ShouldHaveValidationErrorFor(p => p.Email, "test@gmail");
        }
    }
}
