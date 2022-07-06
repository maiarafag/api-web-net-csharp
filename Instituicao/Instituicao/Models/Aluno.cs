using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Instituicao.Models
{
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        [Column("data_nascimento")]
        public DateTime DataNascimento { get; set; }
        public char Sexo { get; set; }
        public int Turma_Id { get; set; }
        public int TotalFaltas { get; set; }

        #region  Navigation Properties
        [JsonIgnore]
        public virtual Turma? Turma { get; set; }
        #endregion
    }
}
