﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RandomizerMod.Settings
{
    [Serializable]
    public class GenerationSettings : ICloneable
    {
        public int Seed;
        public TransitionSettings TransitionSettings = new TransitionSettings();
        public SkipSettings SkipSettings = new SkipSettings();
        public PoolSettings PoolSettings = new PoolSettings();
        public NoveltySettings NoveltySettings = new();
        public CostSettings CostSettings = new();
        public CursedSettings CursedSettings = new CursedSettings();
        public LongLocationSettings LongLocationSettings = new LongLocationSettings();
        public StartLocationSettings StartLocationSettings = new StartLocationSettings();
        public StartItemSettings StartItemSettings = new StartItemSettings();
        public MiscSettings MiscSettings = new MiscSettings();

        public GenerationSettings()
        {
        }

        private SettingsModule[] modules => moduleFields.Select(f => f.GetValue(this) as SettingsModule).ToArray();
        private static readonly FieldInfo[] moduleFields = typeof(GenerationSettings).GetFields().Where(f => f.FieldType.IsSubclassOf(typeof(SettingsModule)))
            .OrderBy(f => f.Name).ToArray();

        public string Serialize()
        {
            return string.Join(BinaryFormatting.CLASS_SEPARATOR.ToString(), modules.Select(o => BinaryFormatting.Serialize(o)).ToArray());
        }

        public static GenerationSettings Deserialize(string code)
        {
            GenerationSettings gs = new GenerationSettings();
            string[] pieces = code.Split(BinaryFormatting.CLASS_SEPARATOR);
            object[] fields = gs.modules;

            if (pieces.Length != fields.Length)
            {
                LogHelper.LogWarn("Invalid settings code: not enough pieces.");
                return null;
            }

            for (int i = 0; i < fields.Length; i++)
            {
                BinaryFormatting.Deserialize(pieces[i], fields[i]);
            }

            return gs;
        }

        public void Randomize(Random rng)
        {
            Seed = rng.Next(1000000000);
            foreach (SettingsModule m in modules) m.Randomize(rng);
            Clamp();
        }

        public void Clamp()
        {
            foreach (SettingsModule m in modules) m.Clamp(this);
        }

        public object Clone()
        {
            GenerationSettings gs = MemberwiseClone() as GenerationSettings;
            foreach (FieldInfo f in moduleFields) f.SetValue(gs, (f.GetValue(this) as SettingsModule).Clone());
            return gs;
        }

        // fields for hard-coding the path of a logic setting
        private const bool True = true;
        private const bool False = false;
    }
}
