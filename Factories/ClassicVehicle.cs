using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System;

namespace Altv_Roleplay.Factories
{
    public class ClassicVehicle : Vehicle
    {
        public int id { get; set; }
        public int charid { get; set; }
        public ulong hash { get; set; }
        public int vehType { get; set; } //0 Spieler, 1 Fraktion, 2 Jobfahrzeug
        public int faction { get; set; } //0 = Keine, 1  = DoJ, 2 = LSPD, 3 = LSFD, 4 = ACLS
        public bool engineState { get; set; } //True = an, false = aus
        public bool isEngineHealthy { get; set; }
        public bool lockState { get; set; } //True = zu, false = auf
        public bool isInGarage { get; set; } //True = in Garage, false = nicht in Garage
        public int garageId { get; set; }
        public float posX { get; set; }
        public float posY { get; set; }
        public float posZ { get; set; }
        public float rotX { get; set; }
        public float rotY { get; set; }
        public float rotZ { get; set; }
        public string plate { get; set; }
        public DateTime lastUsage { get; set; }
        public DateTime buyDate { get; set; }

        //
        public int VehicleId { get; set; }
        public bool Trunkstate { get; set; }
        public bool IsAdmin { get; set; }
        private float _Fuel { get; set; }
        public float Fuel { get { return _Fuel; } set { _Fuel = value; this.SetSyncedMetaData("VEHICLE_FUEL", value); } }
        private float _KM { get; set; }
        public float KM { get { return _KM; } set { _KM = value; this.SetSyncedMetaData("VEHICLE_KM", value); } }
        private bool _Locked { get; set; }
        public bool Locked { get { return _Locked; } set { _Locked = value; this.SetSyncedMetaData("VEHICLE_LOCKED", value); } }

        public ClassicVehicle(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
        }

        public ClassicVehicle(uint model, Position position, Rotation rotation) : base(model, position, rotation)
        {
        }
    }
}
