namespace SharedMessages;

public class UserRegisterEvent
{
    public string UserId {get; set;}
    public string Email {get; set;}
    public DateTime CreatedAt {get; set;}
}
