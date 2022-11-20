using MD.PersianDateTime.Standard;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.Entity;

public abstract class ApiEntity : IApiEntity
{
    [NotMapped]public PersianDateTime CreationPersianDate => new PersianDateTime(CreatedAt);

    [Display(Name = "تاریخ حذف")] public DateTime RemovedAt { get; set; }

    [Display(Name = "تاریخ ساخت")] public DateTime CreatedAt { get; set; }

    [Display(Name = "ساخته شده توسط")] public string CreatedBy { get; set; }

    [Display(Name = "حذف شده")] public bool IsRemoved { get; set; }

    [Display(Name = "اخرین تغییر توسط")] public string ModifiedBy { get; set; }

    [Display(Name = "حذف شده توسط")] public string RemovedBy { get; set; }

    public DateTime ModifiedAt { get; set; }
}