using Terraria;
using Terraria.ModLoader;

namespace RPGARGENTO
{
    public class GlobalRPGItem : GlobalItem
    {
        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
            // Verificamos que sea un item de Melee
            if (item.DamageType.CountsAsClass(DamageClass.Melee))
            {
                var modPlayer = player.GetModPlayer<RPGPlayer>();
                
                // 1. Sumamos el bono que te dan los Yunques (1% por cada punto)
                float totalScaleBonus = modPlayer.yunqueMeleeSize * 0.01f;
                
                // 2. Sumamos el 10% extra si el jugador eligió la clase avanzada BERSERKER
                if (modPlayer.claseElegida == 0 && modPlayer.claseAvanzada == 0)
                {
                    totalScaleBonus += 0.10f;
                }

                // Aplicamos el tamaño final
                scale += totalScaleBonus;
            }
        }
    }
}