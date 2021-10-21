using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EstaparCRUD.Models
{
    /// <summary>
    /// Classe Manobrista
    /// </summary>
    public class Manobrista
    {
        /// <summary>
        /// ID automático do manobrista
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nome completo do Manobrista
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// CPF do Manobrista
        /// </summary>
        public string Cpf { get; set; }
        /// <summary>
        /// Data de Nascimento do Manobrista
        /// </summary>
        public DateTime Nascimento { get; set; }
        /// <summary>
        /// Data de Nascimento do Manobrista formatada
        /// </summary>
        public string NascimentoFormatado { get => Nascimento.ToString("dd/MM/yyyy"); }
    }
}