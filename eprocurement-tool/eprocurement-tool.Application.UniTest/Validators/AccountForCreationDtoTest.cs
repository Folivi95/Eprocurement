using EGPS.Application.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.UniTest.Validators
{
    [TestFixture]
    public class AccountForCreationDtoTest
    {
        private AccountForCreationDtoValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new AccountForCreationDtoValidator();
        }

        [Test]
        public void Should_have_error_when_firstname_is_null()
        {
            validator.ShouldHaveValidationErrorFor(account => account.FirstName, null as string);
        }

        [Test]
        public void Should_not_have_error_when_lastname_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(account => account.LastName, "Chikason");
        }

        [Test]
        public void Should_not_have_error_when_email_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(account => account.ContactEmail, "chuzksy@gmail.com");
        }

        [Test]
        public void Should_have_error_when_invalid_email_is_specified()
        {
            validator.ShouldHaveValidationErrorFor(account => account.ContactEmail, "chuzksy@gm");
        }

        [Test]
        public void Should_have_error_when_invalid_website_url_is_specified()
        {
            validator.ShouldHaveValidationErrorFor(account => account.ContactEmail, "www.chuzksy");
        }
    }
}
