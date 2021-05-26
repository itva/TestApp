using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TestDbModel.Tables
{
    [Table("dbo.Subdivisions")]
    public class Subdivision
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public long? ParentId { get; set; }
        
        [ForeignKey("ParentId")]
        public virtual Subdivision Parent { get; set; }

        public virtual ICollection<Subdivision> Child { get; set; }

        public string Name { get; set; }
}
}
