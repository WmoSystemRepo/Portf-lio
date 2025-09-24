using System.Runtime.Serialization;

[DataContract]
public enum TipoTemplateEnum
{
    [EnumMember(Value = "VIP_MP_Educacional")]
    VIP_MP_Educacional,

    [EnumMember(Value = "VIP_MP_Comercial")]
    VIP_MP_Comercial,

    [EnumMember(Value = "VIP_MP_Governo")]
    VIP_MP_Governo,

    [EnumMember(Value = "3YR_Commit_Educacional")]
    Trier_Commit_Educacional,

    [EnumMember(Value = "3YR_Commit_Comercial")]
    Trier_Commit_Comercial,

    [EnumMember(Value = "3YR_Commit_Governo")]
    Trier_Commit_Governo
}
