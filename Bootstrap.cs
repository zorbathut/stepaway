﻿using CommunityCoreLibrary_StepAway;
using System;
using System.Reflection;
using UnityEngine;
using Verse;

namespace StepAway
{
    class Bootstrap : Def
    {
        static Bootstrap()
        {
            try
            {
                {
                    MethodInfo method1 = typeof(RimWorld.MedicalCareUtility).GetMethod("MedicalCareSetter", BindingFlags.Static | BindingFlags.Public);
                    MethodInfo method2 = typeof(MedicalCareUtility_Detour).GetMethod("MedicalCareSetter", BindingFlags.Static | BindingFlags.Public);
                    if (!Detours.TryDetourFromTo(method1, method2))
                    {
                        Log.Error("EVERYTHING IS BROKEN 1");
                        return;
                    }
                }

                {
                    ConstructorInfo method1 = typeof(RimWorld.Pawn_PlayerSettings).GetConstructor(new Type[] { typeof(Pawn) });
                    MethodInfo method2 = typeof(Pawn_PlayerSettings_Detour).GetMethod("Pawn_PlayerSettings", BindingFlags.Static | BindingFlags.Public);
                    if (!Detours.TryDetourFromTo(method1, method2))
                    {
                        Log.Error("EVERYTHING IS BROKEN 2");
                        return;
                    }
                }
            }
            catch (Exception)
            {
                Log.Error("something is seriously wrong");
            }
        }
    }
}