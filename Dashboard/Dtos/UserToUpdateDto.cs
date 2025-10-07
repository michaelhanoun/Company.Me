namespace Dashboard.Dtos
{
    public class UserToUpdateDto
    {
        public string UserName { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public List<UserRoleDto> UserRoleDtos { get; set; } = new();
    }
}
