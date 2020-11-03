using Payments.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Payments.DAL.Entities
{
    public class Picture : State
    {
        [Key]
        [ForeignKey("ClientProfile")]
        public string Id { get; set; }

        public string Name { get; set; } 

        public byte[] Image { get; set; }

        public virtual ClientProfile ClientProfile { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBlocked { get; set; }
    }
}
