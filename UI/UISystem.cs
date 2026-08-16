using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using RPGARGENTO.UI;

namespace RPGARGENTO
{
    public class UISystem : ModSystem
    {
        internal UserInterface xpInterface;
        internal XPBar xpBar;

        internal UserInterface statsInterface;
        internal StatsMenu statsMenu;

        internal UserInterface yunqueInterface;
        internal YunqueUI yunqueUI;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                xpBar = new XPBar();
                xpBar.Activate();
                xpInterface = new UserInterface();
                xpInterface.SetState(xpBar);

                statsMenu = new StatsMenu();
                statsMenu.Activate();
                statsInterface = new UserInterface();
                statsInterface.SetState(null);

                yunqueUI = new YunqueUI();
                yunqueUI.Activate();
                yunqueInterface = new UserInterface();
                yunqueInterface.SetState(null);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (RPGARGENTO.OpenStatsKey.JustPressed)
            {
                if (statsInterface.CurrentState == null)
                    statsInterface.SetState(statsMenu);
                else
                    statsInterface.SetState(null);
            }

            if (xpInterface?.CurrentState != null) xpInterface.Update(gameTime);
            if (statsInterface?.CurrentState != null) statsInterface.Update(gameTime);
            if (yunqueInterface?.CurrentState != null) yunqueInterface.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "RPGARGENTO: UI Completa",
                    delegate
                    {
                        if (xpInterface?.CurrentState != null) xpInterface.Draw(Main.spriteBatch, new GameTime());
                        if (statsInterface?.CurrentState != null) statsInterface.Draw(Main.spriteBatch, new GameTime());
                        if (yunqueInterface?.CurrentState != null) yunqueInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public void AbrirYunque(int rareza, int s1, int s2, int s3)
        {
            yunqueUI.Sorteo(rareza, s1, s2, s3);
            yunqueInterface.SetState(yunqueUI);
        }

        public void CerrarYunque()
        {
            yunqueInterface.SetState(null);
        }
    }
}
