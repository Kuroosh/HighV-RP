using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;

namespace Altv_Roleplay.Handler
{
    class CharCreatorHandler : IScript
    {
        [ClientEvent("Server:Charcreator:CreateCEF")]
        public static void CreateCefBrowser(ClassicPlayer client)
        {
            if (client == null || !client.Exists) return;
            client.EmitLocked("Client:Charcreator:CreateCEF");
            client.Position = new Position((float)402.778, (float)-996.9758, (float)-98);
            client.Rotation = new Rotation(0, 0, (float)3.1168559);
        }

        [ClientEvent("Server:Charcreator:CreateCharacter")]
        public static void CreateCharacter(ClassicPlayer client, string charname, string birthdate, bool gender, string facefeaturesarray, string headblendsdataarray, string headoverlaysarray)
        {
            if (client == null || !client.Exists) return;
            if (Characters.ExistCharacterName(charname))
            {
                client.EmitLocked("Client:Charcreator:showError", "Der eingegebene Charaktername ist bereits vergeben.");
                return;
            }
            //ToDo: Abfrage ob Umlaute oder Sonderzeichen im namen sind, falls ja => error

            Characters.CreatePlayerCharacter(client, charname, birthdate, gender, facefeaturesarray, headblendsdataarray, headoverlaysarray);
            client.EmitLocked("Client:Charcreator:DestroyCEF");
            LoginHandler.CreateLoginBrowser(client);
        }

        [ClientEvent("Server:Barber:finishBarber")]
        public static void finishBarber(ClassicPlayer player, string headoverlaysarray)
        {
            if (player == null || !player.Exists) return;
            int charId = User.GetPlayerOnline(player);
            if (charId == 0 || headoverlaysarray == "") return;
            if (!CharactersInventory.ExistCharacterItem(charId, "Bargeld", "inventory") || CharactersInventory.GetCharacterItemAmount(charId, "Bargeld", "inventory") < 50) { HUDHandler.SendNotification(player, 3, 5000, $"Du hast nicht genug Bargeld dabei (50$)."); SetCorrectCharacterSkin(player); return; }
            CharactersInventory.RemoveCharacterItemAmount(charId, "Bargeld", 50, "inventory");
            Characters.SetCharacterHeadOverlays(charId, headoverlaysarray);
        }

        [ClientEvent("Server:Barber:RequestCurrentSkin")]
        public static void SetCorrectCharacterSkin(ClassicPlayer player)
        {
            if (player == null || !player.Exists) return;
            int charid = User.GetPlayerOnline(player);
            if (charid == 0) return;
            player.EmitLocked("Client:SpawnArea:setCharSkin", Characters.GetCharacterSkin("facefeatures", charid), Characters.GetCharacterSkin("headblendsdata", charid), Characters.GetCharacterSkin("headoverlays", charid));
        }
    }
}
