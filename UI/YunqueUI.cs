using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.Localization;
using RPGARGENTO.Content.Items;

namespace RPGARGENTO.UI
{
    public class YunqueUI : UIState
    {
        private UIPanel container;
        private UIPanel[] cards = new UIPanel[3];
        private UIText[] statTexts = new UIText[3];
        private UIText[] valueTexts = new UIText[3];
        private UIText titleText;
        private UIText instructionText;
        private int rareza;
        private int[] opciones = new int[3];

        public override void OnInitialize()
        {
            container = new UIPanel();
            container.Width.Set(640, 0f);
            container.Height.Set(400, 0f);
            container.HAlign = 0.5f;
            container.VAlign = 0.5f;
            container.BackgroundColor = new Color(8, 18, 38, 235);
            container.BorderColor = new Color(50, 70, 110, 180);
            container.SetPadding(0);
            Append(container);

            titleText = new UIText("", 1.1f);
            titleText.HAlign = 0.5f;
            titleText.Top.Set(20, 0f);
            titleText.TextColor = Color.Gold;
            container.Append(titleText);

            instructionText = new UIText("", 0.75f);
            instructionText.HAlign = 0.5f;
            instructionText.Top.Set(342, 0f);
            instructionText.TextColor = new Color(120, 130, 160);
            container.Append(instructionText);

            for (int i = 0; i < 3; i++)
            {
                int index = i;

                cards[i] = new UIPanel();
                cards[i].Width.Set(170, 0f);
                cards[i].Height.Set(240, 0f);
                cards[i].Top.Set(75, 0f);
                cards[i].Left.Set(35 + (i * 195), 0f);
                cards[i].BackgroundColor = new Color(15, 25, 50, 220);
                cards[i].BorderColor = new Color(60, 80, 120, 180);
                cards[i].SetPadding(10);
                cards[i].OnLeftClick += (evt, element) => Seleccionar(index);
                container.Append(cards[i]);

                statTexts[i] = new UIText("", 0.9f);
                statTexts[i].HAlign = 0.5f;
                statTexts[i].VAlign = 0.35f;
                statTexts[i].TextColor = Color.White;
                cards[i].Append(statTexts[i]);

                valueTexts[i] = new UIText("---", 1.2f);
                valueTexts[i].HAlign = 0.5f;
                valueTexts[i].VAlign = 0.6f;
                valueTexts[i].TextColor = Color.Gold;
                cards[i].Append(valueTexts[i]);
            }
        }

        public void Sorteo(int r, int s1, int s2, int s3)
        {
            rareza = r;
            opciones[0] = s1;
            opciones[1] = s2;
            opciones[2] = s3;

            string rarezaNombre = r == 1 ? "Plata" : r == 2 ? "Oro" : "Prismático";
            Color rarezaColor = r == 1 ? new Color(180, 180, 200) : r == 2 ? new Color(218, 165, 32) : new Color(140, 180, 255);
            titleText.SetText(Language.GetTextValue("Mods.RPGARGENTO.UI.TituloYunque", rarezaNombre));
            titleText.TextColor = rarezaColor;

            instructionText.SetText(Language.GetTextValue("Mods.RPGARGENTO.UI.ClickSeleccionar"));

            for (int i = 0; i < 3; i++)
            {
                cards[i].BorderColor = r == 1 ? new Color(100, 120, 150, 180) :
                                      r == 2 ? new Color(180, 140, 30, 200) :
                                      new Color(100, 60, 180, 200);
                cards[i].BackgroundColor = r == 1 ? new Color(18, 25, 45, 220) :
                                           r == 2 ? new Color(30, 25, 15, 220) :
                                           new Color(20, 10, 40, 220);

                int id = opciones[i];
                statTexts[i].SetText(Language.GetTextValue($"Mods.RPGARGENTO.Yunque.Stat{id}"));

                string t = "";
                if (r == 1)
                {
                    if (id <= 5 || id == 12 || id == 13) t = "+0.01%";
                    else if (id == 6) t = "+0.25";
                    else if (id == 15 || id == 16 || id == 18) t = "+1%";
                    else if (id == 17) t = "-1%";
                    else if (id == 11) t = "-1";
                    else if (id == 28 || id == 29 || id == 30 || id == 31 || id == 33) t = "+0.01%";
                    else if (id == 34 || id == 35 || id == 38) t = "+0.25";
                    else t = "+1";
                }
                else if (r == 2)
                {
                    if (id <= 5 || id == 12 || id == 13 || id == 21 || id == 16) t = "+0.1%";
                    else if (id == 6) t = "+0.50";
                    else if (id == 7 || id == 8) t = "+3";
                    else if (id == 19) t = "+1%";
                    else if (id == 32 || id == 37 || id == 41) t = "+0.1%";
                    else if (id == 36) t = "+1.0s";
                    else t = "+1";
                }
                else if (r == 3)
                {
                    if (id <= 5) t = "+1%";
                    else if (id == 6) t = "+1";
                    else if (id == 7 || id == 8) t = "+5";
                    else if (id == 22 || id == 23) t = "+0.1%";
                    else if (id == 24 || id == 25) t = "+10%";
                    else if (id == 39 || id == 40) t = "+1%";
                    else t = "+1";
                }
                valueTexts[i].SetText(t);
            }

            if (r == 2)
                SoundEngine.PlaySound(SoundID.Item4);
            else if (r == 3)
                SoundEngine.PlaySound(SoundID.Item29);
        }

