namespace Models.Classes
{
    [PrimaryKey(nameof(IdAccount))]
    public class AccountRefreshToken
    {
        public string? ClaimKey { get; set; }
        public int IdAccount { get; set; }
    }
}
