using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;

namespace RPGARGENTO.UI
{
    public class XPBar : UIState
    {
        private UIPanel barBackground;
        private UIElement barFilling;
        private UIText xpText;

        // Variables para la optimización de memoria
        private int ultimaXpVoz = -1;
        private int ultimoNivelVoz = -1;

        public override void OnInitialize()
        {
            barBackground = new UIPanel();
            barBackground.Left.Set(20, 0f);
            barBackground.Top.Set(-60, 1f); 
            barBackground.Width.Set(200, 0f);
            barBackground.Height.Set(30, 0f);
            barBackground.SetPadding(0); 
            barBackground.BackgroundColor = Color.Transparent; 
            barBackground.BorderColor = Color.Transparent;
            Append(barBackground);

            barFilling = new UIElement();
            barFilling.Left.Set(5, 0f); 
            barFilling.Top.Set(5, 0f);
            barFilling.Height.Set(20, 0f); 
            barBackground.Append(barFilling);

            UIImage marcoXP = new UIImage(ModContent.Request<Texture2D>("RPGARGENTO/Assets/BordeXP"));
            marcoXP.Width.Set(200, 0f);
            marcoXP.Height.Set(30, 0f);
            marcoXP.Left.Set(0, 0f);
            marcoXP.Top.Set(0, 0f);
            marcoXP.IgnoresMouseInteraction = true;
            barBackground.Append(marcoXP);

            xpText = new UIText("lvl: 1, xp: 0/100"); 
            xpText.HAlign = 0.5f; 
            xpText.VAlign = 0.5f; 
            barBackground.Append(xpText);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var modPlayer = Main.LocalPlayer.GetModPlayer<RPGPlayer>();
            
            // Solo actualizamos el texto y la barra si hubo un cambio real
            if (modPlayer.xp != ultimaXpVoz || modPlayer.level != ultimoNivelVoz)
            {
                float quotient = (float)modPlayer.xp / modPlayer.maxXp;
                quotient = MathHelper.Clamp(quotient, 0f, 1f);

                barFilling.Width.Set(190 * quotient, 0f);
                barFilling.Recalculate();

                xpText.SetText($"lvl: {modPlayer.level}, xp: {modPlayer.xp}/{modPlayer.maxXp}");

                // Guardamos los valores para no volver a calcular en el próximo frame
                ultimaXpVoz = modPlayer.xp;
                ultimoNivelVoz = modPlayer.level;
            }

            Rectangle hitbox = barFilling.GetInnerDimensions().ToRectangle();
            spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, hitbox, Color.Cyan);
        }
    }
}