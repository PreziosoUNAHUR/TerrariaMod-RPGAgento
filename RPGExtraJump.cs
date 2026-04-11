using Terraria;
using Terraria.ModLoader;

namespace RPGARGENTO
{
    public class RPGExtraJump : ExtraJump
    {
        // Posicionamos nuestro salto después de los de vainilla
        public override Position GetDefaultPosition() => new After(BlizzardInABottle);

        public override float GetDurationMultiplier(Player player) => 1f;

        // Se ejecuta cuando el jugador toca el piso o se agarra de una cuerda
        public override void OnRefreshed(Player player)
        {
            var modPlayer = player.GetModPlayer<RPGPlayer>();
            modPlayer.saltosRestantes = modPlayer.GetTotalExtraJumps();
        }

        // Se ejecuta en el aire cuando apretás la barra espaciadora
        public override void OnStarted(Player player, ref bool playSound)
        {
            playSound = true;
            var modPlayer = player.GetModPlayer<RPGPlayer>();
            
            // Si nos quedan saltos después de usar este, rehabilitamos el salto para volver a usarlo
            if (modPlayer.saltosRestantes > 1)
            {
                modPlayer.saltosRestantes--;
                player.GetJumpState<RPGExtraJump>().Available = true; 
            }
        }
    }
}