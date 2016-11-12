using System;
using System.Reflection;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;

namespace StepAway
{
    static class MedicalCareUtility_Detour
    {
        public static void MedicalCareSetter(Rect rect, ref MedicalCareCategory medCare)
        {
            Rect rect2 = new Rect(rect.x, rect.y, rect.width / 5f, rect.height);
            for (int i = 0; i < 5; i++)
            {
                MedicalCareCategory mc = (MedicalCareCategory)i;
                Widgets.DrawHighlightIfMouseover(rect2);
                GUI.DrawTexture(rect2, typeof(MedicalCareUtility).GetStaticFieldViaReflection<Texture2D[]>("careTextures")[i]);
                if (Widgets.ButtonInvisible(rect2, false))
                {
                    medCare = mc;
                    SoundDefOf.TickHigh.PlayOneShotOnCamera();
                }
                if (medCare == mc)
                {
                    Widgets.DrawBox(rect2, 3);
                }
                TooltipHandler.TipRegion(rect2, () => mc.GetLabel(), 632165 + i * 17);
                rect2.x += rect2.width;
            }

            if (!HorribleHackWithinDefaultDialog)
            {
                rect2.x += 15;
                rect2.width = 60;
                if (Widgets.ButtonText(rect2, "Defaults"))
                {
                    Find.WindowStack.Add(new Dialog_MedicalDefaults());
                }
            }
        }

        public static bool HorribleHackWithinDefaultDialog = false;
    }
}
