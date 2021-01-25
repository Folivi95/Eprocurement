using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.UniTest.Mappings
{
    [TestFixture]
    public class NotificationProfileTest
    {
        [Test]
        public void Notification_NotificationDTO_Should_HaveValidConfig()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Notification, NotificationDTO>(MemberList.None));
        
            config.AssertConfigurationIsValid();
        }

        [Test]
        public void NotificationForCreationDTO_Notification_Should_HaveValidConfig()
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<NotificationForCreationDTO, Notification>(MemberList.Source));

            configuration.AssertConfigurationIsValid();
        }
    }
}
