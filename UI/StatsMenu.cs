using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic; 

namespace RPGARGENTO.UI
{
    public class StatsMenu : UIState
    {
        private UIPanel panel; private UIText titleText; private int paginaActual = 0;
        private UIText[] statTextsPts = new UIText[10]; private UIText[] hoverTexts = new UIText[10]; 
        private int[] puntosTemporales = new int[10];
        private UIPanel[] btnMinusArray = new UIPanel[10]; private UIPanel[] btnPlusArray = new UIPanel[10];
        
        private UIPanel btnReset; private UIPanel btnAceptar; private UIPanel btnDebugXP; private UIPanel btnDebugLevel; 

        private UIPanel panelScroll; private UIList listaStats; private UIScrollbar scrollbar; 
        private List<UIText> textosEstadisticasVisuales = new List<UIText>();

        private UIText textoBloqueo; private UIPanel[] btnClases = new UIPanel[4]; private UIText textoClaseActual; private UIText tooltipClase; 
        private UIPanel[] btnClaseAvanzada = new UIPanel[2]; private UIText tooltipAdv; private UIText textoLevelCap; 

        private int ultimaClaseDibujada = -1;

        public override void OnInitialize()
        {
            panel = new UIPanel(); panel.Width.Set(580, 0f); panel.Height.Set(450, 0f); panel.HAlign = 0.5f; panel.VAlign = 0.5f; panel.BackgroundColor = new Color(15, 45, 80, 230); Append(panel);
            titleText = new UIText(""); titleText.Top.Set(15, 0f); titleText.HAlign = 0.5f; panel.Append(titleText);

            UIPanel btnPrev = new UIPanel(); btnPrev.Width.Set(30, 0f); btnPrev.Height.Set(30, 0f); btnPrev.Top.Set(10, 0f); btnPrev.Left.Set(20, 0f); btnPrev.Append(new UIText("<") { HAlign = 0.5f, VAlign = 0.5f });
            btnPrev.OnLeftClick += (evt, element) => { paginaActual--; if (paginaActual < 0) paginaActual = 2; RefrescarUI(); }; panel.Append(btnPrev);
            
            UIPanel btnNext = new UIPanel(); btnNext.Width.Set(30, 0f); btnNext.Height.Set(30, 0f); btnNext.Top.Set(10, 0f); btnNext.Left.Set(530, 0f); btnNext.Append(new UIText(">") { HAlign = 0.5f, VAlign = 0.5f });
            btnNext.OnLeftClick += (evt, element) => { paginaActual++; if (paginaActual > 2) paginaActual = 0; RefrescarUI(); }; panel.Append(btnNext);

            for (int i = 0; i < 10; i++) { statTextsPts[i] = new UIText(""); statTextsPts[i].Top.Set(60 + (i * 28), 0f); statTextsPts[i].Left.Set(30, 0f); panel.Append(statTextsPts[i]); CreateStatRow(i, 60 + (i * 28)); }
            ConfigurarBotonesFinales();

            panelScroll = new UIPanel(); panelScroll.Width.Set(540, 0f); panelScroll.Height.Set(340, 0f); panelScroll.Top.Set(50, 0f); panelScroll.Left.Set(10, 0f); panelScroll.BackgroundColor = Color.Transparent; panelScroll.BorderColor = Color.Transparent; panel.Append(panelScroll);
            listaStats = new UIList(); listaStats.Width.Set(510, 0f); listaStats.Height.Set(0, 1f); listaStats.SetPadding(0); 
            listaStats.ManualSortMethod = (list) => { }; 
            panelScroll.Append(listaStats);
            scrollbar = new UIScrollbar(); scrollbar.SetView(100f, 1000f); scrollbar.Height.Set(0, 1f); scrollbar.HAlign = 1f; panelScroll.Append(scrollbar); listaStats.SetScrollbar(scrollbar); 

            AddHeaderToList(Language.GetText("Mods.RPGARGENTO.UI.CatAtaque"), new Color(220, 220, 220)); for(int i=0; i<8; i++) AddStatElement();
            AddHeaderToList(Language.GetText("Mods.RPGARGENTO.UI.CatDefensa"), new Color(220, 220, 220)); for(int i=0; i<6; i++) AddStatElement();
            AddHeaderToList(Language.GetText("Mods.RPGARGENTO.UI.CatExtras"), new Color(220, 220, 220)); for(int i=0; i<16; i++) AddStatElement();

            ConfigurarPaginaClases();
            textoLevelCap = new UIText(""); textoLevelCap.Left.Set(30, 0f); textoLevelCap.Top.Set(410, 0f); textoLevelCap.TextColor = new Color(255, 200, 50); panel.Append(textoLevelCap);
        }

