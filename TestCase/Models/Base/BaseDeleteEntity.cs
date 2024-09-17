using System.ComponentModel.DataAnnotations;

namespace TestCase.Models.Base
{
    public interface IBaseDeleteEntity
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }

    public class BaseDeleteEntity : BaseEntity, IBaseDeleteEntity
    {
        public bool IsDeleted { get; set; }

        [Display(Name = "DeletedOn")]
        public DateTime? DeletedOn { get; set; }
    }
}
