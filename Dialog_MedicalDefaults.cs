using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace StepAway
{
	public class Dialog_MedicalDefaults : Window
	{
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 400f);
			}
		}

        public Dialog_MedicalDefaults()
        {
            this.forcePause = true;
            this.doCloseX = true;
            this.closeOnEscapeKey = true;
            this.doCloseButton = true;
            this.absorbInputAroundWindow = true;
            this.closeOnClickedOutside = true;
        }

		public override void DoWindowContents(Rect inRect)
		{
            Text.Font = GameFont.Medium;
            Rect rect = new Rect(0f, 0f, InitialSize.x, 50f);
            Widgets.Label(rect, "Medical Defaults");
            Text.Font = GameFont.Small;

            Rect content = new Rect(0, 50, inRect.width, inRect.height - 70);

            Listing_Standard listing_Standard = new Listing_Standard(content);
            float stdHeight = MedicalCareUtility.CareSetterHeight;

            float hgap = 40;

            float hposA = 20;
            float hposB = 100;
            float hposC = hposB + MedicalCareUtility.CareSetterWidth + hgap;

            float hwidthA = 60;
            float hwidthB = MedicalCareUtility.CareSetterWidth;
            float hwidthC = MedicalCareUtility.CareSetterWidth;

            try
            {
                MedicalCareUtility_Detour.HorribleHackWithinDefaultDialog = true;

                listing_Standard.Gap(20);

                {
                    Rect title = listing_Standard.GetRect(stdHeight);
                    Widgets.Label(new Rect(hposB + 30, title.y, hwidthB, stdHeight), "Human");
                    Widgets.Label(new Rect(hposC + 30, title.y, hwidthC, stdHeight), "Animal");
                }

                {
                    Rect title = listing_Standard.GetRect(stdHeight);
                    Widgets.Label(new Rect(hposA, title.y, hwidthA, stdHeight), "Colonist");
                    MedicalCareUtility.MedicalCareSetter(new Rect(hposB, title.y, hwidthB, stdHeight), ref Config.Instance.care_PlayerHuman);
                    MedicalCareUtility.MedicalCareSetter(new Rect(hposC, title.y, hwidthC, stdHeight), ref Config.Instance.care_PlayerAnimal);
                }

                {
                    Rect title = listing_Standard.GetRect(stdHeight);
                    Widgets.Label(new Rect(hposA, title.y, hwidthA, stdHeight), "Ally");
                    MedicalCareUtility.MedicalCareSetter(new Rect(hposB, title.y, hwidthB, stdHeight), ref Config.Instance.care_AllyHuman);
                    MedicalCareUtility.MedicalCareSetter(new Rect(hposC, title.y, hwidthC, stdHeight), ref Config.Instance.care_AllyAnimal);
                }

                {
                    Rect title = listing_Standard.GetRect(stdHeight);
                    Widgets.Label(new Rect(hposA, title.y, hwidthA, stdHeight), "Enemy");
                    MedicalCareUtility.MedicalCareSetter(new Rect(hposB, title.y, hwidthB, stdHeight), ref Config.Instance.care_EnemyHuman);
                    MedicalCareUtility.MedicalCareSetter(new Rect(hposC, title.y, hwidthC, stdHeight), ref Config.Instance.care_EnemyAnimal);
                }

                listing_Standard.Gap(50);

                if (listing_Standard.ButtonText("Reset all pawns to these values"))
                {
                    foreach (Pawn p in Find.MapPawns.AllPawns)
                    {
                        Pawn_PlayerSettings_Detour.ResetPPSMedicalCare(p.playerSettings, p);
                    }
                }
            }
            finally
            {
                MedicalCareUtility_Detour.HorribleHackWithinDefaultDialog = false;
            }

            listing_Standard.End();
		}
	}
}
