using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using System;

namespace Altv_Roleplay.Factories
{
    public class ClassicPlayer : Player
    {
        private int _CharacterId { get; set; }
        public int CharacterId { get { return _CharacterId; } set { _CharacterId = value; this.SetSyncedMetaData("PLAYER_ID", value); } }
        private string _Username { get; set; }
        public string Username { get { return _Username; } set { _Username = value; this.SetSyncedMetaData("PLAYER_USERNAME", value); } }
        public string CharacterName { get; set; } = "None";
        public string FarmingAction { get; set; } = "None";
        public bool IsUsingCrowbar { get; set; }
        public bool IsUnconscious { get; set; }
        public string CurrentMinijob { get; set; } = "None";
        public string CurrentMinijobStep { get; set; } = "None";
        public int CurrentMinijobActionCount { get; set; }
        public int CurrentMinijobRouteId { get; set; }
        public bool isRobbingAShop { get; set; }
        public bool Seatbelt { get; set; }
        public int AdminLevel { get; set; }
        public void GivePlayerWeapon(WeaponModel weapon, int ammo, bool selected)
        {
            try { Alt.Emit("GlobalSystems:GiveWeapon", this, (uint)weapon, ammo, selected); }
            catch { }
        }
        public void RemovePlayerWeapon(WeaponModel weapon)
        {
            try { Alt.Emit("GlobalSystems:RemovePlayerWeapon", this, (uint)weapon); }
            catch { }
        }
        public void RemoveAllPlayerWeapons()
        {
            try { Alt.Emit("GlobalSystems:RemoveAllPlayerWeapons", this); }
            catch { }
        }


        public ClassicPlayer(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
        }
    }
}
