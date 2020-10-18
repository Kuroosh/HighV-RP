﻿using AltV.Net.Elements.Entities;
using System;

namespace Altv_Roleplay.Factories
{
    public class ClassicColshape : Checkpoint
    {
        public int ColshapeId { get; set; } = 0;
        public string ColshapeName { get; set; } = "None";
        public string CarDealerVehName { get; set; }
        public ulong CarDealerVehPrice { get; set; }
        public float Radiuss { get; set; }


        public ClassicColshape(IntPtr nativePointer) : base(nativePointer)
        {

        }

        public bool IsInRange(ClassicPlayer player)
        {
            lock (player)
            {
                if (!player.Exists) return false;

                return player.Position.Distance(Position) <= Radiuss;
            }
        }
    }
}
