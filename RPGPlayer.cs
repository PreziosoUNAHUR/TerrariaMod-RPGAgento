using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;

namespace RPGARGENTO
{
    public class RPGPlayer : ModPlayer
    {
        public int xp = 0; public int maxXp = 100; public int level = 1; public int skillPoints = 0;
        public int GetJefesDerrotadosDelMundo() {
            int c = 0; if (NPC.downedSlimeKing) c++; if (NPC.downedBoss1) c++; if (NPC.downedBoss2) c++; if (NPC.downedQueenBee) c++; if (NPC.downedBoss3) c++; if (NPC.downedDeerclops) c++; if (Main.hardMode) c++; if (NPC.downedQueenSlime) c++; if (NPC.downedMechBoss1) c++; if (NPC.downedMechBoss2) c++; if (NPC.downedMechBoss3) c++; if (NPC.downedPlantBoss) c++; if (NPC.downedGolemBoss) c++; if (NPC.downedFishron) c++; if (NPC.downedEmpressOfLight) c++; if (NPC.downedAncientCultist) c++; if (NPC.downedMoonlord) c++; return c;
        }
        public int levelCap => 20 + (GetJefesDerrotadosDelMundo() * 20);
        public int ultimoLevelCapVisto = 20;

        public int claseElegida = -1; 
        public int claseAvanzada = -1; 

        public int ptsVida = 0; public int ptsMana = 0; public int ptsArmadura = 0;
        public int ptsMelee = 0; public int ptsRanger = 0; public int ptsMagic = 0; public int ptsSummon = 0; 
        public int ptsSuerte = 0; public int ptsKnockback = 0; public int ptsMoveSpeed = 0;

        public float yunqueMagDmg, yunqueMelDmg, yunqueRanDmg, yunqueSumDmg;
        public float yunqueKnockback, yunqueCrit, yunqueDef, yunqueManaCost, yunqueAmmoCost;
        public int yunqueLife, yunqueMana, yunqueJumpTimes, yunqueAggro, yunqueFish;
        public float yunqueJumpHeight, yunqueGrabRange, yunqueMeleeSize, yunqueRespawn, yunqueMiningSpeed, yunqueMoveSpeed, yunqueWhipRange;
        public float yunqueRegenVida, yunqueRegenMana, yunqueManaSickRes, yunquePotionSickRes;
        public int yunqueMaxMinion, yunqueMaxSentinel;
        public float yunqueDamageReduction;

        // --- SISTEMA OFICIAL DE SALTOS EXTRAS ---
        public int saltosRestantes = 0;
        public int GetTotalExtraJumps() {
            int total = yunqueJumpTimes;
            if (claseElegida == 0 && claseAvanzada == 0) total += 1; // Berserker
            if (claseElegida == 3 && claseAvanzada == 1) total += 1; // Invocador Inexperto
            return total;
        }

        public void GainXP(int amount) {
            if (level >= levelCap) return; 
            xp += amount;
            while (xp >= maxXp) { xp -= maxXp; level++; skillPoints++; maxXp = (int)(maxXp * 1.5f); if (level >= levelCap) { xp = 0; break; } }
        }

        // =================================================================
        // SISTEMA DE GUARDADO Y CARGA (SERIALIZACIÓN)
        // Guarda todas las variables personalizadas del jugador en el archivo .plr
        // Utiliza TagCompounds (diccionarios Key-Value) propios de tModLoader.
        // =================================================================

