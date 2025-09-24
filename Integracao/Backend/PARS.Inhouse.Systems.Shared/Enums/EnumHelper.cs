using System.Reflection;
using System.Runtime.Serialization;

public static class EnumHelper
{
    /// <summary>
    /// Obtém o valor do atributo EnumMember associado a um valor de enum genérico.
    /// </summary>
    /// <typeparam name="TEnum">O tipo do enum.</typeparam>
    /// <param name="enumValue">O valor do enum.</param>
    /// <returns>O valor da string definida no atributo EnumMember.</returns>
    public static string ObterValorEnumMember<TEnum>(TEnum enumValue) where TEnum : Enum
    {
        var memberInfo = enumValue.GetType().GetMember(enumValue.ToString());
        if (memberInfo.Length == 0)
            throw new InvalidOperationException($"Enum '{enumValue}' não possui metadados de membro.");

        var attribute = memberInfo[0].GetCustomAttribute<EnumMemberAttribute>();
        if (attribute == null || string.IsNullOrWhiteSpace(attribute.Value))
            throw new InvalidOperationException($"Enum '{enumValue}' não possui o atributo [EnumMember] ou valor definido.");

        return attribute.Value;
    }
}
