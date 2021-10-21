using EstaparCRUD.Classes;
using EstaparCRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Data;
using System.Web;

namespace EstaparCRUD.Controllers
{
    /// <summary>
    /// Controle CRUD do carro
    /// </summary>
    public class ManobristaController
    {
        /// <summary>
        /// Retorna todos os manobristas
        /// </summary>
        /// <param name="filtros">filtro</param>
        /// <param name="parameters">parâmetros do filtro</param>
        /// <returns>Lista de Manobristas</returns>
        public List<Manobrista> GetAll(string filtros, List<OleDbParameter> parameters)
        {
            var list = new List<Manobrista>();
            var select = $"SELECT * FROM Manobrista WHERE (1=1) {filtros}";
            try
            {
                var dt = new ConnectionFactory().ExecuteToDataTable(select, parameters);
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Manobrista
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nome = dr["Nome"].ToString(),
                        Cpf = dr["Cpf"].ToString(),
                        Nascimento = dr["Nascimento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["Nascimento"])
                    });

                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        /// <summary>
        /// Retorna um Manobrista específico
        /// </summary>
        /// <param name="Id">Id do Manobrista</param>
        /// <returns>Um único Manobrista</returns>
        public Manobrista GetSingle(int Id)
        {
            var parametros = new List<OleDbParameter>();
            parametros.Add(new OleDbParameter("Id", Id.ToString()));
            return GetAll($" AND Id = ?", parametros).FirstOrDefault();
        }

        /// <summary>
        /// Salva os dados do Motorista
        /// </summary>
        /// <param name="manobrista">Dados do Motorista</param>
        /// <returns>Verdadeiro se sucesso</returns>
        public bool Salvar(Manobrista manobrista)
        {
            var salvar = "";
            var parametros = new List<OleDbParameter>();
            parametros.Add(new OleDbParameter("Nome", manobrista.Nome));
            parametros.Add(new OleDbParameter("Cpf", manobrista.Cpf));
            parametros.Add(new OleDbParameter("Nascimento", manobrista.Nascimento.ToString("dd/MM/yyyy")));

            if (manobrista.Id == 0)
            {
                salvar = $"INSERT INTO Manobrista (Nome, Cpf, Nascimento) VALUES (?, ? , ?)";
            }
            else
            {
                salvar = $"UPDATE Manobrista SET Nome = ?, Cpf = ?, Nascimento = ? WHERE Id = ?";
                parametros.Add(new OleDbParameter("Id", manobrista.Id));
            }

            return new ConnectionFactory().executeNonQuery(salvar, parametros) > 0;
        }

    }
}