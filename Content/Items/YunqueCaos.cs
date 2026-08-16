using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic; 
using RPGARGENTO.UI; 

namespace RPGARGENTO.Content.Items 
{
    public class YunqueCaos : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28; Item.height = 28; Item.useTime = 20; Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp; Item.rare = ItemRarityID.Orange; 
            Item.UseSound = SoundID.Item4; Item.maxStack = 9999; Item.consumable = false; 
        }

        public override bool CanUseItem(Player player) => ModContent.GetInstance<UISystem>().yunqueInterface.CurrentState == null;

        // =================================================================
        // POOL DE STATS POR RAREZA
        // Plata  = stats básicas (IDs 0-18 + 28-31, 33-35, 38)
        // Oro    = stats medias   (IDs 0-8, 12-13, 16, 19-21 + 32, 36-37, 41)
        // Prism. = stats raras    (IDs 0-8, 22-27 + 39-40)
        // =================================================================
        
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                int rareza = 1; 
                int rng = Main.rand.Next(100); 
                
                if (rng < 3) rareza = 3;       // 3% Prismático
                else if (rng < 10) rareza = 2;  // 7% Oro

                List<int> poolStats = new List<int>();

                // IDs: 0:MagDmg, 1:MelDmg, 2:RanDmg, 3:SumDmg, 4:KB, 5:Crit, 6:Def, 7:ManaMax, 8:LifeMax,
                // 9:JumpH, 10:AggroAdd, 11:AggroRed, 12:ManaCost, 13:AmmoCost, 14:Fish, 15:Grab, 16:MelSize,
                // 17:Respawn, 18:MineSpd, 19:MoveSpd, 20:JumpTimes, 21:WhipRng,
                // 22:RegenLife%, 23:RegenMana%, 24:ManaSickRes, 25:PotSickRes, 26:MaxMinion, 27:MaxSentinel,
                // 28:MelSpeed%, 29:RanSpeed%, 30:MagSpeed%, 31:ArmorPen%, 32:AllDmg%,
                // 33:Thorns%, 34:FlatLifeRegen, 35:FlatManaRegen, 36:FlightTime,
                // 37:MaxRunSpeed%, 38:BlockRange, 39:TileSpeed%, 40:WallSpeed%, 41:Luck%

                if (rareza == 1) // PLATA
                {
                    poolStats.AddRange(new int[]{ 0,1,2,3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18,
                                                  28, 29, 30, 31, 33, 34, 35, 38 });
                }
                else if (rareza == 2) // ORO
                {
                    poolStats.AddRange(new int[]{ 0,1,2,3, 4, 5, 6, 7, 8, 19, 12, 13, 20, 21, 16,
                                                  32, 36, 37, 41 });
                }
                else if (rareza == 3) // PRISMATICO
                {
                    poolStats.AddRange(new int[]{ 0,1,2,3, 4, 5, 6, 7, 8, 22, 23, 24, 25, 26, 27,
                                                  39, 40 });
                }

                int s1 = poolStats[Main.rand.Next(poolStats.Count)]; poolStats.Remove(s1);
                int s2 = poolStats[Main.rand.Next(poolStats.Count)]; poolStats.Remove(s2);
                int s3 = poolStats[Main.rand.Next(poolStats.Count)];

                ModContent.GetInstance<UISystem>().AbrirYunque(rareza, s1, s2, s3);
            }
            return true;
        }

        public override void AddRecipes() { CreateRecipe().AddIngredient(ItemID.Wood, 10).AddIngredient(ItemID.FallenStar, 1).AddTile(TileID.WorkBenches).Register(); }
    }
}
