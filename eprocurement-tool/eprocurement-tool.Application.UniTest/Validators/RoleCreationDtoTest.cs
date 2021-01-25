using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace EGPS.Application.UniTest.Validators
{
    [TestFixture]
    public class RoleCreationDtoTest
    {
        private RoleForCreationDtoValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new RoleForCreationDtoValidator();
        }

        [Test]
        public void Should_have_error_when_title_is_null()
        {
            validator.ShouldHaveValidationErrorFor(role => role.Title, null as string);
        }

        [Test]
        public void Should_not_have_error_when_title_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(role => role.Title, "my title");
        }
    }
}
