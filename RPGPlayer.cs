using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace RPGARGENTO
{
    public class RPGPlayer : ModPlayer
    {
        // IDs 0-3: daños por clase
        public float yunqueMagDmg, yunqueMelDmg, yunqueRanDmg, yunqueSumDmg;
        // IDs 4-6: knockback, crit, defensa
        public float yunqueKnockback, yunqueCrit, yunqueDef;
        // IDs 7-8: mana, vida
        public int yunqueMana, yunqueLife;
        // IDs 9-11: salto, aggro+, aggro-
        public float yunqueJumpHeight;
        public int yunqueJumpTimes, yunqueAggro;
        // IDs 12-14: mana cost, ammo cost, pesca
        public float yunqueManaCost, yunqueAmmoCost;
        public int yunqueFish;
        // IDs 15-18: grab range, melee size, respawn, mining speed
        public float yunqueGrabRange, yunqueMeleeSize, yunqueRespawn, yunqueMiningSpeed;
        // IDs 19-21: move speed, jump times (duplicado), whip range
        public float yunqueMoveSpeed, yunqueWhipRange;
        // IDs 22-23: regen vida %, regen mana %
        public float yunqueRegenVida, yunqueRegenMana;
        // IDs 24-25: resistencias
        public float yunqueManaSickRes, yunquePotionSickRes;
        // IDs 26-27: minion, sentinel
        public int yunqueMaxMinion, yunqueMaxSentinel;
        // Sin ID: damage reduction
        public float yunqueDamageReduction;

        // ========== NUEVAS STATS (IDs 28-41) ==========
        // IDs 28-30: velocidad ataque
        public float yunqueMelSpeed, yunqueRanSpeed, yunqueMagSpeed;
        // ID 31: penetracion armadura
        public float yunqueArmorPen;
        // ID 32: daño generico (todas las clases)
        public float yunqueGenericDmg;
        // ID 33: espinas
        public float yunqueThorns;
        // IDs 34-35: regen plano
        public float yunqueFlatLifeRegen, yunqueFlatManaRegen;
        // ID 36: tiempo vuelo
        public float yunqueFlightTime;
        // ID 37: velocidad maxima
        public float yunqueMaxRunSpeed;
        // ID 38: rango construccion
        public float yunqueBlockRange;
        // IDs 39-40: velocidad colocacion
        public float yunqueTileSpeed, yunqueWallSpeed;
        // ID 41: suerte
        public float yunqueLuck;

        public int saltosRestantes = 0;
        public int GetTotalExtraJumps() => yunqueJumpTimes;

        public override void SaveData(TagCompound tag)
        {
            tag["y1"] = yunqueMagDmg; tag["y2"] = yunqueMelDmg; tag["y3"] = yunqueRanDmg; tag["y4"] = yunqueSumDmg;
            tag["y5"] = yunqueKnockback; tag["y6"] = yunqueCrit; tag["y7"] = yunqueDef; tag["y8"] = yunqueManaCost;
            tag["y9"] = yunqueAmmoCost; tag["y10"] = yunqueLife; tag["y11"] = yunqueMana; tag["y12"] = yunqueJumpTimes;
            tag["y13"] = yunqueAggro; tag["y14"] = yunqueFish; tag["y15"] = yunqueJumpHeight; tag["y16"] = yunqueGrabRange;
            tag["y17"] = yunqueMeleeSize; tag["y18"] = yunqueRespawn; tag["y19"] = yunqueMiningSpeed; tag["y20"] = yunqueMoveSpeed;
            tag["y21"] = yunqueWhipRange; tag["y22"] = yunqueRegenVida; tag["y23"] = yunqueRegenMana;
            tag["y24"] = yunqueManaSickRes; tag["y25"] = yunquePotionSickRes; tag["y26"] = yunqueMaxMinion;
            tag["y27"] = yunqueMaxSentinel; tag["y28"] = yunqueDamageReduction;
            tag["y29"] = yunqueMelSpeed; tag["y30"] = yunqueRanSpeed; tag["y31"] = yunqueMagSpeed;
            tag["y32"] = yunqueArmorPen; tag["y33"] = yunqueGenericDmg; tag["y34"] = yunqueThorns;
            tag["y35"] = yunqueFlatLifeRegen; tag["y36"] = yunqueFlatManaRegen; tag["y37"] = yunqueFlightTime;
            tag["y38"] = yunqueMaxRunSpeed; tag["y39"] = yunqueBlockRange; tag["y40"] = yunqueTileSpeed;
            tag["y41"] = yunqueWallSpeed; tag["y42"] = yunqueLuck;
        }

        public override void LoadData(TagCompound tag)
        {
            yunqueMagDmg = tag.GetFloat("y1"); yunqueMelDmg = tag.GetFloat("y2");
            yunqueRanDmg = tag.GetFloat("y3"); yunqueSumDmg = tag.GetFloat("y4");
            yunqueKnockback = tag.GetFloat("y5"); yunqueCrit = tag.GetFloat("y6");
            yunqueDef = tag.GetFloat("y7"); yunqueManaCost = tag.GetFloat("y8");
            yunqueAmmoCost = tag.GetFloat("y9"); yunqueLife = tag.GetInt("y10");
            yunqueMana = tag.GetInt("y11"); yunqueJumpTimes = tag.GetInt("y12");
            yunqueAggro = tag.GetInt("y13"); yunqueFish = tag.GetInt("y14");
            yunqueJumpHeight = tag.GetFloat("y15"); yunqueGrabRange = tag.GetFloat("y16");
            yunqueMeleeSize = tag.GetFloat("y17"); yunqueRespawn = tag.GetFloat("y18");
            yunqueMiningSpeed = tag.GetFloat("y19"); yunqueMoveSpeed = tag.GetFloat("y20");
            yunqueWhipRange = tag.GetFloat("y21"); yunqueRegenVida = tag.GetFloat("y22");
            yunqueRegenMana = tag.GetFloat("y23"); yunqueManaSickRes = tag.GetFloat("y24");
            yunquePotionSickRes = tag.GetFloat("y25"); yunqueMaxMinion = tag.GetInt("y26");
            yunqueMaxSentinel = tag.GetInt("y27"); yunqueDamageReduction = tag.GetFloat("y28");
            yunqueMelSpeed = tag.GetFloat("y29"); yunqueRanSpeed = tag.GetFloat("y30");
            yunqueMagSpeed = tag.GetFloat("y31"); yunqueArmorPen = tag.GetFloat("y32");
            yunqueGenericDmg = tag.GetFloat("y33"); yunqueThorns = tag.GetFloat("y34");
            yunqueFlatLifeRegen = tag.GetFloat("y35"); yunqueFlatManaRegen = tag.GetFloat("y36");
            yunqueFlightTime = tag.GetFloat("y37"); yunqueMaxRunSpeed = tag.GetFloat("y38");
            yunqueBlockRange = tag.GetFloat("y39"); yunqueTileSpeed = tag.GetFloat("y40");
            yunqueWallSpeed = tag.GetFloat("y41"); yunqueLuck = tag.GetFloat("y42");
        }

        public override void ResetEffects()
        {
            // === VIDA / MANA / DEFENSA ===
            Player.statLifeMax2 += yunqueLife;
            Player.statManaMax2 += yunqueMana;
            Player.statDefense += (int)yunqueDef;

            // === REGENERACION % ===
            float hpPS = Player.statLifeMax2 * (yunqueRegenVida / 100f);
            Player.lifeRegen += (int)(hpPS * 2);
            float mnPS = Player.statManaMax2 * (yunqueRegenMana / 100f);
            Player.manaRegenBonus += (int)(mnPS * 2);

            // === REGENERACION PLANA ===
            Player.lifeRegen += (int)yunqueFlatLifeRegen;
            Player.manaRegenBonus += (int)yunqueFlatManaRegen;

            // === MITIGACION ===
            Player.endurance += yunqueDamageReduction * 0.01f;
            Player.thorns += yunqueThorns * 0.01f;

            // === KNOCKBACK ===
            Player.GetKnockback(DamageClass.Generic) += yunqueKnockback * 0.01f;

            // === COSTOS ===
            Player.manaCost -= yunqueManaCost * 0.01f;
            Player.whipRangeMultiplier += yunqueWhipRange * 0.01f;

            // === MOVIMIENTO ===
            float moveBonus = yunqueMoveSpeed * 0.01f;
            Player.moveSpeed += moveBonus;
            Player.maxRunSpeed += moveBonus + yunqueMaxRunSpeed * 0.01f;
            Player.accRunSpeed += yunqueMaxRunSpeed * 0.01f;
            Player.jumpSpeedBoost += yunqueJumpHeight;

            // === VUELO ===
            Player.wingTimeMax += (int)(yunqueFlightTime * 60f);

            // === MINIONS ===
            Player.maxMinions += yunqueMaxMinion;
            Player.maxTurrets += yunqueMaxSentinel;

            // === UTILIDAD ===
            Player.aggro += yunqueAggro;
            Player.fishingSkill += yunqueFish;
            Player.blockRange += (int)yunqueBlockRange;
            Player.pickSpeed -= yunqueMiningSpeed * 0.01f;
            Player.tileSpeed += yunqueTileSpeed * 0.01f;
            Player.wallSpeed += yunqueWallSpeed * 0.01f;
            Player.luck += yunqueLuck * 0.01f;

            // === VELOCIDAD ATAQUE ===
            Player.GetAttackSpeed(DamageClass.Melee) += yunqueMelSpeed * 0.01f;
            Player.GetAttackSpeed(DamageClass.Ranged) += yunqueRanSpeed * 0.01f;
            Player.GetAttackSpeed(DamageClass.Magic) += yunqueMagSpeed * 0.01f;

            // === PENETRACION ARMADURA ===
            Player.GetArmorPenetration(DamageClass.Generic) += yunqueArmorPen * 0.01f;

            // === SALTOS EXTRAS ===
            if (GetTotalExtraJumps() > 0)
                Player.GetJumpState<RPGExtraJump>().Enable();
        }

        // Las resistencias a enfermedades se procesan en PostUpdateBuffs
        public override void PostUpdateBuffs()
        {
            if (yunqueManaSickRes > 0f && Player.HasBuff(BuffID.ManaSickness))
            {
                int idx = Player.FindBuffIndex(BuffID.ManaSickness);
                if (idx >= 0)
                    Player.buffTime[idx] -= (int)(yunqueManaSickRes * 0.01f * 60);
            }
            if (yunquePotionSickRes > 0f && Player.HasBuff(BuffID.PotionSickness))
            {
                int idx = Player.FindBuffIndex(BuffID.PotionSickness);
                if (idx >= 0)
                    Player.buffTime[idx] -= (int)(yunquePotionSickRes * 0.01f * 60);
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            float bonus = yunqueGenericDmg * 0.01f;
            if (item.DamageType.CountsAsClass(DamageClass.Melee)) bonus += yunqueMelDmg * 0.01f;
            else if (item.DamageType.CountsAsClass(DamageClass.Ranged)) bonus += yunqueRanDmg * 0.01f;
            else if (item.DamageType.CountsAsClass(DamageClass.Magic)) bonus += yunqueMagDmg * 0.01f;
            else if (item.DamageType.CountsAsClass(DamageClass.Summon)) bonus += yunqueSumDmg * 0.01f;
            damage += bonus;
        }

        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            crit += yunqueCrit;
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            return Main.rand.NextFloat() >= yunqueAmmoCost * 0.01f;
        }
    }
}
