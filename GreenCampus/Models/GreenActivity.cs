using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GreenCampus.Models
{
    public partial class GreenActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GreenActivityId { get; set; }

        public required string Name { get; set; }
        public required string Description { get; set; }

    }
}