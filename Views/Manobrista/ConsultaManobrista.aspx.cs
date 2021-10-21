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

namespace EstaparCRUD.Views
{
    public partial class ConsultaManobrista : System.Web.UI.Page
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
            grdResultado.DataSource = (List<Manobrista>)Session["consultaClasse"];
            grdResultado.DataBind();
        }

        /// <summary>
        /// Ordenação dos resultados
        /// </summary>
        protected void grdResultado_Sorting(object sender, GridViewSortEventArgs e)
        {
            var classes = (List<Manobrista>)Session["consultaClasse"];
            var direcao = SiteMaster.GetSortDirection(e.SortExpression);

            MemberInfo[] members = typeof(Manobrista).GetMembers();
            foreach (var i in members)
            {
                if ((i.Name+" ").Split(' ')[0].Equals(e.SortExpression))
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

        /// <summary>
        /// Ação nas linhas dos resultados
        /// </summary>
        protected void grdResultado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "alterar" || e.CommandName == "excluir")
            {
                int index = Convert.ToInt32(e.CommandArgument) + (grdResultado.PageIndex * grdResultado.PageSize);
                var classes = (List<Manobrista>)Session["consultaClasse"];

                switch (e.CommandName)
                {
                    case "alterar":
                        Response.Redirect($"CadastroManobrista.aspx?Id={classes[index].Id}");
                        break;
                    case "excluir":
                        if (new GenericoController().Delete(classes[index].Id, "Manobrista"))
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
            var classes = new ManobristaController().GetAll(filtro, parametros);

            Session["coluna"] = "Nome";
            divResultado.Visible = true;
            grdResultado.DataSource = classes;
            grdResultado.DataBind();
            lblTotal.Text = $"<b>Total de Manobristas cadastrados: </b> {classes.Count}.";
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

            if (txtNome.Text != "")
            {
                filtros += $" AND Nome like ? COLLATE Latin1_General_CI_AI";
                param.Add(new OleDbParameter("Nome", $"%{txtNome.Text}%"));
            }
            if (txtInicio.Text != "")
            {
                filtros += $" AND FLOOR(DATEDIFF(DAY, Nascimento, GETDATE()) / 365.25) >= ?";
                param.Add(new OleDbParameter("Nascimento", txtInicio.Text));
            }
            if (txtFim.Text != "")
            {
                filtros += $" AND FLOOR(DATEDIFF(DAY, Nascimento, GETDATE()) / 365.25) <= ?";
                param.Add(new OleDbParameter("Nascimento", txtFim.Text));
            }
            if (txtCPF.Text != "" && CadastroManobrista.isCPF(txtCPF.Text))
            {
                filtros += $" AND Cpf = ?";
                param.Add(new OleDbParameter("Cpf", txtCPF.Text));
            }
            return (filtros, param);
        }

        #endregion
    }
}