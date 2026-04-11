using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input; // Necesario para detectar la tecla K
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using RPGARGENTO.UI;

namespace RPGARGENTO
{
    public class UISystem : ModSystem
    {
        // --- 1. LAS TRES INTERFACES ---
        internal UserInterface xpInterface;
        internal XPBar xpBar;

        internal UserInterface statsInterface;
        internal StatsMenu statsMenu;

        internal UserInterface yunqueInterface;
        internal YunqueUI yunqueUI;

        private bool kFuePresionada = false; // Evita que el menú titile si mantenés apretada la K

        public override void Load()
        {
            if (!Main.dedServ)
            {
                // Cargamos la Barra de XP (Siempre visible)
                xpBar = new XPBar();
                xpBar.Activate();
                xpInterface = new UserInterface();
                xpInterface.SetState(xpBar);

                // Cargamos el Menú de Stats (Arranca invisible)
                statsMenu = new StatsMenu();
                statsMenu.Activate();
                statsInterface = new UserInterface();
                statsInterface.SetState(null);

                // Cargamos el Yunque (Arranca invisible)
                yunqueUI = new YunqueUI();
                yunqueUI.Activate();
                yunqueInterface = new UserInterface();
                yunqueInterface.SetState(null);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            // --- LÓGICA DE LA TECLA K ---
            bool kApretadaAhora = Main.keyState.IsKeyDown(Keys.K);
            if (kApretadaAhora && !kFuePresionada)
            {
                if (statsInterface.CurrentState == null)
                    statsInterface.SetState(statsMenu); // Si está cerrado, lo abre
                else
                    statsInterface.SetState(null); // Si está abierto, lo cierra
            }
            kFuePresionada = kApretadaAhora;

            // Actualizamos las interfaces si están en pantalla
            if (xpInterface?.CurrentState != null) xpInterface.Update(gameTime);
            if (statsInterface?.CurrentState != null) statsInterface.Update(gameTime);
            if (yunqueInterface?.CurrentState != null) yunqueInterface.Update(gameTime);
        }


        // =================================================================
        // INYECCIÓN DE INTERFACES EN EL BUCLE DEL JUEGO
        // Busca la capa del "Mouse Text" de Terraria vanilla e inserta
        // nuestras tres interfaces personalizadas (XP, Stats, Yunque) justo 
        // por encima, para que los tooltips se dibujen correctamente.
        // =================================================================

        
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "RPGARGENTO: UI Completa",
                    delegate
                    {
                        // Dibujamos las tres interfaces juntas
                        if (xpInterface?.CurrentState != null) xpInterface.Draw(Main.spriteBatch, new GameTime());
                        if (statsInterface?.CurrentState != null) statsInterface.Draw(Main.spriteBatch, new GameTime());
                        if (yunqueInterface?.CurrentState != null) yunqueInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        // --- FUNCIONES DEL YUNQUE ---
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