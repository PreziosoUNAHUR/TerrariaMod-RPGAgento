using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace RPGARGENTO
{
    // Esta es tu clase principal (la que hereda de Mod)
    public class RPGARGENTO : Mod
    {
        // 1. Declaramos la variable de la tecla acá, suelta adentro de la clase
        public static ModKeybind OpenStatsKey;

        // 2. Usamos Load() para registrar la tecla cuando el mod arranca
        public override void Load()
        {
            // Le decimos al juego que la tecla por defecto va a ser la "K"
            OpenStatsKey = KeybindLoader.RegisterKeybind(this, "Abrir Menu RPG", "K");
        }

        // 3. Usamos Unload() para limpiarla cuando cerramos el mod
        public override void Unload()
        {
            OpenStatsKey = null;
        }
    }
}