using EstaparCRUD.Classes;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace EstaparCRUD.Controllers
{
    public class GenericoController
    {

        /// <summary>
        /// Exclui definitivamente uma tabela especificada
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        public bool Delete(int Id, string tabela)
        {
            var delete = $"DELETE FROM {tabela} WHERE Id = ?";
            var parametros = new List<OleDbParameter>();
            parametros.Add(new OleDbParameter("Id", Id.ToString()));
            return new ConnectionFactory().executeNonQuery(delete, parametros) > 0;
        }
    }
}