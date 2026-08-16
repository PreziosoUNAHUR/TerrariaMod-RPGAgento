using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.Localization;

namespace RPGARGENTO.UI
{
    public class XPBar : UIState
    {
        private UIText worldLevelText;

        public override void OnInitialize()
        {
            worldLevelText = new UIText("");
            worldLevelText.Left.Set(20, 0f);
            worldLevelText.Top.Set(-50, 1f);
            worldLevelText.TextColor = new Color(255, 200, 50);
            Append(worldLevelText);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            if (Main.gameMenu || Main.LocalPlayer == null || !Main.LocalPlayer.active) return;

            int worldLevel = RPGGlobalNPC.GetWorldLevel();
            worldLevelText.SetText(Language.GetTextValue("Mods.RPGARGENTO.UI.MundoNivel", worldLevel));
        }
    }
}
