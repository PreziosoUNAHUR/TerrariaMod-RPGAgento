using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.Localization;

namespace RPGARGENTO.UI
{
    public class StatsMenu : UIState
    {
        private UIPanel mainPanel;
        private UIText titleText;
        private UIText worldLevelText;
        private int paginaActual = 0;

        private UIList statsList;
        private UIScrollbar scrollbar;

        public override void OnInitialize()
        {
            mainPanel = new UIPanel();
            mainPanel.Width.Set(600, 0f);
            mainPanel.Height.Set(480, 0f);
            mainPanel.HAlign = 0.5f;
            mainPanel.VAlign = 0.5f;
            mainPanel.BackgroundColor = new Color(12, 24, 48, 240);
            mainPanel.BorderColor = new Color(50, 80, 130, 180);
            mainPanel.SetPadding(15);
            Append(mainPanel);

            var btnPrev = new UIPanel();
            btnPrev.Width.Set(28, 0f);
            btnPrev.Height.Set(28, 0f);
            btnPrev.Left.Set(5, 0f);
            btnPrev.Top.Set(5, 0f);
            btnPrev.BackgroundColor = new Color(30, 50, 80, 180);
            btnPrev.BorderColor = new Color(50, 80, 130, 100);
            btnPrev.SetPadding(0);
            btnPrev.Append(new UIText("<") { HAlign = 0.5f, VAlign = 0.5f });
            btnPrev.OnLeftClick += (evt, el) => { paginaActual = paginaActual == 0 ? 1 : 0; RefrescarUI(); };
            mainPanel.Append(btnPrev);

            titleText = new UIText("", 1.05f);
            titleText.HAlign = 0.5f;
            titleText.Top.Set(8, 0f);
            titleText.TextColor = Color.Gold;
            mainPanel.Append(titleText);

            var btnNext = new UIPanel();
            btnNext.Width.Set(28, 0f);
            btnNext.Height.Set(28, 0f);
            btnNext.Left.Set(-33, 1f);
            btnNext.Top.Set(5, 0f);
            btnNext.BackgroundColor = new Color(30, 50, 80, 180);
            btnNext.BorderColor = new Color(50, 80, 130, 100);
            btnNext.SetPadding(0);
            btnNext.Append(new UIText(">") { HAlign = 0.5f, VAlign = 0.5f });
            btnNext.OnLeftClick += (evt, el) => { paginaActual = paginaActual == 0 ? 1 : 0; RefrescarUI(); };
            mainPanel.Append(btnNext);

            var scrollPanel = new UIPanel();
            scrollPanel.Width.Set(560, 0f);
            scrollPanel.Height.Set(380, 0f);
            scrollPanel.Top.Set(42, 0f);
            scrollPanel.HAlign = 0.5f;
            scrollPanel.BackgroundColor = new Color(6, 14, 30, 200);
            scrollPanel.BorderColor = Color.Transparent;
            mainPanel.Append(scrollPanel);

            statsList = new UIList();
            statsList.Width.Set(530, 0f);
            statsList.Height.Set(0, 1f);
            statsList.Left.Set(5, 0f);
            statsList.SetPadding(0);
            statsList.ManualSortMethod = (list) => { };
            scrollPanel.Append(statsList);

            scrollbar = new UIScrollbar();
            scrollbar.SetView(100f, 1000f);
            scrollbar.Height.Set(0, 1f);
            scrollbar.HAlign = 1f;
            scrollbar.Left.Set(-5, 0f);
            scrollPanel.Append(scrollbar);
            statsList.SetScrollbar(scrollbar);

            worldLevelText = new UIText("", 0.8f);
            worldLevelText.Left.Set(10, 0f);
            worldLevelText.Top.Set(435, 0f);
            worldLevelText.TextColor = new Color(255, 200, 50);
            mainPanel.Append(worldLevelText);
        }

        public override void Update(GameTime gameTime) { base.Update(gameTime); }
        public override void OnActivate() { base.OnActivate(); RefrescarUI(); }

        public void RefrescarUI()
        {
            if (Main.gameMenu || Main.LocalPlayer == null || !Main.LocalPlayer.active) return;

            var p = Main.LocalPlayer.GetModPlayer<RPGPlayer>();
            worldLevelText.SetText(Language.GetTextValue("Mods.RPGARGENTO.UI.MundoNivel", RPGGlobalNPC.GetWorldLevel()));

            if (paginaActual == 0)
            {
                titleText.SetText(Language.GetTextValue("Mods.RPGARGENTO.UI.GeneralTitle"));
                BuildGeneralPage(p);
            }
            else
            {
                titleText.SetText(Language.GetTextValue("Mods.RPGARGENTO.UI.AnvilTitle"));
                BuildAnvilPage(p);
            }

            statsList.Recalculate();
        }

        private void ClearList()
        {
            statsList.Clear();
        }

        private void AddHeader(string text, Color color)
        {
            var label = new UIText(text, 0.9f);
            label.TextColor = color;
            label.SetPadding(6);
            statsList.Add(label);
        }

