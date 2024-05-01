using System.ComponentModel.DataAnnotations;

namespace DPTaskTracker.Domain.Models;

public enum UserRole
{
    [Display(Name = "Администратор")]
    Admin = 1,

    [Display(Name = "Менеджер")]
    User,
}