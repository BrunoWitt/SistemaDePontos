using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

public class Controles
{
    #region Método de Login
    public static Colaborador Logar(string email, string senha)
    {
        using (var connection = Database.GetConnection())
        {
            connection.Open();
            string query = "SELECT * FROM Colaboradores WHERE Email = @Email AND Senha = @Senha";
            SqlCommand comando = new SqlCommand(query, connection);
            comando.Parameters.AddWithValue("@Email", email);
            comando.Parameters.AddWithValue("@Senha", senha);
            comando.ExecuteNonQuery();

            using (SqlDataReader reader = comando.ExecuteReader())
            {
                if (reader.Read())
                {
                    int id = reader.GetInt32(reader.GetOrdinal("Id"));
                    string nome = reader.GetString(reader.GetOrdinal("Nome"));
                    string Email = reader.GetString(reader.GetOrdinal("Email"));
                    string Senha = reader.GetString(reader.GetOrdinal("Senha"));
                    bool admin = reader.GetBoolean(reader.GetOrdinal("Admin"));

                    return new Colaborador(id, nome, Email, Senha) { Admin = admin };
                }
            }

        }
        return null; // Retorna null se não encontrar o colaborador
    }
    #endregion

    #region Método de verificação
    public static int VerificarStatusDoDia(int funcionarioid)
    {
        using (var connection = Database.GetConnection())
        {
            connection.Open();

            DateTime DataCompleta = DateTime.Now;
            DateTime Hoje = DataCompleta.Date;
            int verificacao = 0;

            string query = "SELECT * FROM PontoRegistro WHERE FuncionarioId = @funcionarioid AND DataRegistro = @hoje";
            SqlCommand comando = new SqlCommand(query, connection);
            comando.Parameters.AddWithValue("@FuncionarioId", funcionarioid);
            comando.Parameters.AddWithValue("@hoje", Hoje);
            comando.ExecuteNonQuery();

            using (SqlDataReader reader = comando.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (reader["Entrada"] != DBNull.Value) verificacao++;
                    if (reader["InicioIntervalo"] != DBNull.Value) verificacao++;
                    if (reader["FimIntervalo"] != DBNull.Value) verificacao++;
                    if (reader["Saida"] != DBNull.Value) verificacao++;
                }

                reader.Close();
            }
            return verificacao;
        }
    }
    #endregion

    #region Método de Registro de Ponto
    public static string RegistrarPonto(int funcionarioId, int verificacao)
    {
        using (var connection = Database.GetConnection())
        {
            connection.Open();

            DateTime DataCompleta = DateTime.Now;
            DateTime Hoje = DataCompleta.Date;
            TimeSpan Hora = DataCompleta.TimeOfDay;

            if (verificacao == 0)
            {
                string query = "INSERT INTO PontoRegistro(FuncionarioId, DataRegistro, Entrada) VALUES (@id, @hoje, @entrada)";
                SqlCommand comando = new SqlCommand(query, connection);
                comando.Parameters.AddWithValue("@id", funcionarioId);
                comando.Parameters.AddWithValue("hoje", Hoje);
                comando.Parameters.AddWithValue("@entrada", Hora);
                comando.ExecuteNonQuery();
                string mensagem = "Ponto registrado com sucesso!";
                return mensagem;
            }
            else if (verificacao == 1)
            {
                string query = "UPDATE PontoRegistro SET InicioIntervalo = @iniciointervalo WHERE FuncionarioId = @id AND DataRegistro = @hoje";
                SqlCommand comando = new SqlCommand(query, connection);
                comando.Parameters.AddWithValue("@id", funcionarioId);
                comando.Parameters.AddWithValue("@hoje", Hoje);
                comando.Parameters.AddWithValue("@iniciointervalo", Hora);
                comando.ExecuteNonQuery();
                string mensagem = "Ponto registrado com sucesso!";
                return mensagem;
            }
            else if (verificacao == 2)
            {
                string query = "UPDATE PontoRegistro SET FimIntervalo = @fimintervalo WHERE FuncionarioId = @id AND DataRegistro = @hoje";
                SqlCommand comando = new SqlCommand(query, connection);
                comando.Parameters.AddWithValue("@id", funcionarioId);
                comando.Parameters.AddWithValue("@hoje", Hoje);
                comando.Parameters.AddWithValue("@fimintervalo", Hora);
                comando.ExecuteNonQuery();
                string mensagem = "Fim de intervalo registrado com sucesso!";
                return mensagem;
            }
            else if (verificacao == 3)
            {
                string query = "UPDATE PontoRegistro SET Saida = @saida WHERE FuncionarioId = @id AND DataRegistro = @hoje";
                SqlCommand comando = new SqlCommand(query, connection);
                comando.Parameters.AddWithValue("@id", funcionarioId);
                comando.Parameters.AddWithValue("@hoje", Hoje);
                comando.Parameters.AddWithValue("@saida", Hora);
                comando.ExecuteNonQuery();
                string mensagem = "Saida registrada com sucesso!";
                return mensagem;
            }
            else
            {
                string mensagem = "Todos os pontos já batidos";
                return mensagem;
            }
        }
    }
    #endregion
    #region Método para ver seus registros
    public static void VerSeusRegistros(int colaboradorId)
    {
        using (var connection = Database.GetConnection())
        {
            connection.Open();

            string query = "SELECT * FROM PontoRegistro WHERE FuncionarioId = @id";
            SqlCommand comando = new SqlCommand(query, connection);
            comando.Parameters.AddWithValue("@id", colaboradorId);
            comando.ExecuteNonQuery();

            using (SqlDataReader reader = comando.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string DataFormatada = reader["DataRegistro"].ToString();
                        DateTime dataRegistro = DateTime.Parse(DataFormatada);

                        Console.WriteLine(
                            $"Data: {dataRegistro:dd/MM/yyyy}, " +
                            $"Entrada: {FormatarHora(reader["Entrada"])}, " +
                            $"Início Intervalo: {FormatarHora(reader["InicioIntervalo"])}, " +
                            $"Fim Intervalo: {FormatarHora(reader["FimIntervalo"])}, " +
                            $"Saída: {FormatarHora(reader["Saida"])}, " //+
                                                                        //$"Saldo de horas: {reader["HorasTrabalhadas"] ?? "N/A"}"
                        );
                    }
                }
                else
                {
                    Console.WriteLine("Nenhum registro encontrado");
                }
            }
        }
    }
    private static string FormatarHora(object valor)
    {
        if (valor == DBNull.Value) return " - ";
        TimeSpan hora = (TimeSpan)valor;
        return hora.ToString(@"hh\:mm");
    }
    #endregion

}