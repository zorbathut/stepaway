using HugsLib.Utils;
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
            if (Current.ProgramState == ProgramState.Playing)
            {
                pps.joinTick = Find.TickManager.TicksGame;
            }
            else
            {
                pps.joinTick = 0;
            }

            ResetPPSMedicalCare(pps, pawn);
        }

        public static void Notify_FactionChanged(Pawn_PlayerSettings pps)
        {
            ResetPPSMedicalCare(pps, pps.GetFieldViaReflection<Pawn>("pawn"));
        }

        public static void Notify_MadePrisoner(Pawn_PlayerSettings pps)
        {
            Notify_FactionChanged(pps);
        }

        public static void ResetPPSMedicalCare(Pawn_PlayerSettings pps, Pawn pawn)
        {
            if (pps == null || pawn == null)
            {
                return;
            }

            Config config = UtilityWorldObjectManager.GetUtilityWorldObject<Config>();

            // needed for world creation, loading
            if (config == null || Find.FactionManager == null)
            {
                pps.medCare = MedicalCareCategory.HerbalOrWorse;
                if (pawn.IsColonist && !pawn.IsPrisoner && !pawn.RaceProps.Animal)
                {
                    pps.medCare = MedicalCareCategory.Best;
                }
                return;
            }

            if (pawn.RaceProps.Humanlike)
            {
                if (pawn.Faction != null && pawn.Faction.IsPlayer)
                {
                    if (pawn.IsPrisoner)
                    {
                        pps.medCare = config.care_PrisonerHuman;
                    }
                    else
                    {
                        pps.medCare = config.care_PlayerHuman;
                    }
                }
                else if (pawn.Faction == null || !pawn.Faction.HostileTo(Faction.OfPlayer))
                {
                    pps.medCare = config.care_AllyHuman;
                }
                else
                {
                    pps.medCare = config.care_EnemyHuman;
                }
            }
            else
            {
                if (pawn.Faction != null && pawn.Faction.IsPlayer)
                {
                    pps.medCare = config.care_PlayerAnimal;
                }
                else if (pawn.Faction == null || !pawn.Faction.HostileTo(Faction.OfPlayer))
                {
                    pps.medCare = config.care_AllyAnimal;
                }
                else
                {
                    pps.medCare = config.care_EnemyAnimal;
                }
            }
        }
    }
}
