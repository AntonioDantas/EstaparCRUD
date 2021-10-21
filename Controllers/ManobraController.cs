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
    public class ManobraController
    {
        /// <summary>
        /// Retorna todos os manobras
        /// </summary>
        /// <param name="filtros">filtro</param>
        /// <param name="parameters">parâmetros do filtro</param>
        /// <returns>Lista de manobras</returns>
        public List<Manobra> GetAll(string filtros, List<OleDbParameter> parameters)
        {
            var list = new List<Manobra>();
            var select = $"SELECT * FROM Manobra WHERE (1=1) {filtros}";
            try
            {
                var dt = new ConnectionFactory().ExecuteToDataTable(select, parameters);
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Manobra
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        DataHora = Convert.ToDateTime(dr["DataHora"]),
                        Classificacao = Convert.ToBoolean(dr["Classificacao"]),
                        CarroDaManobra = new CarroController().GetSingle(Convert.ToInt32(dr["IdCarro"].ToString())),
                        ManobristaDaManobra = new ManobristaController().GetSingle(Convert.ToInt32(dr["IdManobrista"].ToString()))
                    });

                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        /// <summary>
        /// Retorna uma manobra específica
        /// </summary>
        /// <param name="Id">Id da manobra</param>
        /// <returns>Uma única manobra</returns>
        public Manobra GetSingle(int Id)
        {
            var parametros = new List<OleDbParameter>();
            parametros.Add(new OleDbParameter("Id", Id.ToString()));
            return GetAll($" AND Id = ?", parametros).FirstOrDefault();
        }

        /// <summary>
        /// Salva os dados da manobra
        /// </summary>
        /// <param name="manobrista">Dados do manobra</param>
        /// <returns>Verdadeiro se sucesso</returns>
        public bool Salvar(Manobra manobra)
        {
            var salvar = "";
            var parametros = new List<OleDbParameter>();
            parametros.Add(new OleDbParameter("IdCarro", manobra.CarroDaManobra.Id));
            parametros.Add(new OleDbParameter("IdManobrista", manobra.ManobristaDaManobra.Id));
            parametros.Add(new OleDbParameter("Classificacao", manobra.Classificacao));
            parametros.Add(new OleDbParameter("Datahora", manobra.DataHora));

            if (manobra.Id == 0)
            {
                salvar = $"INSERT INTO Manobra (IdCarro, IdManobrista, Classificacao, Datahora) VALUES (?, ? , ?, ?)";
            }
            else
            {
                salvar = $"UPDATE Manobra SET IdCarro = ?, IdManobrista = ?, Classificacao = ?, Datahora = ? WHERE Id = ?";
                parametros.Add(new OleDbParameter("Id", manobra.Id));
            }

            return new ConnectionFactory().executeNonQuery(salvar, parametros) > 0;
        }

    }
}