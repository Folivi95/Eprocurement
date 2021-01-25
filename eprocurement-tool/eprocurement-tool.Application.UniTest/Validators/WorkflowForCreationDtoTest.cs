using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace EGPS.Application.UniTest.Validators
{
    [TestFixture]
    public class WorkflowForCreationDtoTest
    {
        private WorkflowForCreationValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new WorkflowForCreationValidator();
        }

        [Test]
        public void Should_not_have_error_when_title_is_specified()
        {
            validator.ShouldNotHaveValidationErrorFor(w => w.Title, "Title");
        }

        [Test]
        public void Should_have_error_when_title_is_null()
        {
            validator.ShouldHaveValidationErrorFor(w => w.Title, null as string);
        }

    }
}
