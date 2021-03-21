﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatRenta.Domain
{
    [Table("tblCats")]
    public class AppCat
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        [Required, StringLength(4000)]
        public string Details { get; set; }
        public bool Gender { get; set; }
        [StringLength(255)]
        public string Image { get; set; }
    }
}
