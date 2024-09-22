using Model.User;

namespace Model.Roles;

public class Role
{
    public string Name { get; set; } = string.Empty;
    public List<UserModel> Users { get; set; } = new();

    public override string ToString()
    {
        return Name;
    }
}