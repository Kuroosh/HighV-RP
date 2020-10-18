using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Resources.Chat.Api;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Model;
using Altv_Roleplay.Utils;
using System;
using System.Linq;

namespace Altv_Roleplay.Handler
{
    public class Commands : IScript
    {



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // User Commands 
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Command("support")]
        public void SupportCommand(ClassicPlayer player, string msg)
        {
            try
            {
                if (player == null || !player.Exists || User.GetPlayerOnline(player) <= 0) return;
                foreach (var admin in Alt.Server.GetPlayers().Where(x => x != null && x.Exists && x.HasSyncedMetaData("isDuty") && x.AdminLevel() > 0))
                {
                    admin.SendChatMessage($"[SUPPORT] {Characters.GetCharacterName(User.GetPlayerOnline(player))} (ID: {User.GetPlayerOnline(player)}) benötigt Support: {msg}");
                }
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }


        [Command("id")]
        public static void GetPlayerID(ClassicPlayer player)
        {
            if (player is null) return;
            HUDHandler.SendNotification(player, 3, 5000, "ID : " + player.CharacterId);
        }



        /*[Command("testt")]
        public void testtCommand(ClassicPlayer player)
        {
            if (player.AdminLevel() <= 8) { HUDHandler.SendNotification(player, 4, 5000, "Keine Rechte."); return; }
            try
            {
                Alt.Log("petrit");
                Alt.EmitAllClients("InteractSound_CL:PlayOnAll", "demo", "1.0");
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }*/




        /*
        */









        /*

        [Command("call", true)]
        public void CallCMD(ClassicPlayer player, int targetId)
        {
            try
            {
                if (player == null || !player.Exists) return;
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
                var targetPlayer = Alt.Server.GetPlayers().FirstOrDefault(x => x != null && x.Exists && x.GetCharacterMetaId() == (ulong)targetId);
                if (targetPlayer == null || !targetPlayer.Exists) { HUDHandler.SendNotification(player, 4, 5000, "Fehler: Spieler ist nicht online."); return; }
                player.EmitLocked("SaltyChat_EstablishedCall",targetPlayer);
                targetplayer.EmitLocked("SaltyChat_EstablishedCall",player);

            }
            catch (Exception e)
            {
Core.Debug.CatchExceptions(e);
            }
        }

    */

    }
}
