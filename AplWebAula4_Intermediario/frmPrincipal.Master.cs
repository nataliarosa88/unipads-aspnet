using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace AplWebAula4_Intermediario
{
    public partial class frmPrincipal : System.Web.UI.MasterPage
    {


        public NpgsqlConnection npgConexao = null;
        public NpgsqlCommand npgCmd = new NpgsqlCommand();
        public NpgsqlDataReader npgDados;

        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMenu.Text = Session["s_username"].ToString();

                if (lblMenu.Text == "admin")
                {
                    lblMenu.Text = "<a class='dropdown-item' href='Usuario.aspx'>Usuários</a>";
                } else
                {
                    lblMenu.Text = "";
                }

                npgConexao = new NpgsqlConnection("Server=andretecnologia.com.br;Port=5432;User Id=postgres;Password=postgres;Database=imobiliaria;");
                npgConexao.Open();

                npgCmd.CommandText = "Select * from imoveis";
                NpgsqlCommand cmd = new NpgsqlCommand();
                npgCmd.Connection = npgConexao;

                npgCmd.CommandType = System.Data.CommandType.Text;
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(npgCmd);
                
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);

                GridView2.DataSource = dt;
                GridView2.DataBind();

            }
            catch (NpgsqlException erro)
            {

                lblMensagem.Text = erro.Message.ToString();
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
            txtEndereco.Text = "";
            txtCidade.Text = "";
            txtEstado.Text = "";
            lblMensagem.Text = "";
            txtCodigo.Focus();
        }

        protected void btnConsultar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                String sql = "Select * From imoveis Where i_imoveis = " + txtCodigo.Text;
                NpgsqlCommand npgCmd = new NpgsqlCommand();
                npgCmd.Connection = npgConexao;
                npgCmd.CommandText = sql;
                npgDados = npgCmd.ExecuteReader();
                if (npgDados.HasRows)
                {
                    npgDados.Read();
                    txtCodigo.Text = npgDados["i_imoveis"].ToString();
                    txtEndereco.Text = npgDados["endereco"].ToString();
                    txtCidade.Text = npgDados["cidade"].ToString();
                    txtEstado.Text = npgDados["estado"].ToString();
                }
                else
                {
                    lblMensagem.Text = "Código inexistente !!!";
                    txtCodigo.Focus();
                }
            }
            catch (NpgsqlException erro)
            {
                lblMensagem.Text = erro.Message.ToString();
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

                strSql = "Select * From imoveis Where i_imoveis = " + txtCodigo.Text;



                npgCmd.Connection = npgConexao;
                npgCmd.CommandText = strSql;
                npgDados = npgCmd.ExecuteReader();



                if (!npgDados.HasRows)
                {
                    if (!npgDados.IsClosed) { npgDados.Close(); }

                    strSql = "Insert into imoveis (i_imoveis,endereco,cidade,estado) values (";
                    strSql += txtCodigo.Text + ",";
                    strSql += "'" + txtEndereco.Text + "',";
                    strSql += "'" + txtCidade.Text + "',";
                    strSql += "'" + txtEstado.Text + "')";
                    npgCmd.Connection = npgConexao;
                    npgCmd.CommandText = strSql;
                    npgCmd.ExecuteNonQuery();
                    lblMensagem.Text = "Registro incluído com sucesso !!!";

                    if (!npgDados.IsClosed) { npgDados.Close(); }
                }
                else
                {
                    lblMensagem.Text = "Código Existente !!!";
                    txtCodigo.Focus();
                }
            }
            catch (Exception erro)
            {
                lblMensagem.Text = erro.Message.ToString();
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

                strSql = "Select * From imoveis Where i_imoveis = " + txtCodigo.Text;

                npgCmd.Connection = npgConexao;
                npgCmd.CommandText = strSql;
                npgDados = npgCmd.ExecuteReader();



                if (npgDados.HasRows)
                {
                    if (!npgDados.IsClosed) { npgDados.Close(); }

                    strSql = "Update imoveis set ";
                    strSql += "endereco = '" + txtEndereco.Text + "',";
                    strSql += "cidade = '" + txtCidade.Text + "',";
                    strSql += "estado = '" + txtEstado.Text + "' ";
                    strSql += "Where i_imoveis = " + txtCodigo.Text;
                    npgCmd.Connection = npgConexao;
                    npgCmd.CommandText = strSql;
                    npgCmd.ExecuteNonQuery();
                    lblMensagem.Text = "Registro alterado com sucesso !!!";
                }
                else
                {
                    lblMensagem.Text = "Código Inexistente !!!";
                    txtCodigo.Focus();
                }
            }
            catch (Exception erro)
            {
                lblMensagem.Text = erro.Message.ToString();
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

                strSql = "Select * From imoveis Where i_imoveis = " + txtCodigo.Text;


                npgCmd.Connection = npgConexao;
                npgCmd.CommandText = strSql;
                npgDados = npgCmd.ExecuteReader();



                if (npgDados.HasRows)
                {
                    if (!npgDados.IsClosed) { npgDados.Close(); }
                    strSql = "Delete From imoveis Where i_imoveis = " + txtCodigo.Text;
                    npgCmd.Connection = npgConexao;
                    npgCmd.CommandText = strSql;
                    npgCmd.ExecuteNonQuery();
                    lblMensagem.Text = "Registro excluído com sucesso !!!";
                }
                else
                {
                    lblMensagem.Text = "Código Inexistente !!!";
                    txtCodigo.Focus();
                }
            }
            catch (Exception erro)
            {
                lblMensagem.Text = erro.Message.ToString();
            }
        }
    }
}