        public override void SaveData(TagCompound tag) {
            tag["xp"] = xp; tag["mx"] = maxXp; tag["lv"] = level; tag["sp"] = skillPoints;
            tag["ce"] = claseElegida; tag["ca"] = claseAvanzada;
            tag["pv"] = ptsVida; tag["pm"] = ptsMana; tag["pa"] = ptsArmadura; tag["pme"] = ptsMelee; tag["pr"] = ptsRanger; tag["pma"] = ptsMagic; tag["ps"] = ptsSummon; tag["psu"] = ptsSuerte; tag["pkb"] = ptsKnockback; tag["pms"] = ptsMoveSpeed;
            tag["y1"] = yunqueMagDmg; tag["y2"] = yunqueMelDmg; tag["y3"] = yunqueRanDmg; tag["y4"] = yunqueSumDmg; tag["y5"] = yunqueKnockback; tag["y6"] = yunqueCrit; tag["y7"] = yunqueDef; tag["y8"] = yunqueManaCost; tag["y9"] = yunqueAmmoCost; tag["y10"] = yunqueLife; tag["y11"] = yunqueMana; tag["y12"] = yunqueJumpTimes; tag["y13"] = yunqueAggro; tag["y14"] = yunqueFish; tag["y15"] = yunqueJumpHeight; tag["y16"] = yunqueGrabRange; tag["y17"] = yunqueMeleeSize; tag["y18"] = yunqueRespawn; tag["y19"] = yunqueMiningSpeed; tag["y20"] = yunqueMoveSpeed; tag["y21"] = yunqueWhipRange; tag["y22"] = yunqueRegenVida; tag["y23"] = yunqueRegenMana; tag["y24"] = yunqueManaSickRes; tag["y25"] = yunquePotionSickRes; tag["y26"] = yunqueMaxMinion; tag["y27"] = yunqueMaxSentinel; tag["y28"] = yunqueDamageReduction;
        }

        public override void LoadData(TagCompound tag) {
            xp = tag.GetInt("xp"); maxXp = tag.GetInt("mx"); if(maxXp <= 0) maxXp = 100; level = tag.GetInt("lv"); skillPoints = tag.GetInt("sp");
            claseElegida = tag.ContainsKey("ce") ? tag.GetInt("ce") : -1; claseAvanzada = tag.ContainsKey("ca") ? tag.GetInt("ca") : -1;
            ptsVida = tag.GetInt("pv"); ptsMana = tag.GetInt("pm"); ptsArmadura = tag.GetInt("pa"); ptsMelee = tag.GetInt("pme"); ptsRanger = tag.GetInt("pr"); ptsMagic = tag.GetInt("pma"); ptsSummon = tag.GetInt("ps"); ptsSuerte = tag.GetInt("psu"); ptsKnockback = tag.GetInt("pkb"); ptsMoveSpeed = tag.GetInt("pms");
            yunqueMagDmg = tag.GetFloat("y1"); yunqueMelDmg = tag.GetFloat("y2"); yunqueRanDmg = tag.GetFloat("y3"); yunqueSumDmg = tag.GetFloat("y4"); yunqueKnockback = tag.GetFloat("y5"); yunqueCrit = tag.GetFloat("y6"); yunqueDef = tag.GetFloat("y7"); yunqueManaCost = tag.GetFloat("y8"); yunqueAmmoCost = tag.GetFloat("y9"); yunqueLife = tag.GetInt("y10"); yunqueMana = tag.GetInt("y11"); yunqueJumpTimes = tag.GetInt("y12"); yunqueAggro = tag.GetInt("y13"); yunqueFish = tag.GetInt("y14"); yunqueJumpHeight = tag.GetFloat("y15"); yunqueGrabRange = tag.GetFloat("y16"); yunqueMeleeSize = tag.GetFloat("y17"); yunqueRespawn = tag.GetFloat("y18"); yunqueMiningSpeed = tag.GetFloat("y19"); yunqueMoveSpeed = tag.GetFloat("y20"); yunqueWhipRange = tag.GetFloat("y21"); yunqueRegenVida = tag.GetFloat("y22"); yunqueRegenMana = tag.GetFloat("y23"); yunqueManaSickRes = tag.GetFloat("y24"); yunquePotionSickRes = tag.GetFloat("y25"); yunqueMaxMinion = tag.GetInt("y26"); yunqueMaxSentinel = tag.GetInt("y27"); yunqueDamageReduction = tag.GetFloat("y28");
        }

