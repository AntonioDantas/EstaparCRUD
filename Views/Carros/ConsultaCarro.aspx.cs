using EstaparCRUD.Classes;
using EstaparCRUD.Controllers;
using EstaparCRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EstaparCRUD.Views.Carros
{
    public partial class ConsultaCarro : System.Web.UI.Page
    {
        #region LOADS E INITS
        /// <summary>
        /// Carregamento da página
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region ELEMENTS CHANGES
        /// <summary>
        /// Troca de página
        /// </summary>
        protected void grdResultado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdResultado.PageIndex = e.NewPageIndex;
            grdResultado.DataSource = (List<Carro>)Session["consultaClasse"];
            grdResultado.DataBind();
        }

        /// <summary>
        /// Ordenação dos resultados
        /// </summary>
        protected void grdResultado_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                var classes = (List<Manobrista>)Session["consultaClasse"];
                var direcao = SiteMaster.GetSortDirection(e.SortExpression);

                MemberInfo[] members = typeof(Carro).GetMembers();
                foreach (var i in members)
                {
                    if ((i.Name + " ").Split(' ')[0].Equals(e.SortExpression))
                    {
                        if (direcao == "ASC")
                        {
                            classes = classes.OrderBy(x => i).ToList();
                        }
                        else
                        {
                            classes = classes.OrderByDescending(x => i).ToList();
                        }
                    }
                }
                Session["consultaClasse"] = classes;
                grdResultado.DataSource = classes;
                grdResultado.DataBind();
            }
            catch { }
        }

        /// <summary>
        /// Ação nas linhas dos resultados
        /// </summary>
        protected void grdResultado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "alterar" || e.CommandName == "excluir")
            {
                int index = Convert.ToInt32(e.CommandArgument) + (grdResultado.PageIndex * grdResultado.PageSize);
                var classes = (List<Carro>)Session["consultaClasse"];

                switch (e.CommandName)
                {
                    case "alterar":
                        Response.Redirect($"CadastroCarro.aspx?Id={classes[index].Id}");
                        break;
                    case "excluir":
                        if (new GenericoController().Delete(classes[index].Id, "Carro"))
                        {
                            MessageBox.Show("Excluído com sucesso!");
                            btnPesquisar_Click(null, null);
                        }
                        else
                        {
                            MessageBox.Show("Não foi possível excluir! Verifique se o registro está sendo utilizado");
                        }
                        break;
                }
            }
        }
        #endregion

        #region BOTÕES CLIQUES
        /// <summary>
        /// Pesquisa de Manobrista
        /// </summary>
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            (var filtro, var parametros) = CarregarFiltros();
            var classes = new CarroController().GetAll(filtro, parametros);

            Session["coluna"] = "Placa";
            divResultado.Visible = true;
            grdResultado.DataSource = classes;
            grdResultado.DataBind();
            lblTotal.Text = $"<b>Total de Carros cadastrados: </b> {classes.Count}.";
            Session["consultaClasse"] = classes;
        }
        #endregion

        #region MÉTODOS AUXILIARES
        /// <summary>
        /// Verifica filtros elegíveis e define os parâmetros
        /// </summary>
        /// <returns></returns>
        public (string, List<OleDbParameter>) CarregarFiltros()
        {
            var filtros = "";
            var param = new List<OleDbParameter>();

            if (txtPlaca.Text != "")
            {
                filtros += $" AND Placa like ? COLLATE Latin1_General_CI_AI";
                param.Add(new OleDbParameter("Placa", $"%{txtPlaca.Text}%"));
            }

            if (ddlModelos.SelectedIndex > 0)
            {
                filtros += $" AND IdModelo = ?";
                param.Add(new OleDbParameter("IdModelo", $"{ddlModelos.SelectedValue}"));
            }

            if (ddlMarcas.SelectedIndex > 0)
            {
                filtros += $" AND marcacarro.Id = ?";
                param.Add(new OleDbParameter("marcacarro.Id", $"{ddlMarcas.SelectedValue}"));
            }

            return (filtros, param);
        }


        /// <summary>
        /// Carrega todas as marcas
        /// </summary>
        protected void ddlMarcas_Init(object sender, EventArgs e)
        {
            var lista = new List<MarcaCarro>();
            lista.Add(new MarcaCarro { Marca = "TODOS" });
            var marcas = new CarroController().GetAllMarcas("", new List<System.Data.OleDb.OleDbParameter>());
            foreach (var m in marcas)
            {
                lista.Add(m);
            }
            ddlMarcas.DataSource = lista;
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
            var lista = new List<ModeloCarro>();
            lista.Add(new ModeloCarro { Modelo = "TODOS" });

            if (ddlMarcas.SelectedIndex > 0)
            {
                var par = new List<System.Data.OleDb.OleDbParameter>();
                par.Add(new OleDbParameter("IdMarca", ddlMarcas.SelectedValue));
                var modelos = new CarroController().GetAllModelos(" AND IdMarca = ? ", par);
                foreach (var m in modelos)
                {
                    lista.Add(m);
                }
            }
            ddlModelos.DataSource = lista;
            ddlModelos.DataValueField = "Id";
            ddlModelos.DataTextField = "Modelo";
            ddlModelos.DataBind();
        }

        #endregion
    }
}