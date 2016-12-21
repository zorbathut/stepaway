using System;
using System.Reflection;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace StepAway
{
    static class Pawn_Detour
    {
        public static void SetFaction(this Pawn pawn, Faction newFaction, Pawn recruiter = null)
        {
            if (newFaction == pawn.Faction)
            {
                Log.Warning(string.Concat(new object[]
                {
                    "Used ChangePawnFactionTo to change ",
                    pawn,
                    " to same faction ",
                    newFaction
                }));
                return;
            }
            if (pawn.guest != null)
            {
                pawn.guest.SetGuestStatus(null, false);
            }
            if (pawn.Spawned)
            {
                pawn.Map.mapPawns.DeRegisterPawn(pawn);
                pawn.Map.pawnDestinationManager.RemovePawnFromSystem(pawn);
                pawn.Map.designationManager.RemoveAllDesignationsOn(pawn, false);
            }
            if (newFaction == Faction.OfPlayer || pawn.Faction == Faction.OfPlayer)
            {
                Find.ColonistBar.MarkColonistsDirty();
            }
            Lord lord = pawn.GetLord();
            if (lord != null)
            {
                lord.Notify_PawnLost(pawn, PawnLostCondition.ChangedFaction);
            }
            if (pawn.Faction != null && pawn.Faction.leader == pawn)
            {
                pawn.Faction.Notify_LeaderLost();
            }
            if (newFaction == Faction.OfPlayer && pawn.RaceProps.Humanlike)
            {
                pawn.kindDef = newFaction.def.basicMemberKind;
            }

            // Call base SetFaction directly - horrible mojo warning
            MethodInfo method = typeof(Thing).GetMethod("SetFaction", BindingFlags.Public | BindingFlags.Instance);
            IntPtr fptr = method.MethodHandle.GetFunctionPointer();
            ((Action<Faction, Pawn>)Activator.CreateInstance(typeof(Action<Faction, Pawn>), pawn, fptr))(newFaction, null);

            PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, false);
            if (pawn.Faction != null && pawn.Faction.IsPlayer)
            {
                if (pawn.workSettings != null)
                {
                    pawn.workSettings.EnableAndInitialize();
                }
                Find.Storyteller.intenderPopulation.Notify_PopulationGained();
            }
            if (pawn.Drafted)
            {
                pawn.drafter.Drafted = false;
            }
            ReachabilityUtility.ClearCache();
            pawn.health.surgeryBills.Clear();
            if (pawn.Spawned)
            {
                pawn.Map.mapPawns.RegisterPawn(pawn);
            }
            pawn.GenerateNecessaryName();
            if (pawn.playerSettings != null)
            {
                Pawn_PlayerSettings_Detour.ResetPPSMedicalCare(pawn.playerSettings, pawn);
            }
            pawn.ClearMind(true);
            if (!pawn.Dead && pawn.needs.mood != null)
            {
                pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
            }
            if (pawn.Spawned)
            {
                pawn.Map.attackTargetsCache.UpdateTarget(pawn);
            }
            Find.GameEnder.CheckGameOver();
            AddictionUtility.CheckDrugAddictionTeachOpportunity(pawn);
            if (pawn.needs != null)
            {
                pawn.needs.AddOrRemoveNeedsAsAppropriate();
            }
            if (pawn.playerSettings != null)
            {
                pawn.playerSettings.Notify_FactionChanged();
            }
        }
    }
}
