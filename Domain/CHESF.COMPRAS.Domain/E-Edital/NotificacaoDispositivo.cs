using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHESF.COMPRAS.Domain.E_Edital
{
    [Table("TB_NOTIFICACAO_DISPOSITIVO", Schema = "dbo")]
    public class NotificacaoDispositivo
    {
        [Key] [Column("ID_NOTIFICACAO_DISPOSITIVO")] public int Id { get; set; }
        [Column("ID_NOTIFICACAO")] public int IdNotificacao { get; set; }
        [Column("ID_DISPOSITIVO")] public int IdDispositivo { get; set; }
        [Column("DT_ENTREGUE")] public DateTime? DataEntregue { get; set; }
        [Column("DT_ULTIMA_TENTATIVA_ENTREGA")] public DateTime? DataUltimaTentativaEntrega { get; set; }
        [Column("VL_CODIGO_FALHA")] public int? CodigoFalha { get; set; }
        [Column("VL_CODIGO_FALHA_FCM")] public int? CodigoFalhaFcm { get; set; }
        [Column("DS_FALHA")] public string? Falha { get; set; }

        [ForeignKey("IdNotificacao")]
        public Notificacao Notificacao { get; set; }
        
        [ForeignKey("IdDispositivo")]
        public Dispositivo Dispositivo { get; set; }
    }
}