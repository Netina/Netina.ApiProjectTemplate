namespace Domain.Entities;
public class ApplicationRole : IdentityRole<int>
{
    public string Description { get; set; }
}