public class Colaborador
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }

    public bool Admin { get; set; } = false;

    public Colaborador(int id, string nome, string email, string senha)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Senha = senha;
        Admin = false;
    }

}

class Admin : Colaborador
{
    public Admin(int id, string nome, string email, string senha) : base(id, nome, email, senha)
    {
        Admin = true;
    }
}