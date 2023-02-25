public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfileImage { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    private User(
        Guid id,
        string firstName,
        string lastName,
        string profileImage,
        string email,
        string password)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        ProfileImage = profileImage;
        Email = email;
        Password = password;
    }

    public static User Create(
        string firstName,
        string lastName,
        string profileImage,
        string email,
        string password)
    {
        return new(
            Guid.NewGuid(),
            firstName,
            lastName,
            profileImage,
            email,
            password);
    }
}