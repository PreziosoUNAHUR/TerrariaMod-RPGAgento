using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace RPGARGENTO
{
    public class RPGGlobalNPC : GlobalNPC
    {
        public static int GetWorldLevel()
        {
            int count = 0;
            if (NPC.downedSlimeKing) count++;
            if (NPC.downedBoss1) count++;
            if (NPC.downedBoss2) count++;
            if (NPC.downedQueenBee) count++;
            if (NPC.downedBoss3) count++;
            if (NPC.downedDeerclops) count++;
            if (Main.hardMode) count++;
            if (NPC.downedQueenSlime) count++;
            if (NPC.downedMechBoss1) count++;
            if (NPC.downedMechBoss2) count++;
            if (NPC.downedMechBoss3) count++;
            if (NPC.downedPlantBoss) count++;
            if (NPC.downedGolemBoss) count++;
            if (NPC.downedFishron) count++;
            if (NPC.downedEmpressOfLight) count++;
            if (NPC.downedAncientCultist) count++;
            if (NPC.downedMoonlord) count++;
            return count;
        }

        public override void SetDefaults(NPC npc)
        {
            if (Main.gameMenu || Main.LocalPlayer == null || !Main.LocalPlayer.active) return;

            int bonusFactor = GetWorldLevel();
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

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!npc.active || npc.life <= 0 || npc.friendly || npc.dontTakeDamage) return;

            if (npc.Hitbox.Contains(Main.MouseWorld.ToPoint()))
            {
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
