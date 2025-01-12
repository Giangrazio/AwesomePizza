namespace AwesomePizzaBLL.Structure
{
    public class AuthToken
    {

        public int Id { get; set; }
        public string? Denomination { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string LastTokenId { get; set; }
    }

}