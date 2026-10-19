namespace USER_DEPS.Entity
{
    public class Personal
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public string FatherName { get; set; } = string.Empty;

        public User? User { get; set; }
    }
}
