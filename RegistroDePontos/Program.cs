using System.ComponentModel.DataAnnotations;
using System;
using System.Data.SqlClient;
using Microsoft.VisualBasic;

public class Colaborador
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

public class RegistroPonto
{
    public int Id { get; set; }
    public int ColaboradorId { get; set; }
    public DateTime DataRegistro { get; set; }
    public DateTime Entrada { get; set; }
    public DateTime? InicioIntervalo { get; set; }
    public DateTime? FimIntervalo { get; set; }
    public DateTime? Saida { get; set; }
}

class Admin : Colaborador
{
    public Admin(int id, string nome, string email, string senha) : base(id, nome, email, senha)
    {
        Admin = "true";
    }
}





public class Program
{
    static List<Colaborador> colaboradores = new List<Colaborador>
    {
        new Colaborador(1, "Colaborador 1", "colaborador@gmail.com", "123"),
        new Colaborador(2, "Colaborador 2", "colaborador@gmail.com", "456"),
    };
    static List<Admin> admins = new List<Admin>
    {
        new Admin(1, "Admin", "admin@gmail.com", "admin123"),
    };

    static int ObterStatusAtualDoRegistro(SqlConnection connection, int colaboradorId, DateTime dataRegistro)
{
    string query = "SELECT Entrada, InicioIntervalo, FimIntervalo, Saida FROM RegistrosPonto WHERE ID = @ID AND DataRegistro = @DataRegistro";
    SqlCommand command = new SqlCommand(query, connection);
    command.Parameters.AddWithValue("@ID", colaboradorId);
    command.Parameters.AddWithValue("@DataRegistro", dataRegistro);

    SqlDataReader reader = command.ExecuteReader();
    int status = 0;

    if (reader.Read())
    {
        if (reader["Entrada"] != DBNull.Value) status++;
        if (reader["InicioIntervalo"] != DBNull.Value) status++;
        if (reader["FimIntervalo"] != DBNull.Value) status++;
        if (reader["Saida"] != DBNull.Value) status++;
    }

    reader.Close();
    return status;
}

    public static void Main(string[] args)
    {

        string connectionString = "Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string email = "";
            string opc = "";
            

            while (true)
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
                    var ColaboradorLogado = colaboradores.First(c => c.Email == email && c.Senha == senha);
                    Console.WriteLine("Bem-vindo, Colaborador! \n Opções: \n 1. Registrar ponto \n 2. Verificar pontos \n 3. Sair");
                    opc = Console.ReadLine();
                    

                    switch (opc)
                    {
                        case "1":
                            DateTime hora_completa = DateTime.Now;
                            DateTime hoje = DateTime.Today;
                            TimeSpan hora = new TimeSpan(hora_completa.Hour, hora_completa.Minute, 0);
                            int verificacao = ObterStatusAtualDoRegistro(connection, ColaboradorLogado.Id, hoje);


                            if (verificacao == 1) { Console.WriteLine("Entrada: Batido"); }
                            if (verificacao == 2) { Console.WriteLine("Inicio de Intervalo: Batido"); }
                            if (verificacao == 3) { Console.WriteLine("Fim de intervalo: Batido"); }
                            if (verificacao == 4) { Console.WriteLine("Saída: Batido"); }
                            if (verificacao == 0)
                            {
                                string comando = "INSERT INTO RegistrosPonto(FuncionarioId, DataRegistro, Entrada) VALUES (@ID, @Data_Hoje, @Entrada)";
                                SqlCommand command = new SqlCommand(comando, connection);
                                command.Parameters.AddWithValue("@ID", ColaboradorLogado.Id);
                                command.Parameters.AddWithValue("@Data_Hoje", hoje);
                                command.Parameters.AddWithValue("@Entrada", hora);
                                command.ExecuteNonQuery();
                                Console.WriteLine("Ponto registrado com sucesso!");

                                verificacao++;
                                break;
                            }
                            else if (verificacao == 1)
                            {
                                string comando = "UPDATE RegistrosPonto SET InicioIntervalo = @InicioIntervalo WHERE ID = @ID AND DataRegistro = @DataRegistro";
                                SqlCommand command = new SqlCommand(comando, connection);
                                command.Parameters.AddWithValue("@ID", ColaboradorLogado.Id);
                                command.Parameters.AddWithValue("@InicioIntervalo", hora);
                                command.Parameters.AddWithValue("@DataRegistro", hoje);
                                command.ExecuteNonQuery();
                                Console.WriteLine("Início de intervalo registrado com sucesso!");

                                verificacao++;
                                break;
                            }
                            else if (verificacao == 2)
                            {
                                string comando = "UPDATE RegistrosPonto SET FimIntervalo = @FimIntervalo WHERE ID = @ID AND DataRegistro = @DataRegistro";
                                SqlCommand command = new SqlCommand(comando, connection);
                                command.Parameters.AddWithValue("@ID", ColaboradorLogado.Id);
                                command.Parameters.AddWithValue("@FimIntervalo", hora);
                                command.Parameters.AddWithValue("@DataRegistro", hoje);
                                command.ExecuteNonQuery();
                                Console.WriteLine("Fim de intervalo registrado com sucesso!");

                                verificacao++;
                                break;
                            }
                            else if (verificacao == 3)
                            {
                                string comando = "UPDATE RegistrosPonto SET Saida = @Saida WHERE ID = @ID AND DataRegistro = @DataRegistro";
                                SqlCommand command = new SqlCommand(comando, connection);
                                command.Parameters.AddWithValue("@ID", ColaboradorLogado.Id);
                                command.Parameters.AddWithValue("@Saida", hora);
                                command.Parameters.AddWithValue("@DataRegistro", hoje);
                                command.ExecuteNonQuery();
                                Console.WriteLine("Saída registrada com sucesso!");

                                verificacao++;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Todas as etapas já foram registradas para hoje.");
                                break;
                            }
                        case "2":
                            string comando2 = "SELECT * FROM RegistrosPonto WHERE ID = @ID";
                            SqlCommand command2 = new SqlCommand(comando2, connection);
                            command2.Parameters.AddWithValue("@ID", ColaboradorLogado.Id);
                            SqlDataReader reader = command2.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine($"Data: {reader["DataRegistro"]}, Entrada: {reader["Entrada"]}, Início Intervalo: {reader["InicioIntervalo"]}, Fim Intervalo: {reader["FimIntervalo"]}, Saída: {reader["Saida"]}");

                                }
                            }
                            break;
                        case "3":
                            Console.WriteLine("Saindo...");
                            break;



                    }
                }
                else
                {
                    Console.WriteLine("Email ou senha inválidos.");
                }

            }





        }
    }
}