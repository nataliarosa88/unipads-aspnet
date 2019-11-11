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
    public partial class Usuario : System.Web.UI.MasterPage
    {
        public NpgsqlConnection npgConexao = null;
        public NpgsqlCommand npgCmd = new NpgsqlCommand();
        public NpgsqlDataReader npgDados;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                npgConexao = new NpgsqlConnection("Server=andretecnologia.com.br;Port=5432;User Id=postgres;Password=postgres;Database=imobiliaria;");
                npgConexao.Open();

                npgCmd.CommandText = "Select * from usuario";
                NpgsqlCommand cmd = new NpgsqlCommand();
                npgCmd.Connection = npgConexao;

                npgCmd.CommandType = System.Data.CommandType.Text;
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(npgCmd);

                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);

                GridView.DataSource = dt;
                GridView.DataBind();

            }
            catch (NpgsqlException erro)
            {

                lblMensagem3.Text = erro.Message.ToString();
                Console.WriteLine("Erro de conexão:" + erro.Message);
            }
        }

        protected void GridView_PreRender(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;
            if ((gv.ShowHeader == true && gv.Rows.Count > 0)
                || (gv.ShowHeaderWhenEmpty == true))
            {
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (gv.ShowFooter == true && gv.Rows.Count > 0)
            {
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void btnLimpar_Click(object sender, ImageClickEventArgs e)
        {
            txtCodigo.Text = "";
            txtNome.Text = "";
            txtEmail.Text = "";
            txtTelefone.Text = "";
            lblMensagem3.Text = "";
            txtCodigo.Focus();
        }

        protected void btnConsultar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                String sql = "Select * From usuario Where id = " + txtCodigo.Text;
                NpgsqlCommand npgCmd = new NpgsqlCommand();
                npgCmd.Connection = npgConexao;
                npgCmd.CommandText = sql;
                npgDados = npgCmd.ExecuteReader();
                if (npgDados.HasRows)
                {
                    npgDados.Read();
                    txtCodigo.Text = npgDados["id"].ToString();
                    txtNome.Text = npgDados["nome"].ToString();
                    txtEmail.Text = npgDados["email"].ToString();
                    txtTelefone.Text = npgDados["telefone"].ToString();
                }
                else
                {
                    lblMensagem3.Text = "Código inexistente !!!";
                    txtCodigo.Focus();
                }
            }
            catch (NpgsqlException erro)
            {
                lblMensagem3.Text = erro.Message.ToString();
                Console.WriteLine("Erro de SQL:" + erro.Message);
            }
            finally
            {
                if (!npgDados.IsClosed) { npgDados.Close(); }
            }

        }

        protected void btnCadastrar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string strSql;

                strSql = "Select * From usuario Where id = " + txtCodigo.Text;



                npgCmd.Connection = npgConexao;
                npgCmd.CommandText = strSql;
                npgDados = npgCmd.ExecuteReader();



                if (!npgDados.HasRows)
                {
                    if (!npgDados.IsClosed) { npgDados.Close(); }

                    strSql = "Insert into usuario (id,nome,email,telefone) values (";
                    strSql += txtCodigo.Text + ",";
                    strSql += "'" + txtNome.Text + "',";
                    strSql += "'" + txtEmail.Text + "',";
                    strSql += "'" + txtTelefone.Text + "')";
                    npgCmd.Connection = npgConexao;
                    npgCmd.CommandText = strSql;
                    npgCmd.ExecuteNonQuery();
                    lblMensagem3.Text = "Registro incluído com sucesso !!!";

                    if (!npgDados.IsClosed) { npgDados.Close(); }
                }
                else
                {
                    lblMensagem3.Text = "Código Existente !!!";
                    txtCodigo.Focus();
                }
            }
            catch (Exception erro)
            {
                lblMensagem3.Text = erro.Message.ToString();
            }
            finally
            {
                if (!npgDados.IsClosed) { npgDados.Close(); }
            }
        }

        protected void btnAlterar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string strSql;

                strSql = "Select * From usuario Where id = " + txtCodigo.Text;

                npgCmd.Connection = npgConexao;
                npgCmd.CommandText = strSql;
                npgDados = npgCmd.ExecuteReader();



                if (npgDados.HasRows)
                {
                    if (!npgDados.IsClosed) { npgDados.Close(); }

                    strSql = "Update usuario set ";
                    strSql += "nome = '" + txtNome.Text + "',";
                    strSql += "email = '" + txtEmail.Text + "',";
                    strSql += "telefone = '" + txtTelefone.Text + "' ";
                    strSql += "Where id = " + txtCodigo.Text;
                    npgCmd.Connection = npgConexao;
                    npgCmd.CommandText = strSql;
                    npgCmd.ExecuteNonQuery();
                    lblMensagem3.Text = "Registro alterado com sucesso !!!";
                }
                else
                {
                    lblMensagem3.Text = "Código Inexistente !!!";
                    txtCodigo.Focus();
                }
            }
            catch (Exception erro)
            {
                lblMensagem3.Text = erro.Message.ToString();
            }
            finally
            {
                if (!npgDados.IsClosed) { npgDados.Close(); }
            }
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string strSql;

                strSql = "Select * From usuario Where id = " + txtCodigo.Text;


                npgCmd.Connection = npgConexao;
                npgCmd.CommandText = strSql;
                npgDados = npgCmd.ExecuteReader();



                if (npgDados.HasRows)
                {
                    if (!npgDados.IsClosed) { npgDados.Close(); }
                    strSql = "Delete From usuario Where id = " + txtCodigo.Text;
                    npgCmd.Connection = npgConexao;
                    npgCmd.CommandText = strSql;
                    npgCmd.ExecuteNonQuery();
                    lblMensagem3.Text = "Registro excluído com sucesso !!!";
                }
                else
                {
                    lblMensagem3.Text = "Código Inexistente !!!";
                    txtCodigo.Focus();
                }
            }
            catch (Exception erro)
            {
                lblMensagem3.Text = erro.Message.ToString();
            }
        }



















    }
}