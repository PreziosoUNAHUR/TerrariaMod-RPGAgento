using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RPGARGENTO.Content.Items
{
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class espadita : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.RPGARGENTO.hjson' file.
		public override void SetDefaults()
		{
			Item.damage = 150;
			Item.DamageType = DamageClass.Melee;
			Item.width = 100;
			Item.height = 100;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Expert;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.JestersArrow;
			Item.shootSpeed = 10f;
			Item.expert = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
