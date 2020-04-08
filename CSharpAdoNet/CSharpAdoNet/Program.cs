using System;
using System.Data.SqlClient;
using static System.Console;
namespace CSharpAdoNet
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("=============================== CONTROLE DE CLIENTES =================================\n");
            WriteLine("Selecione uma opção:");
            WriteLine("1 - Listar");
            WriteLine("2 - Cadastrar");
            WriteLine("3 - Editar");
            WriteLine("4 - Excluir");
            WriteLine("5 - Visualizar");
            Write("Opção: ");
            int opc = Convert.ToInt32(ReadLine());
            Clear();
            switch (opc)
            {
                case 1:
                    Title = "Listagem de Clientes";
                    WriteLine("================================================== LISTAGEM DE CLIENTES ====================================");
                    ListarClientes();
                    break;

                case 2:
                    Title = "Novo Cliente";
                    WriteLine("================================================== CADASTRO DE NOVO CLIENTES ====================================");

                    Write("Informe seu nome: ");
                    string nome = ReadLine();
                    Write("Infomre seu email: ");
                    string email = ReadLine();

                    SalvarCliente(nome, email);
                    break;

                case 3:
                    Title = "Alteração de Clientes";
                    WriteLine("================================================== ALTERAÇÃO DE CLIENTES ====================================");

                    ListarClientes();

                    Write("Selecione um cliente pelo ID: ");
                    int idop = Convert.ToInt32(ReadLine());
                    (int _id, string _nome, string _email) = SelecionarCliente(idop);
                    Clear();

                    Title = "Alteração de Clientes" + _nome;
                    WriteLine($"================================= ALTERAÇÃO DE CLIENTES  - {_nome}====================");

                    Write("Informe seu nome: ");
                    nome = ReadLine();
                    Write("Infomre seu email: ");
                    email = ReadLine();

                    nome = nome.Equals("") ? _nome : nome;
                    email = email.Equals("") ? _email : email;

                    SalvarCliente(nome, email, idop);
                    break;

                case 4:
                    Title = "Exclusão de Clientes";
                    WriteLine("============================ EXCLUSÃO DE CLIENTES ==========================");
                    ListarClientes();

                    Write("Selecione um cliente pelo ID: ");
                    idop = Convert.ToInt32(ReadLine());
                    (_id, _nome, _email) = SelecionarCliente(idop);
                    Clear();


                    Title = "Exclusão do Cliente" + _nome;
                    WriteLine($"====================== EXCLUSÃO DO CLIENTE  - {_nome}================");
                    WriteLine("\n\n************** ATENÇÃO ***************\n");
                    Write("Deseja continuar? (S para SIM, N para NÃO): ");
                    string confirmar = ReadLine().ToUpper();

                    if (confirmar.Equals("S"))
                    {
                        DeletarCliente(idop);
                    }

                    break;

                case 5:
                    Title = "Visualização de Clientes";
                    WriteLine("================================================== VER DETALHES DE CLIENTES ====================================");
                    ListarClientes();

                    Write("Selecione um cliente pelo ID: ");
                    idop = Convert.ToInt32(ReadLine());
                    (_id, _nome, _email) = SelecionarCliente(idop);
                    Clear();


                    Title = "Visualização do Cliente" + _nome;
                    WriteLine($"====================== VISUALIZAÇÃO DO CLIENTE  - {_nome}================");

                    WriteLine("ID: {0}", _id);
                    WriteLine("Nome: {0}", _nome);
                    WriteLine("Email: {0}", _email);
                    break;

                default:
                    Title = "Opção Inválida";
                    WriteLine("================================================== SELECIONE UMA OPÇÃO VALIDA ====================================");
                    break;
            }

            ReadLine();
        }
        //LISTAR CLIENTE/CONEXAO
        static void ListarClientes()
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from clientes order by id";

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        WriteLine("ID: {0}", dr["id"]);
                        WriteLine("Nome: {0}", dr["nome"]);
                        WriteLine("--------------------------------------");
                    }
                }

            }

        }
        //SALVAR
        static void SalvarCliente(string nome, string email)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "insert into clientes (nome, email) values (@nome, @email)";
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.ExecuteNonQuery();
            }
        }

        //EDITAR
        static void SalvarCliente(string nome, string email, int id)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "update clientes  set nome = @nome, email = @email where id = @id";
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        //DELETAR
        static void DeletarCliente(int id)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "delete from clientes where id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
        //SELECINAR CLIENTE
        static (int, string, string) SelecionarCliente(int id)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from clientes where id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();
                    return (Convert.ToInt32(dr["id"].ToString()), dr["nome"].ToString(), dr["email"].ToString());
                }

            }
        }

        //CONEXAO
        static string getStringConn()
        {
            string connString = "Server=DESKTOP-U5FER4D\\SQLEXPRESS;Database=CSharpAdoNET;User Id=sa;Password=dev123";
            return connString;
        }

    }
}
