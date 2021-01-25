using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Models;
using EGPS.Application.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;


namespace EGPS.Application.UniTest.Validators
{
    [TestFixture]
    public class DocumentClassDtoTest
    {
        private DocumentClassForCreationDtoValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new DocumentClassForCreationDtoValidator();
        }

        [Test]
        public void Should_not_have_error_when_title_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(d => d.Title, "Document Class Tilte");
        }

        [Test]
        public void Should_have_error_when_title_is_null()
        {
            validator.ShouldHaveValidationErrorFor(d => d.Title, null as string);
        }

        [Test]
        public void Should_Return_False_for_Invalid_accountId()
        {
            var accountId = "Asgsgsg-2233-13ststs";
            Guid guidResult;

            Assert.IsFalse(Guid.TryParse(accountId, out guidResult));
        }


    }
}
