using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AomTung.DataAccessLayer.DataModels;

[Index("username", Name = "username", IsUnique = true)]
public partial class tbl_member
{
    [Key]
    public int id { get; set; }

    public string username { get; set; } = null!;

    [StringLength(255)]
    public string password { get; set; } = null!;

    [StringLength(255)]
    public string firstname { get; set; } = null!;

    [StringLength(255)]
    public string lastname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? date_of_birth { get; set; }

    public bool is_deleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? deleted_timestamp { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime created_timestamp { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime updated_timestamp { get; set; }
}