        private void CreateStatRow(int index, float top) {
            btnMinusArray[index] = new UIPanel(); btnMinusArray[index].Width.Set(25, 0f); btnMinusArray[index].Height.Set(25, 0f); btnMinusArray[index].Top.Set(top - 2, 0f); btnMinusArray[index].BackgroundColor = new Color(150, 70, 70);
            btnMinusArray[index].Append(new UIText("-") { HAlign = 0.5f, VAlign = 0.5f }); btnMinusArray[index].OnLeftClick += (evt, element) => { if (puntosTemporales[index] > 0) puntosTemporales[index]--; RefrescarUI(); }; panel.Append(btnMinusArray[index]);

            btnPlusArray[index] = new UIPanel(); btnPlusArray[index].Width.Set(25, 0f); btnPlusArray[index].Height.Set(25, 0f); btnPlusArray[index].Top.Set(top - 2, 0f); btnPlusArray[index].BackgroundColor = new Color(70, 150, 70);
            btnPlusArray[index].Append(new UIText("+") { HAlign = 0.5f, VAlign = 0.5f }); btnPlusArray[index].OnLeftClick += (evt, element) => { var p = Main.LocalPlayer.GetModPlayer<RPGPlayer>(); int t = 0; for(int j=0; j<10; j++) t += puntosTemporales[j]; if (p.skillPoints - t > 0) puntosTemporales[index]++; RefrescarUI(); }; panel.Append(btnPlusArray[index]);

            hoverTexts[index] = new UIText(""); hoverTexts[index].Top.Set(top, 0f); hoverTexts[index].TextColor = Color.Yellow; panel.Append(hoverTexts[index]);
            
            btnPlusArray[index].OnMouseOver += (evt, element) => { 
                string hoverKey = index == 0 ? "HoverVida" : index == 1 ? "HoverMana" : index == 2 ? "HoverDef" : index == 7 ? "HoverCrit" : index == 8 ? "HoverKb" : index == 9 ? "HoverVel" : "HoverDmg";
                hoverTexts[index].SetText(Language.GetTextValue($"Mods.RPGARGENTO.UI.{hoverKey}")); 
            }; 
            btnPlusArray[index].OnMouseOut += (evt, element) => { hoverTexts[index].SetText(""); };
        }

