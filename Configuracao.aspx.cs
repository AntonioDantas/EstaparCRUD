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

namespace EstaparCRUD
{
    public partial class Configuracao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ddlMarcas_Init(object sender, EventArgs e)
        {
            ddlMarcas.DataSource = new CarroController().GetAllMarcas("",new List<System.Data.OleDb.OleDbParameter>());
            ddlMarcas.DataValueField = "Id";
            ddlMarcas.DataTextField = "Marca";
            ddlMarcas.DataBind();
            ddlMarcas_SelectedIndexChanged(null, null);
        }

        protected void ddlMarcas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!ddlMarcas.SelectedValue.Equals(""))
            {
                var par = new List<System.Data.OleDb.OleDbParameter>();
                par.Add(new OleDbParameter("IdMarca", ddlMarcas.SelectedValue));
                ddlModelos.DataSource = new CarroController().GetAllModelos(" AND IdMarca = ? ", par);
                ddlModelos.DataValueField = "Id";
                ddlModelos.DataTextField = "Modelo";
                ddlModelos.DataBind();
            }
        }

        protected void btnCadastrarMarca_Click(object sender, EventArgs e)
        {
            if(!txtMarca.Text.Equals(""))
            {
                var parametros = new List<OleDbParameter>();
                parametros.Add(new OleDbParameter("Marca", txtMarca.Text));
                if (new CarroController().GetAllMarcas(" AND Marca = ?  COLLATE Latin1_General_CI_AI", parametros).Count == 0)
                {
                    var marca = new MarcaCarro
                    {
                        Marca = txtMarca.Text.ToUpper()
                    };
                    new CarroController().SalvarMarca(marca);
                    MessageBox.Show("Cadastrado com sucesso!");
                    ddlMarcas_Init(null, null);
                    ddlMarcas_SelectedIndexChanged(null, null);
                    txtMarca.Text = "";
                }
                else
                {
                    MessageBox.Show("Marca já existe");
                }
            }
            else
            {
                MessageBox.Show("Digite a Marca");
            }
        }

        protected void btnCadastrarModelo_Click(object sender, EventArgs e)
        {
            if (!txtModelo.Text.Equals(""))
            {
                if (!ddlMarcas.SelectedValue.Equals(""))
                {
                    var parametros = new List<OleDbParameter>();
                    parametros.Add(new OleDbParameter("idMarca", ddlMarcas.SelectedValue));
                    parametros.Add(new OleDbParameter("Modelo", txtModelo.Text));
                    if (new CarroController().GetAllModelos(" AND idMarca = ? AND Modelo = ?  COLLATE Latin1_General_CI_AI", parametros).Count == 0)
                    {
                        parametros = new List<OleDbParameter>();
                        parametros.Add(new OleDbParameter("id", ddlMarcas.SelectedValue));
                        var modelo = new ModeloCarro
                        {
                            Marca = new CarroController().GetAllMarcas(" AND id = ? COLLATE Latin1_General_CI_AI", parametros).FirstOrDefault(),
                            Modelo = txtModelo.Text.ToUpper(),
                        };
                        new CarroController().SalvarModelo(modelo);
                        MessageBox.Show("Cadastrado com sucesso!");
                        ddlMarcas_SelectedIndexChanged(null, null);
                        txtModelo.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Modelo já existe");
                    }
                }
                else
                {
                    MessageBox.Show("Escolha a Marca");
                }
                }
            else
            {
                MessageBox.Show("Digite o Modelo");
            }
        }

        protected void lnkRemoverMarca_Click(object sender, EventArgs e)
        {
            if (!ddlMarcas.SelectedValue.Equals(""))
            {
                if (new GenericoController().Delete(Convert.ToInt32(ddlMarcas.SelectedValue), "MarcaCarro"))
                {
                    MessageBox.Show("Excluído com sucesso!");
                    ddlMarcas_SelectedIndexChanged(null, null);
                }
                else
                {
                    MessageBox.Show("Não foi possível excluir! Verifique se o registro está sendo utilizado");
                }
            }
        }

        protected void lnkRemoverModelo_Click(object sender, EventArgs e)
        {
            if (!ddlModelos.SelectedValue.Equals(""))
            {
                if (new GenericoController().Delete(Convert.ToInt32(ddlModelos.SelectedValue), "ModeloCarro"))
                {
                    MessageBox.Show("Excluído com sucesso!");
                    ddlMarcas_SelectedIndexChanged(null, null);
                }
                else
                {
                    MessageBox.Show("Não foi possível excluir! Verifique se o registro está sendo utilizado");
                }
            }

        }
    }
}