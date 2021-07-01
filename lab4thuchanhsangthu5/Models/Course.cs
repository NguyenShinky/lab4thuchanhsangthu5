namespace lab4thuchanhsangthu5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string LecturerId { get; set; }

        [Required]
        [StringLength(255)]
        public string Place { get; set; }

        [Required]

        public DateTime DateTime { get; set; }

        [Required]

        public int CategoryId { get; set; }

        public String Name;

        public virtual Category Category { get; set; }

        

        public List<Category> ListCategory = new List<Category>();
    }
}