        private void ConfigurarBotonesFinales() {
            btnDebugXP = new UIPanel(); btnDebugXP.Width.Set(80, 0f); btnDebugXP.Height.Set(30, 0f); btnDebugXP.Left.Set(20, 0f); btnDebugXP.Top.Set(400, 0f); btnDebugXP.BackgroundColor = new Color(80, 50, 150); btnDebugXP.Append(new UIText("+1 XP") { HAlign = 0.5f, VAlign = 0.5f }); btnDebugXP.OnLeftClick += (evt, element) => { Main.LocalPlayer.GetModPlayer<RPGPlayer>().skillPoints++; RefrescarUI(); }; panel.Append(btnDebugXP);
            btnDebugLevel = new UIPanel(); btnDebugLevel.Width.Set(80, 0f); btnDebugLevel.Height.Set(30, 0f); btnDebugLevel.Left.Set(110, 0f); btnDebugLevel.Top.Set(400, 0f); btnDebugLevel.BackgroundColor = new Color(150, 80, 50); btnDebugLevel.Append(new UIText("+1 Lvl") { HAlign = 0.5f, VAlign = 0.5f }); btnDebugLevel.OnLeftClick += (evt, element) => { var p = Main.LocalPlayer.GetModPlayer<RPGPlayer>(); if(p.level < p.levelCap) { p.level++; p.skillPoints++; } RefrescarUI(); }; panel.Append(btnDebugLevel);
            
            btnReset = new UIPanel(); btnReset.Width.Set(80, 0f); btnReset.Height.Set(30, 0f); btnReset.Left.Set(350, 0f); btnReset.BackgroundColor = new Color(180, 60, 60); 
            btnReset.Append(new UIText(Language.GetText("Mods.RPGARGENTO.UI.BtnReset")) { HAlign = 0.5f, VAlign = 0.5f }); 
            btnReset.OnLeftClick += (evt, element) => { var p = Main.LocalPlayer.GetModPlayer<RPGPlayer>(); p.skillPoints += p.ptsVida + p.ptsMana + p.ptsArmadura + p.ptsMelee + p.ptsRanger + p.ptsMagic + p.ptsSummon + p.ptsSuerte + p.ptsKnockback + p.ptsMoveSpeed; p.ptsVida = 0; p.ptsMana = 0; p.ptsArmadura = 0; p.ptsMelee = 0; p.ptsRanger = 0; p.ptsMagic = 0; p.ptsSummon = 0; p.ptsSuerte = 0; p.ptsKnockback = 0; p.ptsMoveSpeed = 0; for(int i=0; i<10; i++) puntosTemporales[i] = 0; RefrescarUI(); }; panel.Append(btnReset);
            
            btnAceptar = new UIPanel(); btnAceptar.Width.Set(100, 0f); btnAceptar.Height.Set(30, 0f); btnAceptar.Left.Set(440, 0f); btnAceptar.BackgroundColor = new Color(50, 180, 50); 
            btnAceptar.Append(new UIText(Language.GetText("Mods.RPGARGENTO.UI.BtnAceptar")) { HAlign = 0.5f, VAlign = 0.5f }); 
            btnAceptar.OnLeftClick += (evt, element) => { var p = Main.LocalPlayer.GetModPlayer<RPGPlayer>(); int gastado = 0; for(int i=0; i<10; i++) gastado += puntosTemporales[i]; if (gastado > 0) { p.ptsVida += puntosTemporales[0]; p.ptsMana += puntosTemporales[1]; p.ptsArmadura += puntosTemporales[2]; p.ptsMelee += puntosTemporales[3]; p.ptsRanger += puntosTemporales[4]; p.ptsMagic += puntosTemporales[5]; p.ptsSummon += puntosTemporales[6]; p.ptsSuerte += puntosTemporales[7]; p.ptsKnockback += puntosTemporales[8]; p.ptsMoveSpeed += puntosTemporales[9]; p.skillPoints -= gastado; for(int i=0; i<10; i++) puntosTemporales[i] = 0; RefrescarUI(); } }; panel.Append(btnAceptar);
        }

        private void ConfigurarPaginaClases() {
            textoBloqueo = new UIText(""); textoBloqueo.TextColor = Color.Red; textoBloqueo.HAlign = 0.5f; textoBloqueo.Top.Set(200, 0f); panel.Append(textoBloqueo);
            tooltipClase = new UIText(""); tooltipClase.TextColor = Color.Yellow; tooltipClase.HAlign = 0.5f; tooltipClase.Top.Set(260, 0f); panel.Append(tooltipClase);
            
            string[] nBase = { "Melee", "Ranger", "Magic", "Summon" };
            for(int i = 0; i < 4; i++) {
                int index = i; btnClases[i] = new UIPanel(); btnClases[i].Width.Set(100, 0f); btnClases[i].Height.Set(100, 0f); btnClases[i].Left.Set(30 + (i * 125), 0f); btnClases[i].BackgroundColor = new Color(40, 40, 60);
                btnClases[i].OnLeftClick += (evt, element) => { Main.LocalPlayer.GetModPlayer<RPGPlayer>().claseElegida = index; RefrescarUI(); };
                btnClases[i].OnMouseOver += (evt, element) => { 
                    string key = index == 0 ? "BonoBaseMelee" : index == 1 ? "BonoBaseRanger" : index == 2 ? "BonoBaseMagic" : "BonoBaseSummon";
                    tooltipClase.SetText(Language.GetTextValue($"Mods.RPGARGENTO.UI.{key}"));
                }; 
                btnClases[i].OnMouseOut += (evt, element) => { tooltipClase.SetText(""); };
                UIImage img = new UIImage(ModContent.Request<Texture2D>("RPGARGENTO/Assets/Clase" + nBase[i])); img.Width.Set(80, 0f); img.Height.Set(80, 0f); img.Left.Set(-3, 0f); img.Top.Set(-3, 0f); btnClases[i].Append(img); panel.Append(btnClases[i]);
            }
            textoClaseActual = new UIText(""); textoClaseActual.HAlign = 0.5f; textoClaseActual.Top.Set(60, 0f); panel.Append(textoClaseActual);

            tooltipAdv = new UIText(""); tooltipAdv.TextColor = Color.Yellow; tooltipAdv.HAlign = 0.5f; tooltipAdv.Top.Set(320, 0f); panel.Append(tooltipAdv);
            for(int i = 0; i < 2; i++) {
                int index = i; btnClaseAvanzada[i] = new UIPanel(); btnClaseAvanzada[i].Width.Set(100, 0f); btnClaseAvanzada[i].Height.Set(100, 0f); btnClaseAvanzada[i].Left.Set(160 + (i * 150), 0f); btnClaseAvanzada[i].BackgroundColor = new Color(80, 40, 60);
                btnClaseAvanzada[i].OnLeftClick += (evt, element) => { Main.LocalPlayer.GetModPlayer<RPGPlayer>().claseAvanzada = index; RefrescarUI(); };
                btnClaseAvanzada[i].OnMouseOut += (evt, element) => { tooltipAdv.SetText(""); }; panel.Append(btnClaseAvanzada[i]);
            }
        }

