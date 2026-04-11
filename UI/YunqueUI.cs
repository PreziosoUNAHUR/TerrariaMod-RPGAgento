using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.Localization;
using RPGARGENTO.Content.Items;

namespace RPGARGENTO.UI
{
    public class YunqueUI : UIState
    {
        private UIPanel[] rectangulos = new UIPanel[3];
        private UIImage[] bordes = new UIImage[3];
        private UIText[] textosStat = new UIText[3];
        private UIText[] textosValor = new UIText[3]; 
        private int rareza; private int[] opciones = new int[3];

        public override void OnInitialize()
        {
            for (int i = 0; i < 3; i++)
            {
                int index = i;
                rectangulos[i] = new UIPanel(); rectangulos[i].Width.Set(130, 0f); rectangulos[i].Height.Set(280, 0f); 
                rectangulos[i].HAlign = 0.5f; rectangulos[i].VAlign = 0.5f; rectangulos[i].Left.Set((i - 1) * 180, 0f); rectangulos[i].BorderColor = Color.Transparent;
                rectangulos[i].OnLeftClick += (evt, element) => Seleccionar(index);
                
                bordes[i] = new UIImage(Terraria.GameContent.TextureAssets.MagicPixel);
                bordes[i].Width.Set(130, 0f); bordes[i].Height.Set(280, 0f); bordes[i].Left.Set(-12, 0f); bordes[i].Top.Set(-12, 0f); bordes[i].IgnoresMouseInteraction = true;
                rectangulos[i].Append(bordes[i]);

                textosStat[i] = new UIText(""); textosStat[i].HAlign = 0.5f; textosStat[i].Top.Set(100, 0f); rectangulos[i].Append(textosStat[i]);
                textosValor[i] = new UIText(""); textosValor[i].HAlign = 0.5f; textosValor[i].Top.Set(130, 0f); rectangulos[i].Append(textosValor[i]);
                Append(rectangulos[i]);
            }
        }

        public void Sorteo(int r, int s1, int s2, int s3)
        {
            rareza = r; opciones[0] = s1; opciones[1] = s2; opciones[2] = s3;
            string nombreTextura = r == 1 ? "BordePlata" : r == 2 ? "BordeOro" : "BordePrismatico";
            Color colorRelleno = r == 1 ? new Color(160, 160, 160, 200) : r == 2 ? new Color(218, 165, 32, 200) : new Color(10, 30, 80, 200); 
            var textura = ModContent.Request<Texture2D>("RPGARGENTO/Assets/" + nombreTextura);

            for (int i = 0; i < 3; i++) 
            {
                rectangulos[i].BackgroundColor = colorRelleno; bordes[i].SetImage(textura);
                int id = opciones[i]; 
                
                // Carga el nombre según el idioma configurado en Terraria
                textosStat[i].SetText(Language.GetTextValue($"Mods.RPGARGENTO.Yunque.Stat{id}"));
                
                string t = "";
                if (r == 1) {
                    if (id <= 5 || id == 12 || id == 13) t = "+0.01%"; else if (id == 6) t = "+0.25"; else if (id == 15 || id == 16 || id == 18) t = "+1%"; else if (id == 17) t = "-1%"; else if (id == 11) t = "-1"; else t = "+1";
                } else if (r == 2) {
                    if (id <= 5 || id == 12 || id == 13 || id == 21 || id == 16) t = "+0.1%"; else if (id == 6) t = "+0.50"; else if (id == 7 || id == 8) t = "+3"; else if (id == 19) t = "+1%"; else t = "+1";
                } else if (r == 3) {
                    if (id <= 5) t = "+1%"; else if (id == 6) t = "+1"; else if (id == 7 || id == 8) t = "+5"; else if (id == 22 || id == 23) t = "+0.1%"; else if (id == 24 || id == 25) t = "+10%"; else t = "+1";
                }
                textosValor[i].SetText(t);
            }
        }

        private void Seleccionar(int i)
        {
            var p = Main.LocalPlayer.GetModPlayer<RPGPlayer>(); int id = opciones[i];
            if (rareza == 1) {
                if(id==0)p.yunqueMagDmg+=0.01f; else if(id==1)p.yunqueMelDmg+=0.01f; else if(id==2)p.yunqueRanDmg+=0.01f; else if(id==3)p.yunqueSumDmg+=0.01f; else if(id==4)p.yunqueKnockback+=0.01f; else if(id==5)p.yunqueCrit+=0.01f; else if(id==6)p.yunqueDef+=0.25f; else if(id==7)p.yunqueMana+=1; else if(id==8)p.yunqueLife+=1; else if(id==9)p.yunqueJumpHeight+=1f; else if(id==10)p.yunqueAggro+=1; else if(id==11)p.yunqueAggro-=1; else if(id==12)p.yunqueManaCost+=0.01f; else if(id==13)p.yunqueAmmoCost+=0.01f; else if(id==14)p.yunqueFish+=1; else if(id==15)p.yunqueGrabRange+=1f; else if(id==16)p.yunqueMeleeSize+=1f; else if(id==17)p.yunqueRespawn+=1f; else if(id==18)p.yunqueMiningSpeed+=1f;
            } else if (rareza == 2) {
                if(id==0)p.yunqueMagDmg+=0.1f; else if(id==1)p.yunqueMelDmg+=0.1f; else if(id==2)p.yunqueRanDmg+=0.1f; else if(id==3)p.yunqueSumDmg+=0.1f; else if(id==4)p.yunqueKnockback+=0.1f; else if(id==5)p.yunqueCrit+=0.1f; else if(id==6)p.yunqueDef+=0.5f; else if(id==7)p.yunqueMana+=3; else if(id==8)p.yunqueLife+=3; else if(id==19)p.yunqueMoveSpeed+=1f; else if(id==12)p.yunqueManaCost+=0.1f; else if(id==13)p.yunqueAmmoCost+=0.1f; else if(id==20)p.yunqueJumpTimes+=1; else if(id==21)p.yunqueWhipRange+=0.1f; else if(id==16)p.yunqueMeleeSize+=0.1f;
            } else if (rareza == 3) {
                if(id==0)p.yunqueMagDmg+=1f; else if(id==1)p.yunqueMelDmg+=1f; else if(id==2)p.yunqueRanDmg+=1f; else if(id==3)p.yunqueSumDmg+=1f; else if(id==4)p.yunqueKnockback+=1f; else if(id==5)p.yunqueCrit+=1f; else if(id==6)p.yunqueDef+=1f; else if(id==7)p.yunqueMana+=5; else if(id==8)p.yunqueLife+=5; else if(id==22)p.yunqueRegenVida+=0.1f; else if(id==23)p.yunqueRegenMana+=0.1f; else if(id==24)p.yunqueManaSickRes+=10f; else if(id==25)p.yunquePotionSickRes+=10f; else if(id==26)p.yunqueMaxMinion+=1; else if(id==27)p.yunqueMaxSentinel+=1;
            }
            Main.LocalPlayer.ConsumeItem(ModContent.ItemType<YunqueCaos>());
            ModContent.GetInstance<UISystem>().CerrarYunque();
        }
    }
}