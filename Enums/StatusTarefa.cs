using System.ComponentModel;

namespace SistemaTarefa.Enums
{
    public enum StatusTarefa
    {
        // 1 = A Fazer, 2 = Em Andamento, 3 = Concluída
        [Description("A Fazer")]
        AFazer = 1,
        [Description("Em Andamento")]
        EmAndamento = 2,
        [Description("Concluída")]
        Concluida = 3
    }
}