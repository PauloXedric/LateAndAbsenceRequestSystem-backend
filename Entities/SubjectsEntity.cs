﻿using System.ComponentModel.DataAnnotations;

namespace DLARS.Entities
{
    public class SubjectEntity
    {
        [Key]
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
    }
}
