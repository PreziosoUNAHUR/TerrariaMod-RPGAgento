using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace RPGARGENTO
{
    public class RPGGlobalNPC : GlobalNPC
    {

        // =================================================================
        // ESCALADO DINÁMICO DE ENEMIGOS
        // Modifica la vida, daño y defensa de todos los NPCs basándose en el 
        // nivel actual del jugador local. Se aplica un escalonamiento cada 10 niveles.
        // =================================================================

        public override void SetDefaults(NPC npc)
        {
            if (Main.gameMenu || Main.LocalPlayer == null || !Main.LocalPlayer.active) return;
            var player = Main.LocalPlayer.GetModPlayer<RPGPlayer>();
            
            // Escalado cada 10 niveles
            int bonusFactor = player.level / 10; 
            if (bonusFactor > 0)
            {
                if (npc.boss)
                {
                    npc.damage += 15 * bonusFactor;
                    npc.defense += 20 * bonusFactor;
                    npc.lifeMax += 2000 * bonusFactor;
                }
                else
                {
                    npc.damage += 10 * bonusFactor;
                    npc.defense += 10 * bonusFactor;
                    npc.lifeMax += 100 * bonusFactor;
                }
                npc.life = npc.lifeMax;
            }
        }

        public override void OnKill(NPC npc)
        {
            // XP basada en la vida máxima final
            int xpDrop = npc.lifeMax / 10;
            if (xpDrop < 1) xpDrop = 1;

            Player player = Main.LocalPlayer;
            if (player.active && !player.dead)
            {
                var modPlayer = player.GetModPlayer<RPGPlayer>();
                CombatText.NewText(npc.getRect(), new Color(255, 255, 0), $"+{xpDrop} XP", true);
                modPlayer.GainXP(xpDrop);
            }
        }

        // =================================================================
        // HOLOGRAMA DE ESTADÍSTICAS (UI Flotante)
        // Dibuja en tiempo real sobre la cabeza del enemigo sus stats actuales 
        // si el cursor del jugador (Hitbox) se encuentra sobre él.
        // =================================================================
        
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!npc.active || npc.life <= 0 || npc.friendly || npc.dontTakeDamage) return;
            
            if (npc.Hitbox.Contains(Main.MouseWorld.ToPoint()))
            {
                // Carga los prefijos (Vida:, Defensa:, Daño:) del archivo de idioma
                string tVida = Language.GetTextValue("Mods.RPGARGENTO.NPC.Vida");
                string tArmor = Language.GetTextValue("Mods.RPGARGENTO.NPC.Armor");
                string tDano = Language.GetTextValue("Mods.RPGARGENTO.NPC.Dano");

                string statsText = $"{tVida}{npc.life} / {npc.lifeMax}\n{tArmor}{npc.defense}\n{tDano}{npc.damage}";
                Vector2 textPos = npc.Top - screenPos;
                textPos.Y -= 45f; 
                Terraria.Utils.DrawBorderString(spriteBatch, statsText, textPos, Color.White, 0.7f, 0.5f, 0.5f);
            }
        }
    }
}