        private void Seleccionar(int i)
        {
            var p = Main.LocalPlayer.GetModPlayer<RPGPlayer>();
            int id = opciones[i];
            float val = 0f;
            int valInt = 0;
            bool esInt = false;
            bool esNegativo = false;

            switch (rareza)
            {
                case 1: // PLATA
                    if (id <= 5 || id == 12 || id == 13) val = 0.01f;
                    else if (id == 6) val = 0.25f;
                    else if (id == 7 || id == 8 || id == 10 || id == 14) { valInt = 1; esInt = true; }
                    else if (id == 9) val = 1f;
                    else if (id == 11) { valInt = 1; esInt = true; esNegativo = true; }
                    else if (id == 15 || id == 16 || id == 17 || id == 18) val = 1f;
                    else if (id == 28 || id == 29 || id == 30 || id == 31 || id == 33) val = 0.01f;
                    else if (id == 34 || id == 35 || id == 38) val = 0.25f;
                    break;

                case 2: // ORO
                    if (id <= 5 || id == 12 || id == 13 || id == 21) val = 0.1f;
                    else if (id == 6) val = 0.5f;
                    else if (id == 7 || id == 8 || id == 20) { valInt = 3; esInt = true; }
                    else if (id == 16) val = 0.1f;
                    else if (id == 19) val = 1f;
                    else if (id == 32 || id == 37 || id == 41) val = 0.1f;
                    else if (id == 36) val = 1f;
                    break;

                case 3: // PRISMATICO
                    if (id <= 5) val = 1f;
                    else if (id == 6) val = 1f;
                    else if (id == 7 || id == 8) { valInt = 5; esInt = true; }
                    else if (id == 22 || id == 23) val = 0.1f;
                    else if (id == 24 || id == 25) val = 10f;
                    else if (id == 26 || id == 27) { valInt = 1; esInt = true; }
                    else if (id == 39 || id == 40) val = 1f;
                    break;
            }

            // Aplicar la stat según el ID
            if (esNegativo && esInt)
                valInt = -valInt;

            if (esInt)
            {
                switch (id)
                {
                    case 7: p.yunqueMana += valInt; break;
                    case 8: p.yunqueLife += valInt; break;
                    case 10: p.yunqueAggro += valInt; break;
                    case 11: p.yunqueAggro -= valInt; break; // ya es negativo
                    case 14: p.yunqueFish += valInt; break;
                    case 20: p.yunqueJumpTimes += valInt; break;
                    case 26: p.yunqueMaxMinion += valInt; break;
                    case 27: p.yunqueMaxSentinel += valInt; break;
                }
            }
            else
            {
                switch (id)
                {
                    case 0: p.yunqueMagDmg += val; break;
                    case 1: p.yunqueMelDmg += val; break;
                    case 2: p.yunqueRanDmg += val; break;
                    case 3: p.yunqueSumDmg += val; break;
                    case 4: p.yunqueKnockback += val; break;
                    case 5: p.yunqueCrit += val; break;
                    case 6: p.yunqueDef += val; break;
                    case 9: p.yunqueJumpHeight += val; break;
                    case 12: p.yunqueManaCost += val; break;
                    case 13: p.yunqueAmmoCost += val; break;
                    case 15: p.yunqueGrabRange += val; break;
                    case 16: p.yunqueMeleeSize += val; break;
                    case 17: p.yunqueRespawn += val; break;
                    case 18: p.yunqueMiningSpeed += val; break;
                    case 19: p.yunqueMoveSpeed += val; break;
                    case 21: p.yunqueWhipRange += val; break;
                    case 22: p.yunqueRegenVida += val; break;
                    case 23: p.yunqueRegenMana += val; break;
                    case 24: p.yunqueManaSickRes += val; break;
                    case 25: p.yunquePotionSickRes += val; break;
                    case 28: p.yunqueMelSpeed += val; break;
                    case 29: p.yunqueRanSpeed += val; break;
                    case 30: p.yunqueMagSpeed += val; break;
                    case 31: p.yunqueArmorPen += val; break;
                    case 32: p.yunqueGenericDmg += val; break;
                    case 33: p.yunqueThorns += val; break;
                    case 34: p.yunqueFlatLifeRegen += val; break;
                    case 35: p.yunqueFlatManaRegen += val; break;
                    case 36: p.yunqueFlightTime += val; break;
                    case 37: p.yunqueMaxRunSpeed += val; break;
                    case 38: p.yunqueBlockRange += val; break;
                    case 39: p.yunqueTileSpeed += val; break;
                    case 40: p.yunqueWallSpeed += val; break;
                    case 41: p.yunqueLuck += val; break;
                }
            }

            Main.LocalPlayer.ConsumeItem(ModContent.ItemType<YunqueCaos>());
            ModContent.GetInstance<UISystem>().CerrarYunque();
        }
    }
}
