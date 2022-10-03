using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHESF.COMPRAS.Domain.E_Edital
{
    [Table("TB_DISPOSITIVO_METADADO", Schema = "dbo")]
    public class DispositivoMetadado
    {
        [Key] [Column("ID_DISPOSITIVO_METADADO")] public int Id { get; set; }
        [Column("ID_DISPOSITIVO")] public int IdDispositivo { get; set; }
        [Column("NM_IDENTIFICADOR")] public string Identificador { get; set; }
        [Column("VL_METADADO")] public string Valor { get; set; }

        [ForeignKey("IdDispositivo")]
        public Dispositivo Dispositivo { get; set; }
    }
}