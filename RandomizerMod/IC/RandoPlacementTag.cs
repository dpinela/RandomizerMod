﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemChanger;

namespace RandomizerMod.IC
{
    public class RandoPlacementTag : Tag
    {
        public static event Action<VisitStateChangedEventArgs> OnRandoPlacementVisitStateChanged;

        public override void Load(object parent)
        {
            ((AbstractPlacement)parent).OnVisitStateChanged += OnVisitStateChanged;
        }

        public override void Unload(object parent)
        {
            ((AbstractPlacement)parent).OnVisitStateChanged -= OnVisitStateChanged;
        }

        private void OnVisitStateChanged(VisitStateChangedEventArgs args)
        {
            OnRandoPlacementVisitStateChanged?.Invoke(args);
        }
    }
}
