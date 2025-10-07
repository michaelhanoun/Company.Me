namespace Dashboard.Dtos
{
    public class UserDetailsDto
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }
    }
}
