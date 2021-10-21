using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EstaparCRUD.Models
{
    public class Manobra
    {
        /// <summary>
        /// ID automático
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Manobrista vinculado
        /// </summary>
        public Manobrista ManobristaDaManobra { get; set; }
        /// <summary>
        /// Carro vinculado
        /// </summary>
        public Carro CarroDaManobra { get; set; }
        /// <summary>
        /// Classificação vinculada
        /// </summary>
        public bool Classificacao { get; set; }
        /// <summary>
        /// Datahora do ocorrido
        /// </summary>
        public DateTime DataHora { get; set; }
        /// <summary>
        /// Classificação vinculada
        /// </summary>
        public string ClassificacaoTexto { get => (Classificacao) ? "Recepção do Veículo" : "Retorno do veículo ao Cliente"; }
    }
}