using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;

namespace Altv_Roleplay.Handler
{
    class HUDHandler : IScript
    {

        public static void CreateHUDBrowser(ClassicPlayer client)
        {
            if (client == null || !client.Exists) return;
            client.Emit("Client:HUD:CreateCEF", Characters.GetCharacterHunger(User.GetPlayerOnline(client)), Characters.GetCharacterThirst(User.GetPlayerOnline(client)));
        }

        [ScriptEvent(ScriptEventType.PlayerEnterVehicle)]
        public static void OnPlayerEnterVehicle_Handler(ClassicVehicle vehicle, ClassicPlayer client, byte seat)
        {
            try
            {
                if (client is null) return;
                bool LockState = ServerVehicles.GetVehicleLockState(vehicle);
                client.Emit("ChangeVehicleLockState", LockState);
                client.Emit("Player:SetSeatbeltState", client.Seatbelt);
                client.Emit("Client:HUD:updateHUDPosInVeh", true, vehicle.Fuel, vehicle.KM);
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }

        [ScriptEvent(ScriptEventType.PlayerLeaveVehicle)]
        public static void OnPlayerLeaveVehicle_Handler(ClassicVehicle vehicle, ClassicPlayer client, byte seat)
        {
            try
            {
                if (client == null || !client.Exists) return;
                client.Seatbelt = false;
                client.Emit("Client:HUD:updateHUDPosInVeh", false, 0, 0);
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }

        public static void SendInformationToVehicleHUD(ClassicPlayer player)
        {
            if (player == null || !player.Exists) return;
            ClassicVehicle Veh = (ClassicVehicle)player.Vehicle;
            if (!Veh.Exists) return;
            ulong vehID = Veh.GetVehicleId();
            if (vehID == 0) return;
            player.EmitLocked("Client:HUD:SetPlayerHUDVehicleInfos", Veh.Fuel, Veh.KM);
        }

        public static void SendNotification(IPlayer client, int type, int duration, string msg, int delay = 0) //1 Info | 2 Success | 3 Warning | 4 Error
        {
            try
            {
                if (client == null || !client.Exists) return;
                client.EmitLocked("Client:HUD:sendNotification", type, duration, msg, delay);
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }

        [ClientEvent("Server:Vehicle:UpdateVehicleKM")]
        public static void UpdateVehicleKM(ClassicPlayer player, float km)
        {
            //KM = bei 600 Meter = 600
            //600 / 1000 = 0,6   = 0,6km ?
            try
            {
                //Core.Debug.OutputDebugString(player.Username + " | " + km);
                if (player is null || km <= 0) return;
                if (!player.IsInVehicle || player.Vehicle == null) return;
                float fKM = km / 1000;
                ClassicVehicle veh = (ClassicVehicle)player.Vehicle;
                veh.KM += fKM;
                float FuelCalculation = km / 1000f;
                if ((veh.Fuel -= FuelCalculation) <= 0) { veh.EngineOn = false; veh.Fuel = 0; return; }
                veh.Fuel -= FuelCalculation;
                //Core.Debug.OutputDebugString("Fuel from " + player.Username + " | fKM : " + fKM + " | " + FuelCalculation.ToString());
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }
    }
}
