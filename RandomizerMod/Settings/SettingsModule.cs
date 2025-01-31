﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomizerMod.Settings
{
    public abstract class SettingsModule : ICloneable
    {
        /// <summary>
        /// Randomize the fields of the module.
        /// </summary>
        public virtual void Randomize(Random rng)
        {
            foreach (FieldInfo f in Util.GetOrderedFields(GetType()))
            {
                Type T = f.FieldType;

                if (T == typeof(bool))
                {
                    f.SetValue(this, rng.Next(2) == 0);
                }
                else if (T == typeof(int) || T.IsEnum && Enum.GetUnderlyingType(T) == typeof(int))
                {
                    int maxValue = int.MaxValue - 1;
                    int minValue = int.MinValue;

                    if (T.GetCustomAttribute<MaxValueAttribute>() is MaxValueAttribute max) maxValue = max.Value;
                    else if (T.IsEnum)
                    {
                        maxValue = Enum.GetValues(T).Cast<int>().Max();
                    }

                    if (T.GetCustomAttribute<MinValueAttribute>() is MinValueAttribute min) minValue = min.Value;
                    else if (T.IsEnum)
                    {
                        minValue = Enum.GetValues(T).Cast<int>().Min();
                    }

                    f.SetValue(this, rng.Next(minValue, maxValue + 1));
                }
            }
        }

        /// <summary>
        /// Fix all compatibility or range issues with current settings.
        /// </summary>
        public virtual void Clamp(GenerationSettings gs)
        {

        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