        private void AddStat(string label, string value, Color valueColor)
        {
            var row = new UIPanel();
            row.Width.Set(530, 0f);
            row.Height.Set(22, 0f);
            row.BackgroundColor = new Color(10, 18, 35, 30);
            row.BorderColor = Color.Transparent;
            row.SetPadding(0);

            var lbl = new UIText($"  {label}:", 0.8f);
            lbl.VAlign = 0.5f;
            lbl.TextColor = new Color(170, 180, 210);
            row.Append(lbl);

            var val = new UIText(value, 0.85f);
            val.Left.Set(370, 0f);
            val.VAlign = 0.5f;
            val.TextColor = valueColor;
            row.Append(val);

            statsList.Add(row);
        }

        private void AddSpacer(int height = 4)
        {
            var s = new UIText("");
            s.SetPadding(height / 2);
            statsList.Add(s);
        }

        private void BuildGeneralPage(RPGPlayer p)
        {
            ClearList();

            AddHeader(Language.GetTextValue("Mods.RPGARGENTO.UI.CatAtaque"), new Color(220, 100, 100));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat1"), $"+{p.yunqueMelDmg * 0.01f * 100f:F1}%", new Color(220, 160, 160));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat2"), $"+{p.yunqueRanDmg * 0.01f * 100f:F1}%", new Color(160, 220, 160));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat0"), $"+{p.yunqueMagDmg * 0.01f * 100f:F1}%", new Color(160, 200, 255));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat3"), $"+{p.yunqueSumDmg * 0.01f * 100f:F1}%", new Color(200, 160, 220));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat5"), $"+{p.yunqueCrit:F1}", new Color(255, 200, 80));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat4"), $"+{p.yunqueKnockback * 0.01f * 100f:F1}%", new Color(220, 180, 120));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat12"), $"-{p.yunqueManaCost * 0.01f * 100f:F1}%", new Color(120, 200, 255));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat13"), $"-{p.yunqueAmmoCost * 0.01f * 100f:F1}%", new Color(160, 220, 160));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat21"), $"+{p.yunqueWhipRange * 0.01f * 100f:F1}%", new Color(220, 170, 220));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat16"), $"+{p.yunqueMeleeSize * 0.01f * 100f:F1}%", new Color(220, 170, 130));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat28"), $"+{p.yunqueMelSpeed * 0.01f * 100f:F1}%", new Color(240, 180, 140));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat29"), $"+{p.yunqueRanSpeed * 0.01f * 100f:F1}%", new Color(180, 240, 150));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat30"), $"+{p.yunqueMagSpeed * 0.01f * 100f:F1}%", new Color(150, 200, 255));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat31"), $"+{p.yunqueArmorPen * 0.01f * 100f:F1}%", new Color(200, 180, 240));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat32"), $"+{p.yunqueGenericDmg * 0.01f * 100f:F1}%", new Color(255, 210, 140));

            AddSpacer();
            AddHeader(Language.GetTextValue("Mods.RPGARGENTO.UI.CatDefensa"), new Color(100, 180, 230));

            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat8"), $"+{p.yunqueLife}", new Color(120, 220, 120));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat7"), $"+{p.yunqueMana}", new Color(120, 190, 255));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat6"), $"+{p.yunqueDef:F1}", new Color(190, 200, 210));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat22"), $"+{p.yunqueRegenVida:F2}%", new Color(100, 210, 100));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat23"), $"+{p.yunqueRegenMana:F2}%", new Color(100, 190, 230));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.UI.ReduccionDano"), $"+{p.yunqueDamageReduction}%", new Color(120, 210, 210));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat33"), $"+{p.yunqueThorns * 0.01f * 100f:F1}%", new Color(180, 200, 120));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat34"), $"+{p.yunqueFlatLifeRegen:F1}", new Color(100, 230, 130));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat35"), $"+{p.yunqueFlatManaRegen:F1}", new Color(110, 200, 240));

            AddSpacer();
            AddHeader(Language.GetTextValue("Mods.RPGARGENTO.UI.CatExtras"), new Color(200, 200, 120));

            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat19"), $"+{p.yunqueMoveSpeed * 0.01f * 100f:F1}%", new Color(200, 220, 120));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat37"), $"+{p.yunqueMaxRunSpeed * 0.01f * 100f:F1}%", new Color(200, 210, 100));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat9"), $"+{p.yunqueJumpHeight:F1}", new Color(180, 210, 180));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat20"), $"+{p.yunqueJumpTimes}", new Color(180, 220, 200));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat10"), $"{p.yunqueAggro}", new Color(200, 170, 170));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat15"), $"{p.yunqueGrabRange}%", new Color(160, 190, 200));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat18"), $"{p.yunqueMiningSpeed}%", new Color(190, 180, 150));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat14"), $"{p.yunqueFish}", new Color(150, 200, 220));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat17"), $"-{p.yunqueRespawn}%", new Color(170, 160, 160));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat24"), $"+{p.yunqueManaSickRes}%", new Color(150, 180, 220));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat25"), $"+{p.yunquePotionSickRes}%", new Color(150, 210, 150));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat36"), $"+{p.yunqueFlightTime:F1}s", new Color(200, 200, 240));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat38"), $"+{p.yunqueBlockRange:F1}", new Color(170, 200, 210));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat39"), $"+{p.yunqueTileSpeed * 0.01f * 100f:F1}%", new Color(190, 190, 160));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat40"), $"+{p.yunqueWallSpeed * 0.01f * 100f:F1}%", new Color(180, 180, 150));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat41"), $"+{p.yunqueLuck * 0.01f * 100f:F2}%", new Color(200, 180, 100));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat26"), $"+{p.yunqueMaxMinion}", new Color(210, 160, 230));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat27"), $"+{p.yunqueMaxSentinel}", new Color(210, 160, 230));
        }

        private void BuildAnvilPage(RPGPlayer p)
        {
            ClearList();

            AddHeader(Language.GetTextValue("Mods.RPGARGENTO.UI.CatAtaque"), new Color(220, 100, 100));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat1"), $"+{p.yunqueMelDmg:F2}", new Color(220, 160, 160));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat2"), $"+{p.yunqueRanDmg:F2}", new Color(160, 220, 160));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat0"), $"+{p.yunqueMagDmg:F2}", new Color(160, 200, 255));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat3"), $"+{p.yunqueSumDmg:F2}", new Color(200, 160, 220));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat5"), $"+{p.yunqueCrit:F2}", new Color(255, 200, 80));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat4"), $"+{p.yunqueKnockback:F2}", new Color(220, 180, 120));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat12"), $"-{p.yunqueManaCost:F2}", new Color(120, 200, 255));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat13"), $"-{p.yunqueAmmoCost:F2}", new Color(160, 220, 160));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat21"), $"+{p.yunqueWhipRange:F2}", new Color(220, 170, 220));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat16"), $"+{p.yunqueMeleeSize:F2}", new Color(220, 170, 130));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat28"), $"+{p.yunqueMelSpeed:F2}", new Color(240, 180, 140));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat29"), $"+{p.yunqueRanSpeed:F2}", new Color(180, 240, 150));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat30"), $"+{p.yunqueMagSpeed:F2}", new Color(150, 200, 255));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat31"), $"+{p.yunqueArmorPen:F2}", new Color(200, 180, 240));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat32"), $"+{p.yunqueGenericDmg:F2}", new Color(255, 210, 140));

            AddSpacer();
            AddHeader(Language.GetTextValue("Mods.RPGARGENTO.UI.CatDefensa"), new Color(100, 180, 230));

            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat8"), $"+{p.yunqueLife}", new Color(120, 220, 120));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat7"), $"+{p.yunqueMana}", new Color(120, 190, 255));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat6"), $"+{p.yunqueDef:F1}", new Color(190, 200, 210));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat22"), $"+{p.yunqueRegenVida:F2}", new Color(100, 210, 100));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat23"), $"+{p.yunqueRegenMana:F2}", new Color(100, 190, 230));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.UI.ReduccionDano"), $"+{p.yunqueDamageReduction}%", new Color(120, 210, 210));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat33"), $"+{p.yunqueThorns:F2}", new Color(180, 200, 120));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat34"), $"+{p.yunqueFlatLifeRegen:F2}", new Color(100, 230, 130));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat35"), $"+{p.yunqueFlatManaRegen:F2}", new Color(110, 200, 240));

            AddSpacer();
            AddHeader(Language.GetTextValue("Mods.RPGARGENTO.UI.CatExtras"), new Color(200, 200, 120));

            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat19"), $"+{p.yunqueMoveSpeed:F1}", new Color(200, 220, 120));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat37"), $"+{p.yunqueMaxRunSpeed:F2}", new Color(200, 210, 100));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat9"), $"+{p.yunqueJumpHeight:F1}", new Color(180, 210, 180));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat20"), $"+{p.yunqueJumpTimes}", new Color(180, 220, 200));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat10"), $"{p.yunqueAggro}", new Color(200, 170, 170));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat15"), $"{p.yunqueGrabRange}%", new Color(160, 190, 200));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat18"), $"{p.yunqueMiningSpeed}%", new Color(190, 180, 150));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat14"), $"{p.yunqueFish}", new Color(150, 200, 220));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat17"), $"-{p.yunqueRespawn}%", new Color(170, 160, 160));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat24"), $"+{p.yunqueManaSickRes}%", new Color(150, 180, 220));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat25"), $"+{p.yunquePotionSickRes}%", new Color(150, 210, 150));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat36"), $"+{p.yunqueFlightTime:F1}s", new Color(200, 200, 240));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat38"), $"+{p.yunqueBlockRange:F1}", new Color(170, 200, 210));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat39"), $"+{p.yunqueTileSpeed:F2}", new Color(190, 190, 160));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat40"), $"+{p.yunqueWallSpeed:F2}", new Color(180, 180, 150));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat41"), $"+{p.yunqueLuck:F2}", new Color(200, 180, 100));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat26"), $"+{p.yunqueMaxMinion}", new Color(210, 160, 230));
            AddStat(Language.GetTextValue("Mods.RPGARGENTO.Yunque.Stat27"), $"+{p.yunqueMaxSentinel}", new Color(210, 160, 230));
        }
    }
}
