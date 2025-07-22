using System.ComponentModel.DataAnnotations;
using System;
using System.Data.SqlClient;

class Colaborador
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }

    public string Admin { get; set; } = "false";

    public Colaborador(int id, string nome, string email, string senha)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Senha = senha;
        Admin = "false";
    }

}

class Admin: Colaborador
{
    public Admin(int id, string nome, string email, string senha) : base(id, nome, email, senha)
    {
        Admin = "true";
    }
}

public class Program
{
    static List<Colaborador> colaboradores = new List<Colaborador>();
    static List<Admin> admins = new List<Admin>
    {
        new Admin(1, "Admin", "admin@gmail.com", "admin123"),
    };

    public static void Main(string[] args)
    {
        string email = "";
        do
        {
            Console.WriteLine("Login, Insira seu email:");
            email = Console.ReadLine();
            Console.WriteLine("Insira sua senha:");
            string senha = Console.ReadLine();
            if (admins.Any(a => a.Email == email && a.Senha == senha))
            {
                Console.WriteLine("Bem-vindo, Admin!");
            }
            else if (colaboradores.Any(c => c.Email == email && c.Senha == senha))
            {
                Console.WriteLine("Bem-vindo, Colaborador!");
            }
            else
            {
                Console.WriteLine("Email ou senha inválidos.");
            }

        } while (email != "sair");
    }
}