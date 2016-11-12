using System;
using System.Reflection;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;

namespace StepAway
{
    static class Pawn_PlayerSettings_Detour
    {
        public static void Pawn_PlayerSettings(this Pawn_PlayerSettings pps, Pawn pawn)
        {
            pps.SetFieldViaReflection("pawn", pawn);
            if (Current.ProgramState == ProgramState.MapPlaying)
            {
                pps.joinTick = Find.TickManager.TicksGame;
            }
            else
            {
                pps.joinTick = 0;
            }

            ResetPPSMedicalCare(pps, pawn);
        }

        public static void ResetPPSMedicalCare(Pawn_PlayerSettings pps, Pawn pawn)
        {
            if (pps == null || pawn == null)
            {
                return;
            }

            // needed for world creation
            if (Config.Instance == null)
            {
                if (pawn.RaceProps.Humanlike && !pawn.IsPrisoner)
                {
                    pps.medCare = MedicalCareCategory.Best;
                }
                else
                {
                    pps.medCare = MedicalCareCategory.NoMeds;
                }
                return;
            }

            if (pawn.RaceProps.Humanlike)
            {
                if (pawn.Faction.IsPlayer)
                {
                    pps.medCare = Config.Instance.care_PlayerHuman;
                }
                else if (!pawn.Faction.HostileTo(Faction.OfPlayer))
                {
                    pps.medCare = Config.Instance.care_AllyHuman;
                }
                else
                {
                    pps.medCare = Config.Instance.care_EnemyHuman;
                }
            }
            else
            {
                if (pawn.Faction.IsPlayer)
                {
                    pps.medCare = Config.Instance.care_PlayerAnimal;
                }
                else if (!pawn.Faction.HostileTo(Faction.OfPlayer))
                {
                    pps.medCare = Config.Instance.care_AllyAnimal;
                }
                else
                {
                    pps.medCare = Config.Instance.care_EnemyAnimal;
                }
            }
        }
    }
}
