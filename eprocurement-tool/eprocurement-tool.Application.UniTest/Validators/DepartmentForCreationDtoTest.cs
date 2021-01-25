using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace EGPS.Application.UniTest.Validators
{
    [TestFixture]
    public class DepartmentForCreationDtoTest
    {
        private DepartmentForCreationDtoValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new DepartmentForCreationDtoValidator();
        }

        [Test]
        public void Should_not_have_error_when_name_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(d => d.Name, "Name");
        }

        [Test]
        public void Should_have_error_when_name_is_null()
        {
            validator.ShouldHaveValidationErrorFor(d => d.Name, null as string);
        }

        [Test]
        public void Should_not_have_error_when_accountId_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(d => d.Website, "www.website.com");
        }

        [Test]
        public void Should_not_have_error_when_leadId_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(d => d.LeadId, Guid.NewGuid());
        }
    }
}
