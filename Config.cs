using HugsLib.Utils;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace StepAway
{
    public class Config : UtilityWorldObject
    {
        public MedicalCareCategory care_PlayerHuman = MedicalCareCategory.Best;
        public MedicalCareCategory care_PlayerAnimal = MedicalCareCategory.NoMeds;
        public MedicalCareCategory care_PrisonerHuman = MedicalCareCategory.HerbalOrWorse;
        public MedicalCareCategory care_AllyHuman = MedicalCareCategory.HerbalOrWorse;
        public MedicalCareCategory care_AllyAnimal = MedicalCareCategory.NoMeds;
        public MedicalCareCategory care_EnemyHuman = MedicalCareCategory.NoMeds;
        public MedicalCareCategory care_EnemyAnimal = MedicalCareCategory.NoMeds;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.LookValue(ref care_PlayerHuman, "care_PlayerHuman", MedicalCareCategory.Best);
            Scribe_Values.LookValue(ref care_PlayerAnimal, "care_PlayerAnimal", MedicalCareCategory.NoMeds);
            Scribe_Values.LookValue(ref care_PrisonerHuman, "care_PrisonerHuman", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.LookValue(ref care_AllyHuman, "care_AllyHuman", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.LookValue(ref care_AllyAnimal, "care_AllyAnimal", MedicalCareCategory.NoMeds);
            Scribe_Values.LookValue(ref care_EnemyHuman, "care_EnemyHuman", MedicalCareCategory.NoMeds);
            Scribe_Values.LookValue(ref care_EnemyAnimal, "care_EnemyAnimal", MedicalCareCategory.NoMeds);
        }
    }
}
