using Terraria;
using Terraria.ModLoader;

namespace RPGARGENTO
{
    public class GlobalRPGItem : GlobalItem
    {
        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
            if (item.DamageType.CountsAsClass(DamageClass.Melee))
            {
                var modPlayer = player.GetModPlayer<RPGPlayer>();
                scale += modPlayer.yunqueMeleeSize * 0.01f;
            }
        }
    }
}
