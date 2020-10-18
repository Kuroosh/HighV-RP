using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Handler;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;
using System.Linq;

namespace Altv_Roleplay.Admin
{
    public class Admin : IScript
    {
        public const int ADMINLVL_SUPPORTER = 5;
        public const int ADMINLVL_MODERATOR = 6;
        public const int ADMINLVL_OWNER = 10;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Owner Commands 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [CommandEvent(CommandEventType.CommandNotFound)]
        public static void OnPlayerCommandNotFoundHandler(ClassicPlayer player, string Command)
        {
            try
            {
                HUDHandler.SendNotification(player, 4, 5000, "/" + Command + " wurde nicht gefunden...");
            }
            catch { }
        }
        [Command("pos")]
        public static void GetPlayerPosition(ClassicPlayer player)
        {
            player?.SendChatMessage("POS : X : " + player.Position.X + " | Y : " + player.Position.Y + " | Z : " + player.Position.Z + " |");
            player?.SendChatMessage("ROT X : " + player.Rotation.Roll + " | Y : " + player.Rotation.Pitch + " | Z : " + player.Rotation.Yaw);
            Core.Debug.OutputDebugString("POS X : " + player.Position.X + " | Y : " + player.Position.Y + " | Z : " + player.Position.Z);
            Core.Debug.OutputDebugString("ROT X : " + player.Rotation.Roll + " | Y : " + player.Rotation.Pitch + " | Z : " + player.Rotation.Yaw);
        }

        [Command("setadminlvl")]
        public static void SetAdminLevel(ClassicPlayer player, int playerId, int AdminID)
        {
            try
            {
                if (player.AdminLevel() < ADMINLVL_OWNER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_OWNER); return; }
                if (playerId <= 0) return;
                HUDHandler.SendNotification(player, 4, 5000, "Admin Level von Player ID [" + playerId + "] geupdated : " + AdminID);
                User.SetPlayerAdminLevel(playerId, AdminID);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }

        [Command("createatm")]
        public static void CreateATM(ClassicPlayer player, string name)
        {
            try
            {
                if (player.AdminLevel() < ADMINLVL_OWNER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_OWNER); return; }
                ServerATM.CreateNewATM(player, player.Position, name);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }

        [Command("dv")]
        public static void DeleteVehicleCMD(ClassicPlayer player)
        {
            try
            {
                if (player.AdminLevel() < ADMINLVL_OWNER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_OWNER); return; }
                if (!player.IsInVehicle) return;
                if (player.Vehicle != null)
                {
                    ClassicVehicle veh = (ClassicVehicle)player.Vehicle;
                    ServerVehicles.RemoveVehiclePermanently(veh);
                    player.Vehicle.Remove();
                }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }

