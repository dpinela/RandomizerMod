﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomizerMod.Settings
{
    [Serializable]
    public class MiscSettings : SettingsModule
    {
        public bool AddDuplicateItems;

        public enum MaskShardType
        {
            FourShardsPerMask,
            TwoShardsPerMask,
            OneShardPerMask
        }
        public MaskShardType MaskShards;

        public enum VesselFragmentType
        {
            ThreeFragmentsPerVessel,
            TwoFragmentsPerVessel,
            OneFragmentPerVessel
        }
        public VesselFragmentType VesselFragments;

        public bool RandomizeNotchCosts;
    }
}
