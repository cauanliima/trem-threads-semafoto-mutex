using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CHESF.COMPRAS.Domain.E_Edital
{
    [Table("TB_LICITACAO_ARQUIVO", Schema = "dbo")]
    public class Anexo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ID_LICITACAO_ARQUIVO")]
        public int Codigo { get; set; }

        [Column("NR_PRCS")] public long CodigoLicitacao { get; set; }

        [Column("NM_LICITACAO_ARQUIVO")] public string Nome { get; set; }

        [Column("DS_LICITACAO_ARQUIVO")] public string Descricao { get; set; }

        [Column("DT_CRIACAO_LICITACAO_ARQUIVO")]
        public DateTime DataCriacao { get; set; }


        [JsonIgnore]
        [Column("AR_LICITACAO")]
        public Byte[] Arquivo { get; set; }
    }
}