﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.Modules;
using UnityEngine;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;
using SD = ItemChanger.Util.SceneDataUtil;
using Module = ItemChanger.Modules.Module;
using static RandomizerMod.LogHelper;

namespace RandomizerMod.IC
{
    public class RandomizerModule : Module
    {
        public override void Initialize()
        {
            AbstractItem.ModifyRedundantItemGlobal += ModifyRedundantItem;
            ToggleSceneHooks(true);
        }

        public override void Unload()
        {
            AbstractItem.ModifyRedundantItemGlobal -= ModifyRedundantItem;
            ToggleSceneHooks(false);
        }

        private static void ModifyRedundantItem(GiveEventArgs args)
        {
            args.Item = new ItemChanger.Items.AddGeoItem
            {
                amount = 100,
                name = "100_Geo-" + args.Orig.name,
                UIDef = args.Orig.UIDef?.Clone(),
            };
        }

        private static void ToggleSceneHooks(bool toggle)
        {
            Settings.GenerationSettings gs = RandomizerMod.RS.GenerationSettings;

            switch (gs.StartLocationSettings.StartLocation)
            {
                case "Ancestral Mound":
                    if (gs.NoveltySettings.RandomizeNail)
                    {
                        if (toggle) Events.AddSceneChangeEdit(SceneNames.Crossroads_ShamanTemple, DestroyPlanksForAncestralMoundStart);
                        else Events.RemoveSceneChangeEdit(SceneNames.Crossroads_ShamanTemple, DestroyPlanksForAncestralMoundStart);
                    }
                    break;

                case "Fungal Core":
                    if (toggle) Events.AddSceneChangeEdit(SceneNames.Fungus2_30, CreateBounceShroomsForFungalCoreStart);
                    else Events.RemoveSceneChangeEdit(SceneNames.Fungus2_30, CreateBounceShroomsForFungalCoreStart);
                    break;
            }


        }

        // Destroy planks in cursed nail mode because we can't slash them
        private static void DestroyPlanksForAncestralMoundStart(Scene to)
        {
            foreach ((_, GameObject go) in to.Traverse())
            {
                if (go.name.StartsWith("Plank")) UObject.Destroy(go);
            }
        }

        private static void CreateBounceShroomsForFungalCoreStart(Scene to)
        {
            GameObject bounceShroom = to.FindGameObjectByName("Bounce Shroom C");

            GameObject s0 = UObject.Instantiate(bounceShroom);
            s0.transform.SetPosition3D(12.5f, 26f, 0f);
            s0.SetActive(true);

            GameObject s1 = UObject.Instantiate(bounceShroom);
            s1.transform.SetPosition3D(12.5f, 54f, 0f);
            s1.SetActive(true);

            GameObject s2 = UObject.Instantiate(bounceShroom);
            s2.transform.SetPosition3D(21.7f, 133f, 0f);
            s2.SetActive(true);
        }
    }
}