        public override void Update(GameTime gameTime) { base.Update(gameTime); }
        public override void OnActivate() { base.OnActivate(); RefrescarUI(); }

        public void RefrescarUI()
        {
            if (Main.gameMenu || Main.LocalPlayer == null || !Main.LocalPlayer.active) return;

            var p = Main.LocalPlayer.GetModPlayer<RPGPlayer>(); titleText.TextColor = Main.DiscoColor; 
            panelScroll.Top.Set(paginaActual == 1 ? 50f : -1000f, 0f); 

            if (paginaActual == 0) {
                int totalTemp = 0; for(int i=0; i<10; i++) totalTemp += puntosTemporales[i];
                
                titleText.SetText(Language.GetTextValue("Mods.RPGARGENTO.UI.PuntosDisp", p.skillPoints - totalTemp));
                
                btnReset.Top.Set(400, 0f); btnAceptar.Top.Set(400, 0f); btnDebugXP.Top.Set(400, 0f); btnDebugLevel.Top.Set(400, 0f);
                textoBloqueo.Top.Set(-1000, 0f); textoClaseActual.Top.Set(-1000, 0f); tooltipClase.Top.Set(-1000, 0f); tooltipAdv.Top.Set(-1000, 0f); textoLevelCap.Top.Set(-1000, 0f);
                for(int i = 0; i < 4; i++) btnClases[i].Top.Set(-1000, 0f); for(int i = 0; i < 2; i++) btnClaseAvanzada[i].Top.Set(-1000, 0f);
                for(int i = 0; i < 10; i++) { statTextsPts[i].Left.Set(30, 0f); btnMinusArray[i].Left.Set(260, 0f); btnPlusArray[i].Left.Set(295, 0f); hoverTexts[i].Left.Set(330, 0f); }

                statTextsPts[0].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.UI.StatVida")}: +{p.ptsVida * 5}" + (puntosTemporales[0] > 0 ? $" (+{puntosTemporales[0] * 5})" : ""));
                statTextsPts[1].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.UI.StatMana")}: +{p.ptsMana * 5}" + (puntosTemporales[1] > 0 ? $" (+{puntosTemporales[1] * 5})" : ""));
                statTextsPts[2].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.UI.StatArmadura")}: +{p.ptsArmadura * 1}" + (puntosTemporales[2] > 0 ? $" (+{puntosTemporales[2] * 1})" : ""));
                statTextsPts[3].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.UI.StatMelee")}: +{p.ptsMelee * 0.1f:F1}%" + (puntosTemporales[3] > 0 ? $" (+{puntosTemporales[3] * 0.1f:F1}%)" : ""));
                statTextsPts[4].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.UI.StatRanger")}: +{p.ptsRanger * 0.1f:F1}%" + (puntosTemporales[4] > 0 ? $" (+{puntosTemporales[4] * 0.1f:F1}%)" : ""));
                statTextsPts[5].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.UI.StatMagic")}: +{p.ptsMagic * 0.1f:F1}%" + (puntosTemporales[5] > 0 ? $" (+{puntosTemporales[5] * 0.1f:F1}%)" : ""));
                statTextsPts[6].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.UI.StatSummon")}: +{p.ptsSummon * 0.1f:F1}%" + (puntosTemporales[6] > 0 ? $" (+{puntosTemporales[6] * 0.1f:F1}%)" : ""));
                statTextsPts[7].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.UI.StatSuerte")}: +{p.ptsSuerte * 0.1f:F1}%" + (puntosTemporales[7] > 0 ? $" (+{puntosTemporales[7] * 0.1f:F1}%)" : ""));
                statTextsPts[8].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.UI.StatKnockback")}: +{p.ptsKnockback * 0.1f:F1}%" + (puntosTemporales[8] > 0 ? $" (+{puntosTemporales[8] * 0.1f:F1}%)" : ""));
                statTextsPts[9].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.UI.StatVelMov")}: +{p.ptsMoveSpeed * 1f}%" + (puntosTemporales[9] > 0 ? $" (+{puntosTemporales[9] * 1f}%)" : ""));
            }
            else if (paginaActual == 1) 
            {
                titleText.SetText(""); 
                btnReset.Top.Set(-1000, 0f); btnAceptar.Top.Set(-1000, 0f); btnDebugXP.Top.Set(-1000, 0f); btnDebugLevel.Top.Set(-1000, 0f);
                textoBloqueo.Top.Set(-1000, 0f); textoClaseActual.Top.Set(-1000, 0f); tooltipClase.Top.Set(-1000, 0f); tooltipAdv.Top.Set(-1000, 0f);
                for(int i = 0; i < 4; i++) btnClases[i].Top.Set(-1000, 0f); for(int i = 0; i < 2; i++) btnClaseAvanzada[i].Top.Set(-1000, 0f);
                for(int i = 0; i < 10; i++) { statTextsPts[i].Left.Set(-1000, 0f); btnMinusArray[i].Left.Set(-1000, 0f); btnPlusArray[i].Left.Set(-1000, 0f); hoverTexts[i].Left.Set(-1000, 0f); }
                
                textoLevelCap.Top.Set(410, 0f); textoLevelCap.SetText(Language.GetTextValue("Mods.RPGARGENTO.UI.NivelMundo", p.levelCap));

                float cMag=0, cMel=0, cRan=0, cSum=0, cKb=0, cCrit=0, cDef=0, cMana=0, cHP=0, cSize=0, cManaCost=0, cAmmoCost=0, cWhip=0, cMove=0, cJumpT=0, cJumpH=0, cAggro=0, cRegenHP=0, cRegenMN=0, cMinion=0, cSentinel=0, cResPoc=0, cResMana=0;

                if (p.claseElegida == 0) {
                    if(p.claseAvanzada == -1) { cMel=5; cHP=10; cMove=10; cDef=5; cKb=2; }
                    else if(p.claseAvanzada == 0) { cMel=10; cHP=20; cMove=15; cRegenHP=2; cSize=10; cJumpT=1; }
                    else if(p.claseAvanzada == 1) { cMel=5; cHP=100; cDef=10; cKb=5; cResPoc=10; }
                } else if (p.claseElegida == 1) {
                    if(p.claseAvanzada == -1) { cRan=5; cMove=15; cDef=2; cJumpH=2; cAmmoCost=5; cAggro=-2; cCrit=2; }
                    else if(p.claseAvanzada == 0) { cRan=20; cAmmoCost=20; cCrit=15; cAggro=-10; }
                    else if(p.claseAvanzada == 1) { cRan=10; cMove=10; cDef=5; cAmmoCost=10; cCrit=5; }
                } else if (p.claseElegida == 2) {
                    if(p.claseAvanzada == -1) { cMag=5; cMana=10; cMove=10; cDef=2; cRegenMN=2; cManaCost=10; }
                    else if(p.claseAvanzada == 0) { cMag=15; cMana=20; cResMana=10; cManaCost=20; }
                    else if(p.claseAvanzada == 1) { cMag=10; cHP=10; cMana=20; cMove=10; cRegenMN=10; cResMana=10; }
                } else if (p.claseElegida == 3) {
                    if(p.claseAvanzada == -1) { cSum=5; cHP=10; cMove=5; cDef=5; cMinion=1; cWhip=2; }
                    else if(p.claseAvanzada == 0) { cSum=10; cDef=10; cMinion=2; cSentinel=2; }
                    else if(p.claseAvanzada == 1) { cSum=15; cHP=20; cMove=10; cDef=10; cMinion=-1; cWhip=20; cJumpH=5; cAggro=-2; cJumpT=1; }
                }

                // TODOS LOS TEXTOS TRADUCIDOS DINÁMICAMENTE
                textosEstadisticasVisuales[0].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat0")}: +{(p.ptsMagic * 0.1f) + p.yunqueMagDmg + cMag:F2}%"); textosEstadisticasVisuales[0].TextColor = Color.Cyan;
                textosEstadisticasVisuales[1].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat1")}: +{(p.ptsMelee * 0.1f) + p.yunqueMelDmg + cMel:F2}%"); textosEstadisticasVisuales[1].TextColor = Color.IndianRed;
                textosEstadisticasVisuales[2].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat2")}: +{(p.ptsRanger * 0.1f) + p.yunqueRanDmg + cRan:F2}%"); textosEstadisticasVisuales[2].TextColor = Color.LawnGreen;
                textosEstadisticasVisuales[3].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat3")}: +{(p.ptsSummon * 0.1f) + p.yunqueSumDmg + cSum:F2}%"); textosEstadisticasVisuales[3].TextColor = Color.PaleVioletRed;
                textosEstadisticasVisuales[4].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat4")}: +{(p.ptsKnockback * 0.1f) + p.yunqueKnockback + cKb:F2}%"); textosEstadisticasVisuales[4].TextColor = Color.LightSalmon;
                textosEstadisticasVisuales[5].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat26")}: {Main.LocalPlayer.maxMinions}"); textosEstadisticasVisuales[5].TextColor = Color.White;
                textosEstadisticasVisuales[6].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat27")}: {Main.LocalPlayer.maxTurrets}"); textosEstadisticasVisuales[6].TextColor = Color.White;
                textosEstadisticasVisuales[7].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat5")}: +{(p.ptsSuerte * 0.1f) + p.yunqueCrit + cCrit:F2}%"); textosEstadisticasVisuales[7].TextColor = Color.Orange;

                textosEstadisticasVisuales[8].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat6")}: +{(p.ptsArmadura * 1f) + p.yunqueDef + cDef}"); textosEstadisticasVisuales[8].TextColor = Color.LightGray;
                textosEstadisticasVisuales[9].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat7")}: +{(p.ptsMana * 5f) + p.yunqueMana + cMana}"); textosEstadisticasVisuales[9].TextColor = Color.Cyan;
                textosEstadisticasVisuales[10].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.UI.ReduccionDano")}: +{p.yunqueDamageReduction}%"); textosEstadisticasVisuales[10].TextColor = Color.DeepSkyBlue;
                textosEstadisticasVisuales[11].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat23")}: +{p.yunqueRegenMana + cRegenMN:F2}%"); textosEstadisticasVisuales[11].TextColor = Color.Cyan;
                textosEstadisticasVisuales[12].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat22")}: +{p.yunqueRegenVida + cRegenHP:F2}%"); textosEstadisticasVisuales[12].TextColor = Color.LimeGreen;
                textosEstadisticasVisuales[13].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat8")}: +{(p.ptsVida * 5f) + p.yunqueLife + cHP}"); textosEstadisticasVisuales[13].TextColor = Color.LimeGreen;

                textosEstadisticasVisuales[14].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat9")}: +{p.yunqueJumpHeight + cJumpH}"); textosEstadisticasVisuales[14].TextColor = Color.Gray;
                textosEstadisticasVisuales[15].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat10")}: +{p.yunqueAggro + cAggro}"); textosEstadisticasVisuales[15].TextColor = Color.Gray;
                textosEstadisticasVisuales[16].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat20")}: +{p.yunqueJumpTimes + cJumpT}"); textosEstadisticasVisuales[16].TextColor = Color.Gray;
                textosEstadisticasVisuales[17].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat12")}: -{p.yunqueManaCost + cManaCost:F2}%"); textosEstadisticasVisuales[17].TextColor = Color.Cyan;
                textosEstadisticasVisuales[18].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat13")}: -{p.yunqueAmmoCost + cAmmoCost:F2}%"); textosEstadisticasVisuales[18].TextColor = Color.LawnGreen;
                textosEstadisticasVisuales[19].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat14")}: +{p.yunqueFish}"); textosEstadisticasVisuales[19].TextColor = Color.Gray;
                textosEstadisticasVisuales[20].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat15")}: +{p.yunqueGrabRange}%"); textosEstadisticasVisuales[20].TextColor = Color.Gray;
                textosEstadisticasVisuales[21].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat21")}: +{p.yunqueWhipRange + cWhip:F2}%"); textosEstadisticasVisuales[21].TextColor = Color.PaleVioletRed;
                textosEstadisticasVisuales[22].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.UI.ReduccionAggro")}: 0"); textosEstadisticasVisuales[22].TextColor = Color.Gray;
                textosEstadisticasVisuales[23].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat24")}: +{p.yunqueManaSickRes + cResMana}%"); textosEstadisticasVisuales[23].TextColor = Color.Gray;
                textosEstadisticasVisuales[24].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat25")}: +{p.yunquePotionSickRes + cResPoc}%"); textosEstadisticasVisuales[24].TextColor = Color.Gray;
                textosEstadisticasVisuales[25].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat16")}: +{p.yunqueMeleeSize + cSize:F2}%"); textosEstadisticasVisuales[25].TextColor = Color.IndianRed;
                textosEstadisticasVisuales[26].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat17")}: -{p.yunqueRespawn}%"); textosEstadisticasVisuales[26].TextColor = Color.Gray;
                textosEstadisticasVisuales[27].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.UI.TiempoVuelo")}: 0"); textosEstadisticasVisuales[27].TextColor = Color.Gray;
                textosEstadisticasVisuales[28].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat18")}: +{p.yunqueMiningSpeed}%"); textosEstadisticasVisuales[28].TextColor = Color.Gray;
                textosEstadisticasVisuales[29].SetText($"{Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat19")}: +{(Main.LocalPlayer.moveSpeed - 1f) * 100f:F1}%"); textosEstadisticasVisuales[29].TextColor = Color.LightGray;

                listaStats.Recalculate(); 
            }
            else if (paginaActual == 2) 
            {
                titleText.SetText("");
                btnReset.Top.Set(-1000, 0f); btnAceptar.Top.Set(-1000, 0f); btnDebugXP.Top.Set(-1000, 0f); btnDebugLevel.Top.Set(-1000, 0f);
                for(int i = 0; i < 10; i++) { statTextsPts[i].Left.Set(-1000, 0f); btnMinusArray[i].Left.Set(-1000, 0f); btnPlusArray[i].Left.Set(-1000, 0f); hoverTexts[i].Left.Set(-1000, 0f); }
                textoLevelCap.Top.Set(410, 0f); textoLevelCap.SetText(Language.GetTextValue("Mods.RPGARGENTO.UI.NivelMundo", p.levelCap));

                if (p.level < 10) {
                    textoBloqueo.SetText(Language.GetTextValue("Mods.RPGARGENTO.UI.SubeNivel10")); textoBloqueo.Top.Set(200, 0f);
                    textoClaseActual.Top.Set(-1000, 0f); tooltipClase.Top.Set(-1000, 0f); tooltipAdv.Top.Set(-1000, 0f);
                    for(int i = 0; i < 4; i++) btnClases[i].Top.Set(-1000, 0f); for(int i = 0; i < 2; i++) btnClaseAvanzada[i].Top.Set(-1000, 0f);
                } else {
                    if (p.claseElegida == -1) {
                        textoBloqueo.Top.Set(-1000, 0f); textoClaseActual.Top.Set(-1000, 0f); tooltipAdv.Top.Set(-1000, 0f); tooltipClase.Top.Set(260, 0f); 
                        for(int i = 0; i < 4; i++) btnClases[i].Top.Set(150, 0f); for(int i = 0; i < 2; i++) btnClaseAvanzada[i].Top.Set(-1000, 0f);
                    } else {
                        string nombreC = p.claseElegida == 0 ? "Melee" : p.claseElegida == 1 ? "Ranger" : p.claseElegida == 2 ? "Magic" : "Summon";
                        for(int i = 0; i < 4; i++) btnClases[i].Top.Set(-1000, 0f); tooltipClase.Top.Set(-1000, 0f); textoClaseActual.Top.Set(60, 0f); 

                        if (ultimaClaseDibujada != p.claseElegida) {
                            string[] nBase = { "Melee", "Ranger", "Magic", "Summon" };
                            for(int i = 0; i < 2; i++) {
                                btnClaseAvanzada[i].RemoveAllChildren();
                                UIImage imgAdv = new UIImage(ModContent.Request<Texture2D>("RPGARGENTO/Assets/Clase" + nBase[p.claseElegida]));
                                imgAdv.Width.Set(80, 0f); imgAdv.Height.Set(80, 0f); imgAdv.Left.Set(-3, 0f); imgAdv.Top.Set(-3, 0f);
                                btnClaseAvanzada[i].Append(imgAdv);
                            }
                            ultimaClaseDibujada = p.claseElegida;
                        }

                        if (p.level < 25) {
                            textoBloqueo.SetText(Language.GetTextValue("Mods.RPGARGENTO.UI.SubeNivel25", nombreC)); textoBloqueo.Top.Set(280, 0f);
                            string keyBono = p.claseElegida == 0 ? "BonoBaseMelee" : p.claseElegida == 1 ? "BonoBaseRanger" : p.claseElegida == 2 ? "BonoBaseMagic" : "BonoBaseSummon";
                            textoClaseActual.SetText(Language.GetTextValue("Mods.RPGARGENTO.UI.ClaseBase", nombreC, Language.GetTextValue($"Mods.RPGARGENTO.UI.{keyBono}"))); 
                            tooltipAdv.Top.Set(-1000, 0f);
                            for(int i = 0; i < 2; i++) btnClaseAvanzada[i].Top.Set(-1000, 0f);
                        } else {
                            textoBloqueo.Top.Set(-1000, 0f);
                            if (p.claseAvanzada == -1) {
                                textoClaseActual.SetText(Language.GetTextValue("Mods.RPGARGENTO.UI.EvolucionaClase", nombreC)); tooltipAdv.Top.Set(320, 0f);
                                for(int i = 0; i < 2; i++) btnClaseAvanzada[i].Top.Set(180, 0f);
                                
                                btnClaseAvanzada[0].OnMouseOver += (evt, element) => { 
                                    string hA = p.claseElegida == 0 ? "BonoAdvBerserker" : p.claseElegida == 1 ? "BonoAdvFrancotirador" : p.claseElegida == 2 ? "BonoAdvBrujo" : "BonoAdvExperto";
                                    string nA = p.claseElegida == 0 ? "Berserker" : p.claseElegida == 1 ? "Francotirador" : p.claseElegida == 2 ? "Brujo" : "Inv. Experto";
                                    tooltipAdv.SetText($"{nA}:\n{Language.GetTextValue($"Mods.RPGARGENTO.UI.{hA}")}");
                                };
                                btnClaseAvanzada[1].OnMouseOver += (evt, element) => { 
                                    string hB = p.claseElegida == 0 ? "BonoAdvPaladin" : p.claseElegida == 1 ? "BonoAdvArtillero" : p.claseElegida == 2 ? "BonoAdvHechicero" : "BonoAdvInexperto";
                                    string nB = p.claseElegida == 0 ? "Paladín" : p.claseElegida == 1 ? "Artillero" : p.claseElegida == 2 ? "Hechicero" : "Inv. Inexperto";
                                    tooltipAdv.SetText($"{nB}:\n{Language.GetTextValue($"Mods.RPGARGENTO.UI.{hB}")}");
                                };
                            } else {
                                for(int i = 0; i < 2; i++) btnClaseAvanzada[i].Top.Set(-1000, 0f); tooltipAdv.Top.Set(-1000, 0f);
                                string nombreA = ""; string keyBonoAdv = "";
                                if(p.claseElegida==0) { nombreA = p.claseAvanzada == 0 ? "Berserker" : "Paladín"; keyBonoAdv = p.claseAvanzada == 0 ? "BonoAdvBerserker" : "BonoAdvPaladin"; }
                                if(p.claseElegida==1) { nombreA = p.claseAvanzada == 0 ? "Francotirador" : "Artillero"; keyBonoAdv = p.claseAvanzada == 0 ? "BonoAdvFrancotirador" : "BonoAdvArtillero"; }
                                if(p.claseElegida==2) { nombreA = p.claseAvanzada == 0 ? "Brujo" : "Hechicero"; keyBonoAdv = p.claseAvanzada == 0 ? "BonoAdvBrujo" : "BonoAdvHechicero"; }
                                if(p.claseElegida==3) { nombreA = p.claseAvanzada == 0 ? "Inv. Experto" : "Inv. Inexperto"; keyBonoAdv = p.claseAvanzada == 0 ? "BonoAdvExperto" : "BonoAdvInexperto"; }
                                textoClaseActual.SetText(Language.GetTextValue("Mods.RPGARGENTO.UI.EresMaestro", nombreA, Language.GetTextValue($"Mods.RPGARGENTO.UI.{keyBonoAdv}")));
                            }
                        }
                    }
                }
            }
        }
        
        private void AddHeaderToList(LocalizedText text, Color color) { UIText header = new UIText(text); header.TextColor = color; header.HAlign = 0.5f; header.SetPadding(8); listaStats.Add(header); }
        private void AddStatElement() { UIText stat = new UIText("", 0.95f); stat.SetPadding(2); listaStats.Add(stat); textosEstadisticasVisuales.Add(stat); }
    }
}