using System.ComponentModel.DataAnnotations;

namespace NetCoreMongoDB.Entities;

public class User : Base
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
    public UserRole Role { get; set; }
}

public enum UserRole
{
    [Display(Name = "Quản trị viên")] Admin = 1,
    [Display(Name = "Khách hàng")] Customer = 2
}