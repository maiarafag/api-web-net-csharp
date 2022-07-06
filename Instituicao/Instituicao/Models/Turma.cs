using System.Text.Json.Serialization;

namespace Instituicao.Models
{
    public class Turma
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; } 


        #region Navigation Properties
        [JsonIgnore]
        public virtual List<Aluno>? Alunos { get; set; }
        #endregion

    }
}
