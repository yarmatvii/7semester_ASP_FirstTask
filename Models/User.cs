namespace _7semester_ASP_FirstTask.Models
{
    public class User
    {
        public User(string name, string login, string password, string country)
        {
            Name = name;
            this.login = login;
            this.password = password;
            Country = country;
        }

        public User() { }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? login { get; set; }
        public string? password { get; set; }
        public string? Country { get; set; }
    }
}
