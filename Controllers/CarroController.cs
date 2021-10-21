using EstaparCRUD.Classes;
using EstaparCRUD.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace EstaparCRUD.Controllers
{
    public class CarroController
    {

        /// <summary>
        /// Retorna todos os carros
        /// </summary>
        /// <param name="filtros">filtro</param>
        /// <param name="parameters">parâmetros do filtro</param>
        /// <returns>Lista de carros</returns>
        public List<Carro> GetAll(string filtros, List<OleDbParameter> parameters)
        {
            var list = new List<Carro>();
            var select = $@"SELECT carro.* FROM carro 
                            INNER JOIN modelocarro ON modelocarro.id = carro.idmodelo
                            INNER JOIN marcacarro ON marcacarro.id = ModeloCarro.IdMarca
                            WHERE (1=1) {filtros}";
            try
            {
                var dt = new ConnectionFactory().ExecuteToDataTable(select, parameters);
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Carro
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Placa = dr["Placa"].ToString(),
                        Modelo = new CarroController().GetSingleModelo(Convert.ToInt32(dr["IdModelo"].ToString()))
                    });

                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        /// <summary>
        /// Retorna um carro específico
        /// </summary>
        /// <param name="Id">Id do carro</param>
        /// <returns>Um único carro</returns>
        public Carro GetSingle(int Id)
        {
            var parametros = new List<OleDbParameter>();
            parametros.Add(new OleDbParameter("Id", Id.ToString()));
            return GetAll($" AND carro.Id = ?", parametros).FirstOrDefault();
        }

        /// <summary>
        /// Salva os dados do carro
        /// </summary>
        /// <param name="carro">Dados do carro</param>
        /// <returns>Verdadeiro se sucesso</returns>
        public bool Salvar(Carro carro)
        {
            var salvar = "";
            var parametros = new List<OleDbParameter>();
            parametros.Add(new OleDbParameter("IdModelo", carro.Modelo.Id));
            parametros.Add(new OleDbParameter("placa", carro.Placa));

            if (carro.Id == 0)
            {
                salvar = $"INSERT INTO carro (IdModelo, placa) VALUES (?, ?)";
            }
            else
            {
                salvar = $"UPDATE carro SET IdModelo = ?, placa = ? WHERE Id = ?";
                parametros.Add(new OleDbParameter("Id", carro.Id));
            }

            return new ConnectionFactory().executeNonQuery(salvar, parametros) > 0;
        }


        /// <summary>
        /// Resgatar todos os modelos de carro do BD
        /// </summary>
        /// <param name="filtros">filtros</param>
        /// <param name="parameters">parâmetros dos filtros</param>
        /// <returns></returns>
        public List<ModeloCarro> GetAllModelos(string filtros, List<OleDbParameter> parameters)
        {
            var list = new List<ModeloCarro>();
            var select = $"SELECT * FROM ModeloCarro WHERE (1=1) {filtros}";
            try
            {
                var dt = new ConnectionFactory().ExecuteToDataTable(select, parameters);
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new ModeloCarro
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Modelo = dr["Modelo"].ToString(),
                        Marca = new CarroController().GetSingleMarca(Convert.ToInt32(dr["IdMarca"].ToString()))
                    });

                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        /// <summary>
        /// Retorna um modelo de carro específico
        /// </summary>
        /// <param name="Id">Id do modelo</param>
        /// <returns>Um único modelo</returns>
        public ModeloCarro GetSingleModelo(int Id)
        {
            var parametros = new List<OleDbParameter>();
            parametros.Add(new OleDbParameter("Id", Id.ToString()));
            return GetAllModelos($" AND Id = ?", parametros).FirstOrDefault();
        }

        /// <summary>
        /// Salva os dados do modeko de carro
        /// </summary>
        /// <param name="carro">Dados do modelo</param>
        /// <returns>Verdadeiro se sucesso</returns>
        public bool SalvarModelo(ModeloCarro modelo)
        {
            var salvar = "";
            var parametros = new List<OleDbParameter>();
            parametros.Add(new OleDbParameter("IdMarca", modelo.Marca.Id));
            parametros.Add(new OleDbParameter("Modelo", modelo.Modelo));

            if (modelo.Id == 0)
            {
                salvar = $"INSERT INTO ModeloCarro (IdMarca, Modelo) VALUES (?, ?)";
            }
            else
            {
                salvar = $"UPDATE ModeloCarro SET IdMarca = ?, Modelo = ? WHERE Id = ?";
                parametros.Add(new OleDbParameter("Id", modelo.Id));
            }

            return new ConnectionFactory().executeNonQuery(salvar, parametros) > 0;
        }


        /// <summary>
        /// Resgatar todos as marcas de carro do BD
        /// </summary>
        /// <param name="filtros">filtros</param>
        /// <param name="parameters">parâmetros dos filtros</param>
        /// <returns></returns>
        public List<MarcaCarro> GetAllMarcas(string filtros, List<OleDbParameter> parameters)
        {
            var list = new List<MarcaCarro>();
            var select = $"SELECT * FROM MarcaCarro WHERE (1=1) {filtros}";
            try
            {
                var dt = new ConnectionFactory().ExecuteToDataTable(select, parameters);
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new MarcaCarro
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Marca = dr["Marca"].ToString()
                    });

                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        /// <summary>
        /// Retorna uma marca específico
        /// </summary>
        /// <param name="Id">Id da marca</param>
        /// <returns>Um único carro</returns>
        public MarcaCarro GetSingleMarca(int Id)
        {
            var parametros = new List<OleDbParameter>();
            parametros.Add(new OleDbParameter("Id", Id.ToString()));
            return GetAllMarcas($" AND Id = ?", parametros).FirstOrDefault();
        }

        /// <summary>
        /// Salva os dados da marca
        /// </summary>
        /// <param name="marca">Dados da Marca</param>
        /// <returns>Verdadeiro se sucesso</returns>
        public bool SalvarMarca(MarcaCarro marca)
        {
            var salvar = "";
            var parametros = new List<OleDbParameter>();
            parametros.Add(new OleDbParameter("Marca", marca.Marca));

            if (marca.Id == 0)
            {
                salvar = $"INSERT INTO MarcaCarro (Marca) VALUES (?)";
            }
            else
            {
                salvar = $"UPDATE MarcaCarro SET Marca = ? WHERE Id = ?";
                parametros.Add(new OleDbParameter("Id", marca.Id));
            }

            return new ConnectionFactory().executeNonQuery(salvar, parametros) > 0;
        }

    }
}