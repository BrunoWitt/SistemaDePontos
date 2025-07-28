using System.ComponentModel.DataAnnotations;
using System;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Collections;

public class Program
{
    public static void Main(string[] args)
    {
        using (var connection = Database.GetConnection())
        {
            connection.Open();
            Console.WriteLine("Conexão bem sucedida");

            string email = "";
            string senha = "";
            string opcao = "";
            int verificacao = 0;
            string mensagem = "";

            Console.WriteLine("Digite seu email:");
            email = Console.ReadLine();
            Console.WriteLine("Digite sua senha:");
            senha = Console.ReadLine();

            Colaborador colaboradorLogado = Controles.Logar(email, senha);
            if (colaboradorLogado != null)
            {
                Console.WriteLine($"Bem vindo, {colaboradorLogado.Nome}!");
                if (colaboradorLogado.Admin)
                {
                    Console.WriteLine("Você é um administrador.");
                }
                else if (colaboradorLogado is not Admin)
                {
                    Console.WriteLine("Você é um colaborador comum.");
                    Console.WriteLine("1. Registrar Ponto \n2. Ver Registro de Ponto");
                    opcao = Console.ReadLine();

                    switch (opcao)
                    {
                        case "1":
                            verificacao = Controles.VerificarStatusDoDia(colaboradorLogado.Id);

                            if (verificacao == 1) { Console.WriteLine("Entrada: ✅"); }
                            if (verificacao == 2) { Console.WriteLine("Inicio de Intervalo: ✅"); }
                            if (verificacao == 3) { Console.WriteLine("Fim de Intervalo: ✅"); }
                            if (verificacao == 4) { Console.WriteLine("Todos os pontos batidos ✅"); }

                            mensagem = Controles.RegistrarPonto(colaboradorLogado.Id, verificacao);
                            Console.WriteLine(mensagem);

                            break;

                        case "2":
                            Controles.VerSeusRegistros(colaboradorLogado.Id);
                            break;
                        case "3":
                            Console.WriteLine("Encerrando o programa...");
                            Environment.Exit(0); // Finaliza o programa imediatamente
                            break;
                        default:
                            Console.WriteLine("Opção inválida.");
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