using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EstaparCRUD.Models
{
    /// <summary>
    /// Classe do Carro
    /// </summary>
    public class Carro
    {
        /// <summary>
        /// ID automático
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Modelo/Marca do Carro
        /// </summary>
        public ModeloCarro Modelo { get; set; }
        /// <summary>
        /// Placa do Carro
        /// </summary>
        public string Placa { get; set; }
    }
    /// <summary>
    /// Classe da Marca
    /// </summary>
    public class MarcaCarro
    {
        /// <summary>
        /// ID automático
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Descrição
        /// </summary>
        public string Marca { get; set; }
    }
    /// <summary>
    /// Classe do Modelo
    /// </summary>
    public class ModeloCarro
    {
        /// <summary>
        /// ID automático
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Marca referênciada
        /// </summary>
        public MarcaCarro Marca { get; set; }
        /// <summary>
        /// Descrição
        /// </summary>
        public string Modelo { get; set; }
    }
}