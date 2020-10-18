using AltV.Net;
using Altv_Roleplay.Factories;
using System;

namespace Altv_Roleplay.Vehicles
{
    public class Vehicles : IScript
    {
        [ClientEvent("Vehicle:UpdateStats")]
        public static void OnVehicleUpdate(ClassicPlayer player)
        {
            try
            {
                if (player is null) return;
                if (player.IsInVehicle)
                {
                    ClassicVehicle vehClass = (ClassicVehicle)player.Vehicle;
                    if (vehClass is null) return;


                }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
    }
}
