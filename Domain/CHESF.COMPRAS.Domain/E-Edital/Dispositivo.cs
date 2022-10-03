using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHESF.COMPRAS.Domain.E_Edital
{
    [Table("TB_DISPOSITIVO", Schema = "dbo")]
    public class Dispositivo
    {
        public Dispositivo()
        {
            Metadados = new List<DispositivoMetadado>();
        }
        
        [Key] [Column("ID_DISPOSITIVO")] public int Id { get; set; }
        [Column("UID_FIREBASE_INSTALLATION")] public string UidFirebaseInstallation { get; set; }
        [Column("VL_FCM_TOKEN")] public string? FcmToken { get; set; }
        [Column("DT_ATUALIZACAO_FCM_TOKEN")] public DateTime? DataAtualizacaoFcmToken { get; set; }
        [Column("NR_CNPJ_VINCULADO")] public string? CnpjVinculado { get; set; }

        public List<DispositivoMetadado> Metadados { get; set; }
    }
}