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
    public partial class CadastroManobrista : System.Web.UI.Page
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

            var classe = (Manobrista)Session["classe"];
            classe.Nome = txtNome.Text;
            classe.Cpf = txtCpf.Text;
            classe.Nascimento = Convert.ToDateTime(txtNascimento.Text);

            if(classe.Id == 0)
            {
                var parametros = new List<OleDbParameter>();
                parametros.Add(new OleDbParameter("Cpf", classe.Cpf));
                if (new ManobristaController().GetAll(" AND Cpf = ?",parametros).Count > 0)
                {
                    MessageBox.Show("CPF já cadastrado no sistema");
                    return;
                }
            }

            if (new ManobristaController().Salvar(classe))
            {
                MessageBox.ShowAndRedirect("Salvo com sucesso!", "ConsultaManobrista.aspx");
                return;
            }

            MessageBox.Show("Não foi possível salvar! Verifique os dados");
        }

        #endregion

        #region MÉTODOS AUXILIARES


        /// <summary>
        /// Verifica a validade de um cpf
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static bool isCPF(string valor)
        {
            if (valor == "")
                return false;

            string cpf = valor;

            int d1, d2;
            int soma = 0;
            string digitado = "";
            string calculado = "";

            cpf = cpf.Replace(".", "").Replace("-", "");
            // Pesos para calcular o primeiro digito
            int[] peso1 = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            // Pesos para calcular o segundo digito
            int[] peso2 = new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] n = new int[11];

            // Se o tamanho for < 11 entao retorna como inválido
            if (cpf.Length != 11)
                return false;

            // Caso coloque todos os numeros iguais
            switch (cpf)
            {
                case "11111111111":
                    return false;
                case "00000000000":
                    return false;
                case "2222222222":
                    return false;
                case "33333333333":
                    return false;
                case "44444444444":
                    return false;
                case "55555555555":
                    return false;
                case "66666666666":
                    return false;
                case "77777777777":
                    return false;
                case "88888888888":
                    return false;
                case "99999999999":
                    return false;
            }

            try
            {

                // Quebra cada digito do CPF
                n[0] = Convert.ToInt32(cpf.Substring(0, 1));
                n[1] = Convert.ToInt32(cpf.Substring(1, 1));
                n[2] = Convert.ToInt32(cpf.Substring(2, 1));
                n[3] = Convert.ToInt32(cpf.Substring(3, 1));
                n[4] = Convert.ToInt32(cpf.Substring(4, 1));
                n[5] = Convert.ToInt32(cpf.Substring(5, 1));
                n[6] = Convert.ToInt32(cpf.Substring(6, 1));
                n[7] = Convert.ToInt32(cpf.Substring(7, 1));
                n[8] = Convert.ToInt32(cpf.Substring(8, 1));
                n[9] = Convert.ToInt32(cpf.Substring(9, 1));
                n[10] = Convert.ToInt32(cpf.Substring(10, 1));
            }
            catch
            {
                return false;
            }

            // Calcula cada digito com seu respectivo peso
            for (int i = 0; i <= peso1.GetUpperBound(0); i++)
                soma += (peso1[i] * Convert.ToInt32(n[i]));

            // Pega o resto da divisao
            int resto = soma % 11;

            if (resto == 1 || resto == 0)
                d1 = 0;
            else
                d1 = 11 - resto;

            soma = 0;

            // Calcula cada digito com seu respectivo peso
            for (int i = 0; i <= peso2.GetUpperBound(0); i++)
                soma += (peso2[i] * Convert.ToInt32(n[i]));

            // Pega o resto da divisao
            resto = soma % 11;
            if (resto == 1 || resto == 0)
                d2 = 0;
            else
                d2 = 11 - resto;

            calculado = d1.ToString() + d2.ToString();
            digitado = n[9].ToString() + n[10].ToString();

            // Se os ultimos dois digitos calculados bater com
            // os dois ultimos digitos do cpf entao é válido
            if (calculado == digitado)
                return (true);
            else
                return (false);
        }

        /// <summary>
        /// Carrega informações do Manobrista
        /// </summary>
        /// <param name="id">Id do manobrista quando existe</param>
        public void CarregarCampos(int id = 0)
        {
            var classe = new Manobrista();
            if (id > 0)
            {
                classe = new ManobristaController().GetSingle(id);
                txtNome.Text = classe.Nome;
                txtCpf.Text = classe.Cpf;
                txtNascimento.Text = classe.NascimentoFormatado;
                txtCpf.Enabled = false;
            }
            Session["classe"] = classe;
        }

        /// <summary>
        /// Valida campos de entrada
        /// </summary>
        /// <returns></returns>
        public string ValidaCampos()
        {
            if (txtNome.Text.Trim() == "")
            {
                txtNome.Focus();
                return "Digite o nome!";
            }
            if (!isCPF(txtCpf.Text.Trim()))
            {
                txtCpf.Focus();
                return "Digite o CPF corretamente!";
            }
            var data = DateTime.MinValue;
            if (txtNascimento.Text == "" || !DateTime.TryParse(txtNascimento.Text,out data) || data >= DateTime.Now.AddYears(-18))
            {
                return "Digite a data corretamente";
            }
            return "";
        }

        #endregion

    }
}