namespace Domain.Entities;
public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }

    [NotMapped]
    public string FullName => FirstName + " " + LastName;
}