        // =================================================================
        // ACTUALIZACIÓN DE ESTADÍSTICAS GLOBALES
        // Se ejecuta cada frame. Aquí se aplican matemáticamente todos los 
        // bonos de puntos, yunques y clases elegidas directamente al jugador.
        // =================================================================

        public override void ResetEffects() {
            Player.statLifeMax2 += (ptsVida * 5) + yunqueLife; Player.statManaMax2 += (ptsMana * 5) + yunqueMana; Player.statDefense += ptsArmadura + (int)yunqueDef;
            float hpPS = Player.statLifeMax2 * (yunqueRegenVida / 100f); Player.lifeRegen += (int)(hpPS * 2); 
            float mnPS = Player.statManaMax2 * (yunqueRegenMana / 100f); Player.manaRegenBonus += (int)(mnPS * 2);
            Player.endurance += (yunqueDamageReduction * 0.01f); Player.GetKnockback(DamageClass.Generic) += (ptsKnockback * 0.001f) + (yunqueKnockback * 0.01f);
            Player.manaCost -= yunqueManaCost * 0.01f; Player.whipRangeMultiplier += yunqueWhipRange * 0.01f;
            float moveBonus = (ptsMoveSpeed * 0.01f) + (yunqueMoveSpeed * 0.01f); Player.moveSpeed += moveBonus; Player.maxRunSpeed += moveBonus;
            Player.maxMinions += yunqueMaxMinion; Player.maxTurrets += yunqueMaxSentinel; Player.aggro += yunqueAggro; Player.fishingSkill += yunqueFish; Player.jumpSpeedBoost += yunqueJumpHeight; 

            // Activamos el salto nuevo
            if (GetTotalExtraJumps() > 0) {
                Player.GetJumpState<RPGExtraJump>().Enable();
            }

            // --- LÓGICAS DE CLASE ---
            if (claseElegida == 0) {
                if (claseAvanzada == -1) { Player.statLifeMax2 += 10; Player.moveSpeed += 0.1f; Player.maxRunSpeed += 0.1f; Player.statDefense += 5; Player.GetKnockback(DamageClass.Melee) += 0.02f; }
                else if (claseAvanzada == 0) { Player.statLifeMax2 += 20; Player.moveSpeed += 0.15f; Player.maxRunSpeed += 0.15f; Player.lifeRegen += 2; } 
                else if (claseAvanzada == 1) { Player.statLifeMax2 += 100; Player.statDefense += 10; Player.GetKnockback(DamageClass.Melee) += 0.05f; }
            } else if (claseElegida == 1) {
                if (claseAvanzada == -1) { Player.moveSpeed += 0.15f; Player.maxRunSpeed += 0.15f; Player.statDefense += 2; Player.jumpSpeedBoost += 2f; Player.aggro -= 2; }
                else if (claseAvanzada == 0) { Player.aggro -= 10; }
                else if (claseAvanzada == 1) { Player.moveSpeed += 0.10f; Player.maxRunSpeed += 0.10f; Player.statDefense += 5; }
            } else if (claseElegida == 2) {
                if (claseAvanzada == -1) { Player.statManaMax2 += 10; Player.moveSpeed += 0.1f; Player.maxRunSpeed += 0.1f; Player.statDefense += 2; Player.manaRegenBonus += 2; Player.manaCost -= 0.1f; }
                else if (claseAvanzada == 0) { Player.statManaMax2 += 20; Player.manaCost -= 0.2f; }
                else if (claseAvanzada == 1) { Player.statLifeMax2 += 10; Player.statManaMax2 += 20; Player.moveSpeed += 0.1f; Player.maxRunSpeed += 0.1f; Player.manaRegenBonus += 10; }
            } else if (claseElegida == 3) {
                if (claseAvanzada == -1) { Player.statLifeMax2 += 10; Player.moveSpeed += 0.05f; Player.maxRunSpeed += 0.05f; Player.statDefense += 5; Player.maxMinions += 1; Player.whipRangeMultiplier += 0.02f; }
                else if (claseAvanzada == 0) { Player.statDefense += 10; Player.maxMinions += 2; Player.maxTurrets += 2; }
                else if (claseAvanzada == 1) { Player.statLifeMax2 += 20; Player.moveSpeed += 0.10f; Player.maxRunSpeed += 0.10f; Player.statDefense += 10; Player.maxMinions -= 1; Player.whipRangeMultiplier += 0.20f; Player.jumpSpeedBoost += 5f; Player.aggro -= 2; }
            }
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo) {
            float chanceToSave = yunqueAmmoCost * 0.01f;
            if (claseElegida == 1 && claseAvanzada == -1) chanceToSave += 0.05f;
            if (claseElegida == 1 && claseAvanzada == 0) chanceToSave += 0.20f;
            if (claseElegida == 1 && claseAvanzada == 1) chanceToSave += 0.10f;
            if (Main.rand.NextFloat() < chanceToSave) return false; return true; 
        }
        
