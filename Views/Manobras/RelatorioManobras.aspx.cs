using EstaparCRUD.Classes;
using EstaparCRUD.Controllers;
using EstaparCRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EstaparCRUD.Views.Manobras
{
    public partial class RelatorioManobras : System.Web.UI.Page
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
            grdResultado.DataSource = (List<Manobra>)Session["consultaClasse"];
            grdResultado.DataBind();
        }

        /// <summary>
        /// Ordenação dos resultados
        /// </summary>
        protected void grdResultado_Sorting(object sender, GridViewSortEventArgs e)
        {
            var classes = (List<Manobra>)Session["consultaClasse"];
            var direcao = SiteMaster.GetSortDirection(e.SortExpression);

            MemberInfo[] members = typeof(Manobra).GetMembers();
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

        /// <summary>
        /// Ação nas linhas dos resultados
        /// </summary>
        protected void grdResultado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "alterar" || e.CommandName == "excluir")
            {
                int index = Convert.ToInt32(e.CommandArgument) + (grdResultado.PageIndex * grdResultado.PageSize);
                var classes = (List<Manobra>)Session["consultaClasse"];

                switch (e.CommandName)
                {
                    case "alterar":
                        Response.Redirect($"ManobraCadastro.aspx?Id={classes[index].Id}");
                        break;
                    case "excluir":
                        if (new GenericoController().Delete(classes[index].Id, "Manobra"))
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
        /// Pesquisa de Manobra
        /// </summary>
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            (var filtro, var parametros) = CarregarFiltros();
            var classes = new ManobraController().GetAll(filtro, parametros);

            Session["coluna"] = "DataHora";
            divResultado.Visible = true;
            grdResultado.DataSource = classes;
            grdResultado.DataBind();
            lblTotal.Text = $"<b>Total de Manobras cadastrados: </b> {classes.Count}.";
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
            DateTime data = DateTime.MinValue;

            if (DateTime.TryParse(txtData.Text,out data))
            {
                filtros += $" AND DataHora = ? ";
                param.Add(new OleDbParameter("DataHora", $"%{txtData.Text}%"));
            }

            if (DateTime.TryParse(txtDe.Text, out data))
            {
                filtros += $" AND DataHora >= ? ";
                param.Add(new OleDbParameter("DataHora", $"%{txtDe.Text}%"));
            }

            if (DateTime.TryParse(txtAte.Text, out data))
            {
                filtros += $" AND DataHora <= ? ";
                param.Add(new OleDbParameter("DataHora", $"%{txtAte.Text}%"));
            }

            if (ddlCarro.SelectedIndex > 0)
            {
                filtros += $" AND IdCarro = ?";
                param.Add(new OleDbParameter("IdCarro", $"%{ddlCarro.SelectedValue}%"));
            }

            if (ddlManobrista.SelectedIndex > 0)
            {
                filtros += $" AND IdManobrista = ?";
                param.Add(new OleDbParameter("IdManobrista", $"%{ddlManobrista.SelectedValue}%"));
            }

            return (filtros, param);
        }


        protected void ddlCarro_Init(object sender, EventArgs e)
        {
            var lista = new List<Carro>();
            lista.Add(new Carro { Placa = "TODOS" });
            var marcas = new CarroController().GetAll("", new List<System.Data.OleDb.OleDbParameter>());
            foreach (var m in marcas)
            {
                lista.Add(m);
            }
            ddlCarro.DataSource = lista;
            ddlCarro.DataValueField = "Id";
            ddlCarro.DataTextField = "Placa";
            ddlCarro.DataBind();
        }

        protected void ddlManobrista_Init(object sender, EventArgs e)
        {
            var lista = new List<Manobrista>();
            lista.Add(new Manobrista { Nome = "TODOS" });
            var marcas = new ManobristaController().GetAll("", new List<System.Data.OleDb.OleDbParameter>());
            foreach (var m in marcas)
            {
                lista.Add(m);
            }
            ddlManobrista.DataSource = lista;
            ddlManobrista.DataValueField = "Id";
            ddlManobrista.DataTextField = "Nome";
            ddlManobrista.DataBind();
        }

        #endregion

        protected void btnRelatorio_Click(object sender, EventArgs e)
        {
            GridView grid = new GridView();
            for(int i = 0; i<  grdResultado.Columns.Count-2;i++)
            {
                grid.Columns.Add(grdResultado.Columns[i]);
            }
            grid.AutoGenerateColumns = false;
            grid.DataSource = (List<Manobra>)Session["consultaClasse"];
            grid.DataBind();

            Response.Clear();
            Response.AddHeader("content-disposition",
                string.Format("attachment;filename={0}.xls", "Relatorio_" + DateTime.Now.ToShortDateString().Replace("/", "") + "_" + DateTime.Now.ToShortTimeString().Replace(":", "")));
            Response.AddHeader("Content-Type", "text/html; charset=utf-8");
            Response.AddHeader("Pragma", "no-cache");
            Response.Charset = "utf-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            Response.ContentType = "application/vnd.xls";

            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            grid.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();

        }
    }
}
