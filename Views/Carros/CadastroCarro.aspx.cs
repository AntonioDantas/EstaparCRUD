using EstaparCRUD.Classes;
using EstaparCRUD.Controllers;
using EstaparCRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EstaparCRUD.Views.Carros
{
    public partial class CadastroCarro : System.Web.UI.Page
    {
        #region LOADS E INITS

        /// <summary>
        /// Método de carregamento da página de cadastro ou alteração quando houver string na URL
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString.Count > 0)
                {
                    btnCadastrar.CssClass = "btn btn-warning btn-preloader";
                    btnCadastrar.Text = "<span class='glyphicon glyphicon-pencil'></span> ALTERAR";
                    CarregarCampos(Convert.ToInt32(Request.QueryString["id"]));
                }
                else
                {
                    btnCadastrar.CssClass = "btn btn-success btn-preloader";
                    btnCadastrar.Text = "<span class='glyphicon glyphicon-floppy-saved'></span> CADASTRAR";
                    CarregarCampos();
                }
            }
        }

        #endregion

        #region BOTÕES CLIQUES

        /// <summary>
        /// Realiza o cadastro/alteração
        /// </summary>
        protected void btnCadastrar_Click(object sender, EventArgs e)
        {
            var resultado = ValidaCampos();
            if (resultado != "")
            {
                MessageBox.Show(resultado);
                return;
            }

            var classe = (Carro)Session["classe"];
            classe.Placa = txtPlaca.Text;
            classe.Modelo = new CarroController().GetSingleModelo(Convert.ToInt32(ddlModelos.SelectedValue));

            if (classe.Id == 0)
            {
                var parametros = new List<OleDbParameter>();
                parametros.Add(new OleDbParameter("Placa", classe.Placa));
                if (new CarroController().GetAll(" AND Placa = ?", parametros).Count > 0)
                {
                    MessageBox.Show("Placa já cadastrada no sistema");
                    return;
                }
            }

            if (new CarroController().Salvar(classe))
            {
                MessageBox.ShowAndRedirect("Salvo com sucesso!", "ConsultaCarro.aspx");
                return;
            }

            MessageBox.Show("Não foi possível salvar! Verifique os dados");
        }

        #endregion

        #region MÉTODOS AUXILIARES

        /// <summary>
        /// Carrega todas as marcas
        /// </summary>
        protected void ddlMarcas_Init(object sender, EventArgs e)
        {
            ddlMarcas.DataSource = new CarroController().GetAllMarcas("", new List<System.Data.OleDb.OleDbParameter>());
            ddlMarcas.DataValueField = "Id";
            ddlMarcas.DataTextField = "Marca";
            ddlMarcas.DataBind();
            ddlMarcas_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// Carrega todos os modelos
        /// </summary>
        protected void ddlMarcas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ddlMarcas.SelectedValue.Equals(""))
            {
                var par = new List<System.Data.OleDb.OleDbParameter>();
                par.Add(new OleDbParameter("IdMarca", ddlMarcas.SelectedValue));
                var modelos = new CarroController().GetAllModelos(" AND IdMarca = ? ", par);
                ddlModelos.DataSource = modelos;
                ddlModelos.DataValueField = "Id";
                ddlModelos.DataTextField = "Modelo";
                ddlModelos.DataBind();
            }
        }

        /// <summary>
        /// Carrega informações do Carro
        /// </summary>
        /// <param name="id">Id do Carro quando existe</param>
        public void CarregarCampos(int id = 0)
        {
            var classe = new Carro();
            if (id > 0)
            {
                classe = new CarroController().GetSingle(id);
                txtPlaca.Text = classe.Placa;
                ddlMarcas.SelectedValue = classe.Modelo.Marca.Id.ToString();
                ddlMarcas_SelectedIndexChanged(null, null);
                ddlModelos.SelectedValue = classe.Modelo.Id.ToString();
                txtPlaca.Enabled = false;
            }
            Session["classe"] = classe;
        }

        /// <summary>
        /// Valida campos de entrada
        /// </summary>
        /// <returns></returns>
        public string ValidaCampos()
        {
            if (txtPlaca.Text.Trim() == "" || txtPlaca.Text.Length < 7)
            {
                txtPlaca.Focus();
                return "Digite a placa!";
            }
            if (ddlModelos.SelectedValue.Equals(""))
            {
                ddlModelos.Focus();
                return "Escolha o modelo!";
            }            
            return "";
        }

        #endregion
    }
}