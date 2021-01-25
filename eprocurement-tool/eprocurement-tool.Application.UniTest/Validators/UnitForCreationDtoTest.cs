using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace EGPS.Application.UniTest.Validators
{
    [TestFixture]
    public class UnitForCreationDtoTest
    {
        private UnitForCreationDtoValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new UnitForCreationDtoValidator();
        }

        [Test]
        public void Should_not_have_error_when_name_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(u => u.Name, "Name");
        }

        [Test]
        public void Should_have_error_when_name_is_null()
        {
            validator.ShouldHaveValidationErrorFor(u => u.Name, null as string);
        }

        [Test]
        public void Should_not_have_error_when_leadId_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(u => u.LeadId, Guid.NewGuid());
        }

        [Test]
        public void Should_not_have_error_when_departmentId_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(u => u.DepartmentId, Guid.NewGuid());
        }

    }
}
