using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//
namespace yz_Petrifaction
{
    //
    [BepInPlugin("yuzutuki.yzPetrifaction", "Petrifaction", "1.0.0.0")]
    public class yzPetrifaction : BaseUnityPlugin
    {
        /*
        public void OnStartCore()
        {
        }
        */
        private void Awake()
        {
            var harmony = new Harmony("yz_Petrifaction");
            harmony.PatchAll();
        }
    }
}
