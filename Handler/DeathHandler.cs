using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;
using System.Linq;


namespace Altv_Roleplay.Handler
{
    class DeathHandler : IScript
    {
        [ScriptEvent(ScriptEventType.PlayerDead)]
        public static void OnPlayerDeath(ClassicPlayer player, IEntity killer, uint weapon)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                if (charId <= 0) return;
                if (Characters.IsCharacterUnconscious(charId)) return;
                if (Characters.IsCharacterInJail(charId))
                {
                    player.Spawn(new Position(1691.4594f, 2565.7056f, 45.556763f), 0);
                    player.Position = new Position(1691.4594f, 2565.7056f, 45.556763f);
                    return;
                }
                openDeathscreen(player);
                Characters.SetCharacterUnconscious(charId, true, 1); // Von 15 auf 10 geändert.
                ServerFactions.createFactionDispatch(player, 3, $"HandyNotruf", $"Eine Verletzte Person wurde gemeldet");

                if (!(killer is ClassicPlayer killerPlayer)) return;
                if (killerPlayer is null) return;
                WeaponModel weaponModel = (WeaponModel)weapon;
                if (weaponModel == WeaponModel.Fist) return;
                /*if (Enum.IsDefined(typeof(AntiCheat.forbiddenWeapons), (Utils.AntiCheat.forbiddenWeapons)weaponModel))
                {
                    User.SetPlayerBanned(killerPlayer, true, $"Waffen Hack[2]: {weaponModel}");
                    killerPlayer.Kick("");
                    player.Health = 200;
                    foreach (ClassicPlayer p in Alt.Server.GetPlayers().ToList().Where(x => x != null && x.Exists && ((ClassicPlayer)x).CharacterId > 0 && x.AdminLevel() > 0))
                    {
                        p.SendChatMessage($"{Characters.GetCharacterName(killerPlayer.CharacterId)} wurde gebannt: Waffenhack[2] - {weaponModel}");
                    }
                    return;
                }
                */
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }

        internal static void openDeathscreen(ClassicPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                if (charId <= 0) return;
                Position pos = new Position(player.Position.X, player.Position.Y, player.Position.Z + 1);
                player.Spawn(pos, 0);
                player.Emit("Client:Ragdoll:SetPedToRagdoll", true, 0); //Ragdoll setzen
                player.Emit("Client:Deathscreen:openCEF"); // Deathscreen öffnen
                player.SetPlayerIsUnconscious(true);
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }

        internal static void closeDeathscreen(ClassicPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                if (charId <= 0) return;
                player.Emit("Client:Deathscreen:closeCEF");
                player.SetPlayerIsUnconscious(false);
                player.SetPlayerIsFastFarm(false);
                player.Emit("Client:Ragdoll:SetPedToRagdoll", false, 2000);
                Characters.SetCharacterUnconscious(charId, false, 0);
                Characters.SetCharacterFastFarm(charId, false, 0);
                player.Emit("Client:Inventory:StopEffect", "DrugsMichaelAliensFight");

                foreach (var item in CharactersInventory.CharactersInventory_.ToList().Where(x => x.charId == charId))
                {
                    if (item.itemName.Contains("EC Karte") || item.itemName.Contains("Ausweis") || item.itemName.Contains("Fahrzeugschluessel") || ServerItems.GetItemType(ServerItems.ReturnNormalItemName(item.itemName)) == "clothes") continue;
                    CharactersInventory.RemoveCharacterItem(charId, item.itemName, item.itemLocation);
                }

                Characters.SetCharacterWeapon(player, "PrimaryWeapon", "None");
                Characters.SetCharacterWeapon(player, "PrimaryAmmo", 0);
                Characters.SetCharacterWeapon(player, "SecondaryWeapon2", "None");
                Characters.SetCharacterWeapon(player, "SecondaryWeapon", "None");
                Characters.SetCharacterWeapon(player, "SecondaryAmmo2", 0);
                Characters.SetCharacterWeapon(player, "SecondaryAmmo", 0);
                Characters.SetCharacterWeapon(player, "FistWeapon", "None");
                Characters.SetCharacterWeapon(player, "FistWeaponAmmo", 0);
                player.Emit("Client:Smartphone:equipPhone", false, Characters.GetCharacterPhonenumber(charId), Characters.IsCharacterPhoneFlyModeEnabled(charId));
                Characters.SetCharacterPhoneEquipped(charId, false);
                player.RemoveAllPlayerWeapons();
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }
        internal static void revive(ClassicPlayer player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                int charId = (int)player.GetCharacterMetaId();
                if (charId <= 0) return;
                player.EmitLocked("Client:Deathscreen:closeCEF");
                //player.SetPlayerIsUnconscious(false);
                player.Spawn(player.Position, 0);
                Characters.SetCharacterUnconscious(charId, false, 0);
                //DeathHandler.closeDeathscreen(player);
                //player.Spawn(new Position(355.54285f, -596.33405f, 28.75768f));
                player.Health = player.MaxHealth;
                player.Emit("Client:Ragdoll:SetPedToRagdoll", false, 2000);
                //Characters.SetCharacterUnconscious(charId, false, 0);
                ServerFactions.SetFactionBankMoney(3, ServerFactions.GetFactionBankMoney(3) + 1500); //ToDo: Preis anpassen
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }
    }
}
