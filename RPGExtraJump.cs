using Terraria;
using Terraria.ModLoader;

namespace RPGARGENTO
{
    public class RPGExtraJump : ExtraJump
    {
        public override Position GetDefaultPosition() => new After(BlizzardInABottle);

        public override float GetDurationMultiplier(Player player) => 1f;

        public override void OnRefreshed(Player player)
        {
            var modPlayer = player.GetModPlayer<RPGPlayer>();
            modPlayer.saltosRestantes = modPlayer.GetTotalExtraJumps();
        }

        public override void OnStarted(Player player, ref bool playSound)
        {
            playSound = true;
            var modPlayer = player.GetModPlayer<RPGPlayer>();

            if (modPlayer.saltosRestantes > 1)
            {
                modPlayer.saltosRestantes--;
                player.GetJumpState<RPGExtraJump>().Available = true;
            }
        }
    }
}
