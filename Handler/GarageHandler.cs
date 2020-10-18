using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.models;
using Altv_Roleplay.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Altv_Roleplay.Handler
{
    class GarageHandler : IScript
    {
        internal static void OpenGarageCEF(ClassicPlayer player, int garageId)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                if (player == null || !player.Exists || garageId == 0) return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                var garageInfo = ServerGarages.ServerGarages_.FirstOrDefault(x => x.id == garageId);
                var garageSlots = ServerGarages.ServerGarageSlots_.Where(x => x.garageId == garageId).ToList();
                if (garageInfo == null || !garageSlots.Any()) return;
                if (!player.Position.IsInRange(new Position(garageInfo.posX, garageInfo.posY, garageInfo.posZ), 2f)) return;
                var charFaction = ServerFactions.GetCharacterFactionId(charId);
                var factionCut = ServerFactions.GetFactionShortName(charFaction);
                bool charFactionDuty = ServerFactions.IsCharacterInFactionDuty(charId);
                var inString = "";
                var outString = "";
                var garageName = "";
                //0 Auto | 1 Boot | 2 Flugzeug | 3 Heli
                if (garageInfo.type == 0) { garageName = $"Fahrzeuggarage: {garageInfo.name}"; }
                else if (garageInfo.type == 1) { garageName = $"Bootsgarage: {garageInfo.name}"; }
                else if (garageInfo.type == 2) { garageName = $"Flugzeuggarage: {garageInfo.name}"; }
                else if (garageInfo.type == 3) { garageName = $"Heligarage: {garageInfo.name}"; }

                if (garageInfo.name.Contains("Fraktion"))
                {
                    if (charFaction <= 0) { HUDHandler.SendNotification(player, 4, 5000, $"Keine Berechtigung."); return; }
                    var gFactionCut = garageInfo.name.Split(" ")[1]; //Fraktion LSPD Mission Row  <- Beispiel
                    if (gFactionCut != factionCut) { HUDHandler.SendNotification(player, 4, 5000, $"Keine Berechtigung [002]."); return; }
                    /*if(!charFactionDuty) { HUDHandler.SendNotification(player, 4, 5000, $"Keine Berechtigung (nicht im Dienst)."); return; }*/
                    inString = GetGarageParkInString(player, garageSlots, charId, garageId, true, factionCut, charFaction);
                    outString = GetGarageParkOutString(player, garageId, charId, true, factionCut);
                    player.EmitLocked("Client:Garage:OpenGarage", garageId, garageName, inString, outString);
                    stopwatch.Stop();
                    if (stopwatch.Elapsed.Milliseconds > 30) Alt.Log($"{charId} - OpenGarageCEF benötigte {stopwatch.Elapsed.Milliseconds}ms");
                    return;
                }

                inString = GetGarageParkInString(player, garageSlots, charId, garageId, false, "Zivilist", charFaction); //Array von Fahrzeugen die um die Garage rum zum Einparken stehen
                outString = GetGarageParkOutString(player, garageId, charId, false, "Zivilist");
                Global.mGlobal.VirtualAPI.TriggerClientEventSafe(player, "Client:Garage:OpenGarage", garageId, garageName, inString, outString);


                stopwatch.Stop();
                if (stopwatch.Elapsed.Milliseconds > 30) Alt.Log($"{charId} - OpenGarageCEF benötigte {stopwatch.Elapsed.Milliseconds}ms");
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }

        public static string GetGarageParkInString(ClassicPlayer player, IReadOnlyCollection<Server_Garage_Slots> garageSlots, int charId, int garageId, bool isFaction, string factionShort, int factionId)
        {
            if (player == null || !player.Exists || !garageSlots.Any() || garageId == 0 || charId == 0) return "undefined";
            List<IVehicle> vehicles = null;
            if (isFaction == false) { vehicles = Alt.Server.GetVehicles().Where(x => x != null && x.Exists && x.HasVehicleId() && x.GetVehicleId() > 0 && x.Position.IsInRange(player.Position, 50f) && x.Dimension == 0).ToList(); }
            else if (isFaction == true) { vehicles = Alt.Server.GetVehicles().Where(x => x != null && x.Exists && x.HasVehicleId() && x.GetVehicleId() > 0 && x.Position.IsInRange(player.Position, 50f) && ServerVehicles.GetVehicleFactionId(x) == factionId && x.NumberplateText.Contains(factionShort) && x.Dimension == 0).ToList(); }
            int garageType = ServerGarages.GetGarageType(garageId);
            if (garageType == -1) return "undefined";
            dynamic array = new JArray() as dynamic;
            dynamic entry = new JObject();
            foreach (var veh in vehicles)
            {
                bool hasKey = false,
                    isOwner = ServerVehicles.GetVehicleOwner(veh) == charId;
                if (isFaction) hasKey = CharactersInventory.ExistCharacterItem(charId, $"Fahrzeugschluessel {factionShort}", "inventory");
                else if (!isFaction) hasKey = CharactersInventory.ExistCharacterItem(charId, $"Fahrzeugschluessel {veh.NumberplateText}", "inventory");
                if (!isOwner && !hasKey) continue;
                entry = new JObject();
                entry.vehid = veh.GetVehicleId();
                entry.plate = veh.NumberplateText;
                entry.hash = veh.Model;
                entry.name = ServerVehicles.GetVehicleNameOnHash(veh.Model);
                array.Add(entry);
            }

            //foreach (var slot in garageSlots.Where(x => x.garageId == garageId))
            //{
            //    var pos = new Position(slot.posX, slot.posY, slot.posZ);
            //    var entity = vehicles.OrderBy(x => x.Position.Distance(pos)).ToList().FirstOrDefault();
            //    if(entity == null) { continue; }
            //    var distance = entity.Position.Distance(pos);
            //    if (distance >= 1.5f) { continue; }
            //    var vehicle = ServerVehicles.ServerVehicles_.Where(x => x.isInGarage == false).ToList().FirstOrDefault(x => string.Equals(x.plate, entity.NumberplateText, StringComparison.CurrentCultureIgnoreCase));
            //    if(vehicle == null) { continue; }
            //    var vehicleData = ServerAllVehicles.ServerAllVehicles_.FirstOrDefault(x => x.hash == entity.Model);
            //    if (vehicleData == null) { continue; }
            //    if(vehicleData.vehClass != garageType) { continue; }
            //    bool hasKey = false;
            //    if (isFaction == false) { hasKey = CharactersInventory.ExistCharacterItem(charId, "Fahrzeugschluessel " + entity.NumberplateText, "inventory"); }
            //    else if(isFaction == true) { hasKey = CharactersInventory.ExistCharacterItem(charId, "Fahrzeugschluessel " + factionShort, "inventory"); }
            //    bool isOwner = vehicle.charid == charId;
            //    if(!hasKey && !isOwner) { continue; }

            //    entry = new JObject();
            //    entry.vehid = vehicle.id;
            //    entry.plate = entity.NumberplateText;
            //    entry.hash = entity.Model;
            //    entry.name = vehicleData.name;
            //    array.Add(entry);
            //}
            return array.ToString();
        }

        public static string GetGarageParkOutString(ClassicPlayer player, int garageId, int charId, bool isFaction, string factionShort)
        {
            try
            {
                if (player == null || !player.Exists || garageId == 0 || charId == 0) return "undefined";
                List<ClassicVehicle> inGarageVehs = null;
                if (isFaction == false) { inGarageVehs = ServerVehicles.ServerVehicles_.Where(x => x.isInGarage && x.garageId == garageId).ToList(); }
                else if (isFaction == true) { inGarageVehs = ServerVehicles.ServerVehicles_.Where(x => x.isInGarage && x.garageId == garageId && x.plate.Contains(factionShort)).ToList(); }
                dynamic array = new JArray() as dynamic;
                dynamic entry = new JObject();

                foreach (var vehicle in inGarageVehs)
                {
                    bool hasKey = false;
                    if (isFaction == false) { hasKey = CharactersInventory.ExistCharacterItem(charId, "Fahrzeugschluessel " + vehicle.plate, "inventory"); }
                    else if (isFaction == true) { hasKey = CharactersInventory.ExistCharacterItem(charId, "Fahrzeugschluessel " + factionShort, "inventory"); }
                    bool isOwner = vehicle.charid == charId;
                    if (!hasKey && !isOwner) continue;

                    entry = new JObject();
                    entry.vehid = vehicle.id;
                    entry.plate = vehicle.plate;
                    entry.hash = vehicle.hash;
                    entry.name = ServerAllVehicles.GetVehicleNameOnHash(vehicle.hash);
                    array.Add(entry);
                }

                return array.ToString();
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
            return "[]";
        }

        [ClientEvent("Server:Garage:DoAction")]
        public static void DoGarageAction(ClassicPlayer player, int garageid, string action, int vehID)
        {
            try
            {
                if (player == null || !player.Exists || action == "" || vehID <= 0 || garageid <= 0) return;
                int charId = User.GetPlayerOnline(player);
                if (charId <= 0) return;
                ClassicVehicle vehicle = (ClassicVehicle)Alt.Server.GetVehicles().ToList().FirstOrDefault(x => x.GetVehicleId() == (ulong)vehID);
                if (action == "storage")
                {
                    //Fahrzeug einparken
                    if (vehicle == null) return;
                    if (!vehicle.Position.IsInRange(player.Position, 50f) && vehicle.Dimension == 0) return;
                    ServerVehicles.SetVehicleInGarage(vehicle, true, garageid);
                    HUDHandler.SendNotification(player, 2, 5000, $"Fahrzeug erfolgreich eingeparkt.");
                }
                else if (action == "take")
                {
                    //Fahrzeug ausparken
                    Position outPos = new Position(0, 0, 0);
                    int curPid = 1;
                    bool Created = false;
                    foreach (var x in ServerGarages.ServerGarageSlots_.Where(x => x.garageId == garageid))
                    {
                        foreach (ClassicVehicle veh in Alt.Server.GetVehicles().ToList())
                        {
                            if (!veh.Position.IsInRange(ServerGarages.GetGarageSlotPosition(garageid, curPid), 2f) && veh.Dimension == 0)
                            {
                                curPid++;
                                Created = true;
                                vehicle.Position = ServerGarages.GetGarageSlotPosition(garageid, curPid);
                                vehicle.Rotation = ServerGarages.GetGarageSlotRotation(garageid, curPid);
                                vehicle.Dimension = 0;
                                vehicle.isInGarage = false;
                                vehicle.LockState = VehicleLockState.Locked;
                                vehicle.EngineOn = false;
                                vehicle.SetVehicleTrunkState(false);
                                ServerVehicles.SetVehicleModsCorrectly(vehicle);
                                ServerVehicles.SetVehicleInGarage(vehicle, false, garageid);
                                return;
                            }
                        }
                    }
                    if (!Created) { HUDHandler.SendNotification(player, 4, 5000, $"Es ist kein Parkplatz mehr frei."); return; }
                }

                if (!CharactersTablet.HasCharacterTutorialEntryFinished(charId, "useGarage"))
                {
                    CharactersTablet.SetCharacterTutorialEntryState(charId, "useGarage", true);
                    HUDHandler.SendNotification(player, 1, 2500, "Erfolg freigeschaltet: Keine Schäden");
                }
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }
    }
}