        [Command("ooc", greedyArg: true)]
        public static void CreateOOCMessage(ClassicPlayer player, string OOCMessage)
        {
            try
            {
                foreach (ClassicPlayer players in Alt.GetAllPlayers().ToList())
                {
                    if (player.Position.Distance(players.Position) <= 15)
                        HUDHandler.SendNotification(players, 4, 5000, "[OOC] " + player.CharacterName + " : " + OOCMessage);
                }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }

        [Command("faction")]
        public void FactionCMD(ClassicPlayer player, int charId, int id)
        {
            try
            {
                if (player == null || !player.Exists || player.GetCharacterMetaId() <= 0) return;
                if (player.AdminLevel() < ADMINLVL_OWNER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_OWNER); return; }
                if (ServerFactions.IsCharacterInAnyFaction(charId))
                    ServerFactions.RemoveServerFactionMember(ServerFactions.GetCharacterFactionId(charId), charId);

                ServerFactions.CreateServerFactionMember(id, charId, ServerFactions.GetFactionMaxRankCount(id), charId);
                HUDHandler.SendNotification(player, 1, 5000, $"Du hast den Spieler mit der Id {charId} zur Fraktion mit der Id: {id} gesetzt");
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }

        [Command("giveitem")]
        public void GiveItemCMD(ClassicPlayer player, string itemName, int itemAmount)
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() < ADMINLVL_OWNER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_OWNER); return; }
            if (!ServerItems.ExistItem(ServerItems.ReturnNormalItemName(itemName))) { HUDHandler.SendNotification(player, 4, 5000, $"Itemname nicht gefunden: {itemName}"); return; }
            ulong charId = player.GetCharacterMetaId();
            if (charId <= 0) return;
            CharactersInventory.AddCharacterItem((int)charId, itemName, itemAmount, "inventory");
            HUDHandler.SendNotification(player, 2, 5000, $"Gegenstand '{itemName}' ({itemAmount}x) erhalten.");
        }

        [Command("parkallvehicles")]
        public static void ParkAllVehiclesCommand(ClassicPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                if (player.AdminLevel() < ADMINLVL_OWNER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_OWNER); return; }
                int count = 0;
                foreach (ClassicVehicle veh in Alt.Server.GetVehicles().Where(x => x != null && x.Exists && x.HasVehicleId()))
                {
                    if (veh == null || !veh.Exists || !veh.HasVehicleId()) continue;
                    int currentGarageId = ServerVehicles.GetVehicleGarageId(veh);
                    if (currentGarageId <= 0) continue;
                    ServerVehicles.SetVehicleInGarage(veh, true, currentGarageId);
                    count++;
                }
                HUDHandler.SendNotification(player, 4, 5000, $"{count} Fahrzeuge eingeparkt.");
            }
            catch (Exception e) { Core.Debug.CatchExceptions(e); }
        }

        [Command("parkvehiclekz", true)]
        public static void CMD_parkVehicle(ClassicPlayer player, string plate)
        {
            try
            {
                if (player == null || !player.Exists || string.IsNullOrWhiteSpace(plate)) return;
                if (player.AdminLevel() < ADMINLVL_OWNER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_OWNER); return; }
                ClassicVehicle vehicle = (ClassicVehicle)Alt.Server.GetVehicles().ToList().FirstOrDefault(x => x != null && x.Exists && x.HasVehicleId() && (int)x.GetVehicleId() > 0 && x.NumberplateText.ToLower() == plate.ToLower());
                if (vehicle == null) return;
                ServerVehicles.SetVehicleInGarage(vehicle, true, 25);
                HUDHandler.SendNotification(player, 4, 5000, $"Fahrzeug mit dem Kennzeichen {plate} in Garage 1 (Pillbox) eingeparkt");
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }


        [Command("getaccountidbymail")]
        public static void GetAccountIdByMailCommand(ClassicPlayer player, string mail)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || player.AdminLevel() <= 0) return;
                var accEntry = User.Player.ToList().FirstOrDefault(x => x.Email == mail);
                if (accEntry == null) return;
                if (player.AdminLevel() < ADMINLVL_OWNER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_OWNER); return; }
                player.SendChatMessage($"Spieler-ID der E-Mail {mail} lautet: {accEntry.playerid} - Spielername: {accEntry.playerName}");
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }
        [Command("spawnveh")]
        public void SpawnVehCommand(ClassicPlayer player, string model)
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() < ADMINLVL_OWNER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_OWNER); return; }
            if (player.Vehicle != null && player.Vehicle.Exists) player.Vehicle.Remove();
            ClassicVehicle veh = (ClassicVehicle)Alt.CreateVehicle(model, player.Position, player.Rotation);
            if (veh != null)
            {
                veh.IsAdmin = true;
                player.Emit("setPedIntoVehicle", veh, -1);
                veh.EngineOn = true;
                veh.Fuel = 100;
                veh.LockState = VehicleLockState.Unlocked;
            }
        }

        [Command("resethwid")]
        public void CMD_ResetHwId(ClassicPlayer player, int accountId)
        {
            try
            {
                if (player == null || !player.Exists) return;
                if (player.AdminLevel() < ADMINLVL_OWNER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_OWNER); return; }
                if (!User.ExistPlayerById(accountId)) { HUDHandler.SendNotification(player, 3, 2500, "Der Spieler existiert nicht."); return; }
                User.ResetPlayerHardwareID(accountId);
                HUDHandler.SendNotification(player, 1, 2500, $"Hardware-ID zurückgesetzt (Acc-ID: {accountId}).");
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Moderator Commands 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        
        [Command("announce", true)]
        public void AnnounceCommand(ClassicPlayer player, string msg)
        {
            try
            {
                if (player == null || !player.Exists) return;
                if (player.AdminLevel() < ADMINLVL_MODERATOR) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_MODERATOR); return; }

                foreach (ClassicPlayer client in Alt.Server.GetPlayers().ToList())
                {
                    if (client == null || !client.Exists) continue;
                    HUDHandler.SendNotification(client, 4, 5000, msg);
                }
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }


        [Command("ban")]
        public static void BanCommand(ClassicPlayer player, int accId)
        {
            try
            {
                if (player == null || !player.Exists || accId <= 0) return;
                if (player.AdminLevel() < ADMINLVL_MODERATOR) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_MODERATOR); return; }
                User.SetPlayerBanned(accId, true, $"Gebannt von {Characters.GetCharacterName(User.GetPlayerOnline(player))}");
                ClassicPlayer targetP = (ClassicPlayer)Alt.Server.GetPlayers().ToList().FirstOrDefault(x => x != null && x.Exists && User.GetPlayerAccountId((ClassicPlayer)x) == accId);
                if (targetP != null) targetP.Kick("");
                HUDHandler.SendNotification(player, 4, 5000, $"Spieler mit ID {accId} Erfolgreich gebannt.");
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }
        [Command("unban")]
        public static void UnbanCommand(ClassicPlayer player, int accId)
        {
            try
            {
                if (player == null || !player.Exists || player.CharacterId <= 0 || accId <= 0 || player.AdminLevel() <= 0) return;
                if (player.AdminLevel() < ADMINLVL_MODERATOR) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_MODERATOR); return; }
                User.SetPlayerBanned(accId, false, "");
                HUDHandler.SendNotification(player, 4, 5000, $"Spieler mit ID {accId} Erfolgreich entbannt.");
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Supporter Commands 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Command("players")]
        public void PlayerCommand(ClassicPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                if (player.AdminLevel() <= 0) { HUDHandler.SendNotification(player, 4, 5000, "Keine Rechte."); return; }
                string msg = "Liste aller Spieler:<br>";
                foreach (var p in Alt.Server.GetPlayers().ToList().Where(x => x != null && x.Exists && x.GetCharacterMetaId() > 0))
                {
                    msg += $"{Characters.GetCharacterName((int)p.GetCharacterMetaId())} ({p.GetCharacterMetaId()})<br>";
                }
                HUDHandler.SendNotification(player, 1, 8000, msg);
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }

        [Command("aduty")]
        public void SdutyCMD(ClassicPlayer player)
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() < ADMINLVL_SUPPORTER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_SUPPORTER); return; }
            if (player.GetSyncedMetaData("isAduty", out bool isADuty) && isADuty)
            {
                player.SetSyncedMetaData("isAduty", false);
                player.Emit("Client:Admin:Invincible", false);
                Characters.SetCharacterCorrectClothes(player);
                HUDHandler.SendNotification(player, 4, 5000, $"Du befindest dich nun im nicht mehr im Sduty");
            }
            else
            {
                switch (player.AdminLevel())
                {
                    case ADMINLVL_OWNER:
                        if (!Characters.GetCharacterGender((int)player.GetCharacterMetaId()))
                        {
                            //Männlich
                            player.EmitLocked("Client:SpawnArea:setCharClothes", 1, 135, 2);
                            player.EmitLocked("Client:SpawnArea:setCharClothes", 4, 114, 2);
                            player.EmitLocked("Client:SpawnArea:setCharClothes", 6, 78, 2);
                            player.EmitLocked("Client:SpawnArea:setCharClothes", 3, 3, 0);
                            player.EmitLocked("Client:SpawnArea:setCharClothes", 11, 287, 2);
                            player.EmitLocked("Client:SpawnArea:setCharClothes", 8, 1, 99);
                        }
                        else
                        {
                            //Weiblich
                            player.EmitLocked("Client:SpawnArea:setCharClothes", 1, 135, 2);
                            player.EmitLocked("Client:SpawnArea:setCharClothes", 11, 300, 2);
                            player.EmitLocked("Client:SpawnArea:setCharClothes", 4, 121, 2);
                            player.EmitLocked("Client:SpawnArea:setCharClothes", 3, 8, 0);
                            player.EmitLocked("Client:SpawnArea:setCharClothes", 8, 1, 99);
                            player.EmitLocked("Client:SpawnArea:setCharClothes", 6, 82, 2);
                        }
                        break;
                    case ADMINLVL_MODERATOR:
                        if (!Characters.GetCharacterGender((int)player.GetCharacterMetaId()))
                        {
                            //Männlich
                            player.Emit("Client:SpawnArea:setCharClothes", 1, 135, 1);
                            player.Emit("Client:SpawnArea:setCharClothes", 4, 114, 1);
                            player.Emit("Client:SpawnArea:setCharClothes", 6, 78, 1);
                            player.Emit("Client:SpawnArea:setCharClothes", 3, 3, 0);
                            player.Emit("Client:SpawnArea:setCharClothes", 11, 287, 1);
                            player.Emit("Client:SpawnArea:setCharClothes", 8, 1, 99);
                        }
                        else
                        {
                            //Weiblich
                            player.Emit("Client:SpawnArea:setCharClothes", 1, 135, 1);
                            player.Emit("Client:SpawnArea:setCharClothes", 11, 300, 1);
                            player.Emit("Client:SpawnArea:setCharClothes", 4, 121, 1);
                            player.Emit("Client:SpawnArea:setCharClothes", 3, 8, 0);
                            player.Emit("Client:SpawnArea:setCharClothes", 8, 1, 99);
                            player.Emit("Client:SpawnArea:setCharClothes", 6, 82, 1);
                        }
                        break;
                    case ADMINLVL_SUPPORTER:
                        if (!Characters.GetCharacterGender((int)player.GetCharacterMetaId()))
                        {
                            //Männlich
                            player.Emit("Client:SpawnArea:setCharClothes", 1, 135, 5);
                            player.Emit("Client:SpawnArea:setCharClothes", 4, 114, 5);
                            player.Emit("Client:SpawnArea:setCharClothes", 6, 78, 5);
                            player.Emit("Client:SpawnArea:setCharClothes", 3, 3, 0);
                            player.Emit("Client:SpawnArea:setCharClothes", 11, 287, 5);
                            player.Emit("Client:SpawnArea:setCharClothes", 8, 1, 99);
                        }
                        else
                        {
                            //Weiblich
                            player.Emit("Client:SpawnArea:setCharClothes", 1, 135, 5);
                            player.Emit("Client:SpawnArea:setCharClothes", 11, 300, 5);
                            player.Emit("Client:SpawnArea:setCharClothes", 4, 121, 5);
                            player.Emit("Client:SpawnArea:setCharClothes", 3, 8, 0);
                            player.Emit("Client:SpawnArea:setCharClothes", 8, 1, 99);
                            player.Emit("Client:SpawnArea:setCharClothes", 6, 82, 5);
                        }
                        break;
                }
                player.SetSyncedMetaData("isAduty", true);
                player.Emit("Client:Admin:Invincible", true);
                HUDHandler.SendNotification(player, 4, 5000, $"Du befindest dich nun im Aduty");
            }
        }

        [Command("teamchat", true)]
        public void TeamchatCommand(ClassicPlayer player, string msg)
        {
            try
            {
                if (player.AdminLevel() <= 0)
                {
                    HUDHandler.SendNotification(player, 4, 5000, "Keine Rechte.");
                    return;
                }

                if (player == null || !player.Exists || User.GetPlayerOnline(player) <= 0) return;
                foreach (ClassicPlayer admin in Alt.Server.GetPlayers().ToList().Where(x => x != null && x.Exists && x.HasSyncedMetaData("isAduty") && x.AdminLevel() > 0))
                {
                    admin.SendChatMessage($"[TEAMCHAT] {player.Name}: {msg}");
                }
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }
        [Command("kick", greedyArg: true)]
        public static void KickCommand(ClassicPlayer player, int charId, string Reason)
        {
            try
            {
                if (player == null || !player.Exists || charId <= 0 || player.AdminLevel() <= 0) return;
                ClassicPlayer targetP = (ClassicPlayer)Alt.Server.GetPlayers().ToList().FirstOrDefault(x => x != null && x.Exists && User.GetPlayerOnline((ClassicPlayer)x) == charId);
                if (targetP == null) return;
                if (player.AdminLevel() < ADMINLVL_SUPPORTER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_SUPPORTER); return; }
                targetP.Kick(Reason);
                HUDHandler.SendNotification(player, 4, 5000, $"Spieler mit Char-ID {charId} Erfolgreich gekickt.");
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }

        [Command("whitelist")]
        public void WhitelistCMD(ClassicPlayer player, int targetAccId)
        {
            try
            {
                if (player == null || !player.Exists || targetAccId <= 0 || player.GetCharacterMetaId() <= 0) return;
                if (player.AdminLevel() < ADMINLVL_SUPPORTER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_SUPPORTER); return; }
                if (!User.ExistPlayerById(targetAccId)) { HUDHandler.SendNotification(player, 4, 5000, $"Diese ID existiert nicht {targetAccId}"); return; }
                if (User.IsPlayerWhitelisted(targetAccId)) { HUDHandler.SendNotification(player, 4, 5000, "Der Spieler ist bereits gewhitelisted."); return; }
                User.SetPlayerWhitelistState(targetAccId, true);
                HUDHandler.SendNotification(player, 1, 5000, $"Du hast den Spieler {targetAccId} gewhitelistet.");
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }


        [Command("goto")]
        public void GotoCMD(ClassicPlayer player, int targetId)
        {
            try
            {
                if (player.AdminLevel() < ADMINLVL_SUPPORTER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_SUPPORTER); return; }
                if (player == null || !player.Exists) return;
                if (targetId <= 0 || targetId.ToString().Length <= 0)
                {
                    player.SendChatMessage("Benutzung: /goto charId");
                    return;
                }
                string targetCharName = Characters.GetCharacterName(targetId);
                if (targetCharName.Length <= 0)
                {
                    HUDHandler.SendNotification(player, 3, 5000, $"Warnung: Die angegebene Character-ID wurde nicht gefunden ({targetId}).");
                    return;
                }
                if (!Characters.ExistCharacterName(targetCharName))
                {
                    HUDHandler.SendNotification(player, 3, 5000, $"Warnung: Der angegebene Charaktername wurde nicht gefunden ({targetCharName} - ID: {targetId}).");
                    return;
                }
                var targetPlayer = (ClassicPlayer)Alt.Server.GetPlayers().FirstOrDefault(x => x != null && x.Exists && x.GetCharacterMetaId() == (ulong)targetId);
                if (targetPlayer == null || !targetPlayer.Exists) { HUDHandler.SendNotification(player, 4, 5000, "Fehler: Spieler ist nicht online."); return; }
                HUDHandler.SendNotification(targetPlayer, 1, 5000, $"{Characters.GetCharacterName((int)player.GetCharacterMetaId())} hat sich zu dir teleportiert.");
                HUDHandler.SendNotification(player, 2, 5000, $"Du hast dich zu dem Spieler {Characters.GetCharacterName((int)targetPlayer.GetCharacterMetaId())} teleportiert.");
                player.Position = targetPlayer.Position + new Position(0, 0, 1);
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }

        [Command("revive")]
        public void ReviveTargetCMD(ClassicPlayer player, int targetId)
        {
            if (player == null || !player.Exists) return;
            if (player.AdminLevel() < ADMINLVL_SUPPORTER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_SUPPORTER); return; }
            string charName = Characters.GetCharacterName(targetId);
            if (!Characters.ExistCharacterName(charName)) return;
            ClassicPlayer tp = (ClassicPlayer)Alt.Server.GetPlayers().FirstOrDefault(x => x != null && x.Exists && x.GetCharacterMetaId() == (ulong)targetId);
            if (tp != null)
            {
                DeathHandler.revive((ClassicPlayer)tp);
                HUDHandler.SendNotification(player, 1, 5000, $"Du hast den Spieler {charName} wiederbelebt.");
                return;
            }
            HUDHandler.SendNotification(player, 1, 5000, $"Der Spieler {charName} ist nicht online.");
        }

        [Command("gethere")]
        public void GetHereCMD(ClassicPlayer player, int targetId)
        {
            try
            {
                if (player.AdminLevel() < ADMINLVL_SUPPORTER) { HUDHandler.SendNotification(player, 4, 5000, "Du benötigst mind. Admin-LVL " + ADMINLVL_SUPPORTER); return; }
                if (player == null || !player.Exists) return;
                if (targetId <= 0 || targetId.ToString().Length <= 0)
                {
                    player.SendChatMessage("Benutzung: /gethere charId");
                    return;
                }
                string targetCharName = Characters.GetCharacterName(targetId);
                if (targetCharName.Length <= 0)
                {
                    HUDHandler.SendNotification(player, 3, 5000, $"Warnung: Die angegebene Character-ID wurde nicht gefunden ({targetId}).");
                    return;
                }
                if (!Characters.ExistCharacterName(targetCharName))
                {
                    HUDHandler.SendNotification(player, 3, 5000, $"Warnung: Der angegebene Charaktername wurde nicht gefunden ({targetCharName} - ID: {targetId}).");
                    return;
                }
                ClassicPlayer targetPlayer = (ClassicPlayer)Alt.Server.GetPlayers().FirstOrDefault(x => x != null && x.Exists && x.GetCharacterMetaId() == (ulong)targetId);
                if (targetPlayer == null || !targetPlayer.Exists) { HUDHandler.SendNotification(player, 4, 5000, "Fehler: Spieler ist nicht online."); return; }
                HUDHandler.SendNotification(targetPlayer, 1, 5000, $"{Characters.GetCharacterName((int)player.GetCharacterMetaId())} hat dich zu Ihm teleportiert.");
                HUDHandler.SendNotification(player, 2, 5000, $"Du hast den Spieler {Characters.GetCharacterName((int)targetPlayer.GetCharacterMetaId())} zu dir teleportiert.");
                targetPlayer.Position = player.Position + new Position(0, 0, 1);
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }


    }
}
