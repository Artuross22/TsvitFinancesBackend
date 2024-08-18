namespace Infrastructure;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExexpiresHours { get; set; }
}
