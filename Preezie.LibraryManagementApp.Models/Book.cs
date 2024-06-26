﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preezie.LibraryManagementApp.Models;

[Table("Books")]
public class Book
{
    [Key]
    public int BookId { get; set; }

    [Required]
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Author { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string ISBN { get; set; } = string.Empty;

    public bool IsBorrowed { get; set; } = false;
}

