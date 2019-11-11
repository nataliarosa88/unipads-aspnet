using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AplWebAula4_Intermediario
{
    public partial class Login : System.Web.UI.MasterPage
    {
        public NpgsqlConnection npgConexao = null;
        public NpgsqlCommand npgCmd = new NpgsqlCommand();
        public NpgsqlDataReader npgDados;

        protected void Page_Load(object sender, EventArgs e)
        {
            npgConexao = new NpgsqlConnection("Server=andretecnologia.com.br;Port=5432;User Id=postgres;Password=postgres;Database=imobiliaria;");
            npgConexao.Open();
        }

        protected void btnlogin_Click(object sender, EventArgs e)
        {

            if (txtUsername.Text != string.Empty && txtPassword.Text != string.Empty)

            { 

                        try
                        {
                            String sql = "select * from usuario where nome = '" + txtUsername.Text + "'";
                            NpgsqlCommand npgCmd = new NpgsqlCommand();
                            npgCmd.Connection = npgConexao;
                            npgCmd.CommandText = sql;
                            npgDados = npgCmd.ExecuteReader();
                            if (npgDados.HasRows)
                            {
                                npgDados.Read();
                                Session["s_username"] = npgDados["nome"].ToString();
                                Response.Redirect("Principal.aspx");
                            }
                            else
                            {
                                lblMessage.Text = "Usuário e senha inválidos";
                                txtUsername.Focus();
                            }
                        }
                        catch (NpgsqlException erro)
                        {
                            lblMessage.Text = erro.Message.ToString();
                            Console.WriteLine("Erro de SQL:" + erro.Message);
                        }
                        finally
                        {
                            // if (!npgDados.IsClosed) { npgDados.Close(); }
                        }

            }

            else

            {

                lblMessage.Text = "Please make sure that the username and the password is Correct";

            }
        }

    }
}
