using SoftJail.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.Data.Models
{
    public class Officer
    {
        public Officer()
        {
                
             this.OfficerPrisoners = new HashSet<OfficerPrisoner>();
        }
        
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string FullName { get; set; }

    //[Required]
    public decimal Salary { get; set; }//?

     [Required]
     public virtual Position Position { get; set; }

     [Required]
     public virtual Weapon Weapon { get; set; }

    //[Required]
    public int DepartmentId { get; set; }

    //[Required]
    public virtual Department Department { get; set; }

    public ICollection<OfficerPrisoner> OfficerPrisoners { get; set; }


    }  
}
