﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EGPS.Domain.Common;

namespace EGPS.Domain.Entities
{
    public class Workflow : AuditableEntity
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public Guid AccountId { get; set; }
        public Guid CreatedById { get; set; }
        public Account Account { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Deleted { get; set; } = false;
        public ICollection<Stage> Stages { get; set; }
    }
}
