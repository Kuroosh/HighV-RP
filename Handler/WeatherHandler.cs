using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using System;

namespace Altv_Roleplay.Handler
{
    class WeatherHandler
    {
        public static void SetRealTime(ClassicPlayer player)
        {
            if (player == null || !player.Exists) return;
            player.SetDateTime(DateTime.Now);
        }
    }
}
