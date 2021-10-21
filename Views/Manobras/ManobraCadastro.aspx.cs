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

namespace EstaparCRUD.Views.Manobras
{
    public partial class ManobraCadastro : System.Web.UI.Page
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

            var classe = (Manobra)Session["classe"];
            classe.DataHora = DateTime.Now;
            classe.Classificacao = ddlClassificacao.SelectedValue.Equals("1");
            classe.CarroDaManobra = new CarroController().GetSingle(Convert.ToInt32(ddlCarro.SelectedValue));
            classe.ManobristaDaManobra = new ManobristaController().GetSingle(Convert.ToInt32(ddlManobrista.SelectedValue));

            if (classe.Id == 0)
            {
                var parametros = new List<OleDbParameter>();
                parametros.Add(new OleDbParameter("IdCarro", classe.CarroDaManobra.Id));
                var consulta = new ManobraController().GetAll(" AND IdCarro = ?", parametros);
                if (classe.Classificacao) //Entrando
                {
                    if (consulta.Count > 0 && consulta.OrderByDescending(x => x.DataHora).FirstOrDefault().Classificacao == true)
                    {
                        MessageBox.Show("O carro já está dentro do local");
                        return;
                    }
                }
                else //Saindo
                {
                    if (consulta.Count == 0 || consulta.OrderByDescending(x => x.DataHora).FirstOrDefault().Classificacao == false)
                    {
                        MessageBox.Show("O carro não está dentro do local");
                        return;
                    }
                }
                
            }

            if (new ManobraController().Salvar(classe))
            {
                MessageBox.ShowAndRedirect("Salvo com sucesso!", "ManobraCadastro.aspx");
                return;
            }

            MessageBox.Show("Não foi possível salvar! Verifique os dados");
        }

        #endregion

        #region MÉTODOS AUXILIARES



        /// <summary>
        /// Carrega informações do Manobra
        /// </summary>
        /// <param name="id">Id do Manobra quando existe</param>
        public void CarregarCampos(int id = 0)
        {
            var classe = new Manobra();
            if (id > 0)
            {
                classe = new ManobraController().GetSingle(id);
                ddlClassificacao.SelectedValue = classe.Classificacao ? "1" : "0";
                ddlCarro.SelectedValue = classe.CarroDaManobra.Id.ToString();
                ddlManobrista.SelectedValue = classe.ManobristaDaManobra.Id.ToString();
            }
            Session["classe"] = classe;
        }

        /// <summary>
        /// Valida campos de entrada
        /// </summary>
        /// <returns></returns>
        public string ValidaCampos()
        {
            if (ddlCarro.SelectedValue.Equals(""))
            {
                ddlCarro.Focus();
                return "Escolha o Carro!";
            }
            if (ddlManobrista.SelectedValue.Equals(""))
            {
                ddlManobrista.Focus();
                return "Escolha o manobrista!";
            }
            return "";
        }

        #endregion


        protected void ddlManobrista_Init(object sender, EventArgs e)
        {
            ddlManobrista.DataSource = new ManobristaController().GetAll("", new List<System.Data.OleDb.OleDbParameter>());
            ddlManobrista.DataValueField = "Id";
            ddlManobrista.DataTextField = "Nome";
            ddlManobrista.DataBind();
        }

        protected void ddlCarro_Init(object sender, EventArgs e)
        {
            ddlCarro.DataSource = new CarroController().GetAll("", new List<System.Data.OleDb.OleDbParameter>());
            ddlCarro.DataValueField = "Id";
            ddlCarro.DataTextField = "Placa";
            ddlCarro.DataBind();
        }
    }
}