        // =================================================================
        // LÓGICA DE CLASES BASE Y AVANZADAS
        // Modifica el daño final dependiendo de la clase y subclase seleccionada.
        // Se separa por tipo de daño (DamageClass) para evitar conflictos.
        // =================================================================

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
            float bonus = 0f;
            if (item.DamageType.CountsAsClass(DamageClass.Melee)) { bonus += (ptsMelee * 0.001f) + (yunqueMelDmg * 0.01f); if (claseElegida == 0) { if (claseAvanzada == -1) bonus += 0.05f; else if (claseAvanzada == 0) bonus += 0.10f; else if (claseAvanzada == 1) bonus += 0.05f; } }
            else if (item.DamageType.CountsAsClass(DamageClass.Ranged)) { bonus += (ptsRanger * 0.001f) + (yunqueRanDmg * 0.01f); if (claseElegida == 1) { if (claseAvanzada == -1) bonus += 0.05f; else if (claseAvanzada == 0) bonus += 0.20f; else if (claseAvanzada == 1) bonus += 0.10f; } }
            else if (item.DamageType.CountsAsClass(DamageClass.Magic)) { bonus += (ptsMagic * 0.001f) + (yunqueMagDmg * 0.01f); if (claseElegida == 2) { if (claseAvanzada == -1) bonus += 0.05f; else if (claseAvanzada == 0) bonus += 0.15f; else if (claseAvanzada == 1) bonus += 0.10f; } }
            else if (item.DamageType.CountsAsClass(DamageClass.Summon)) { bonus += (ptsSummon * 0.001f) + (yunqueSumDmg * 0.01f); if (claseElegida == 3) { if (claseAvanzada == -1) bonus += 0.05f; else if (claseAvanzada == 0) bonus += 0.10f; else if (claseAvanzada == 1) bonus += 0.15f; } }
            damage += bonus;
        }

        public override void ModifyWeaponCrit(Item item, ref float crit) {
            crit += (ptsSuerte * 0.1f) + yunqueCrit;
            if (claseElegida == 1) { if (claseAvanzada == -1 && item.DamageType.CountsAsClass(DamageClass.Ranged)) crit += 2f; if (claseAvanzada == 0 && item.DamageType.CountsAsClass(DamageClass.Ranged)) crit += 15f; if (claseAvanzada == 1 && item.DamageType.CountsAsClass(DamageClass.Ranged)) crit += 5f; }
        }

        public override void OnEnterWorld() { ultimoLevelCapVisto = levelCap; }
        public override void PostUpdate() { 
            if (levelCap > ultimoLevelCapVisto) { 
                string mensaje = Language.GetTextValue("Mods.RPGARGENTO.UI.AvisoProgreso", levelCap);
                Main.NewText(mensaje, new Color(255, 150, 50)); 
                ultimoLevelCapVisto = levelCap; 
            } 
        }
    }
}