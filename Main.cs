using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Elements.Refs;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Handler;
using Altv_Roleplay.Model;
using Altv_Roleplay.Services;
using Altv_Roleplay.Utils;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Altv_Roleplay
{
    public class Main : AsyncResource
    {
        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new AccountsFactory();
        }

        public override IBaseObjectFactory<IColShape> GetColShapeFactory()
        {
            return new ColshapeFactory();
        }

        public override IEntityFactory<IVehicle> GetVehicleFactory()
        {
            return new VehicleFactory();
        }

        public override void OnStart()
        {
            Environment.SetEnvironmentVariable("COMPlus_legacyCorruptedState­­ExceptionsPolicy", "1");

            #region Database Init
            Database.DatabaseHandler.ResetDatabaseOnlineState();
            Database.DatabaseHandler.LoadAllPlayers();
            Database.DatabaseHandler.LoadAllPlayerCharacters();
            Database.DatabaseHandler.LoadAllCharacterClothes();
            Database.DatabaseHandler.LoadAllCharacterSkins();
            Database.DatabaseHandler.LoadAllCharacterBankAccounts();
            Database.DatabaseHandler.LoadAllCharacterLastPositions();
            Database.DatabaseHandler.LoadAllCharacterInventorys();
            Database.DatabaseHandler.LoadAllCharacterLicenses();
            Database.DatabaseHandler.LoadAllCharacterPermissions();
            Database.DatabaseHandler.LoadAllCharacterMinijobData();
            Database.DatabaseHandler.LoadAllCharacterPhoneChats();
            Database.DatabaseHandler.LoadAllCharacterWanteds();
            Database.DatabaseHandler.LoadAllServerBlips();
            Database.DatabaseHandler.LoadAllServerMarkers();
            Database.DatabaseHandler.LoadAllServerVehiclesGlobal();
            Database.DatabaseHandler.LoadAllServerAnimations();
            Database.DatabaseHandler.LoadAllServerATMs();
            Database.DatabaseHandler.LoadAllServerBanks();
            Database.DatabaseHandler.LoadAllServerBankPapers();
            Database.DatabaseHandler.LoadAllServerItems();
            Database.DatabaseHandler.LoadAllServerPeds();
            Database.DatabaseHandler.LoadAllClothesShops();
            Database.DatabaseHandler.LoadAllServerShops();
            Database.DatabaseHandler.LoadAllServerShopItems();
            Database.DatabaseHandler.LoadAllServerBarbers();
            Database.DatabaseHandler.LoadAllServerTeleports();
            Database.DatabaseHandler.LoadAllGarages();
            Database.DatabaseHandler.LoadAllGarageSlots();
            Database.DatabaseHandler.LoadAllServerVehicleMods();
            Database.DatabaseHandler.LoadAllVehicleMods();
            Database.DatabaseHandler.LoadAllVehicles();
            Database.DatabaseHandler.LoadAllVehicleTrunkItems();
            Database.DatabaseHandler.LoadAllVehicleShops();
            Database.DatabaseHandler.LoadAllVehicleShopItems();
            Database.DatabaseHandler.LoadAllServerFarmingSpots();
            Database.DatabaseHandler.LoadAllServerFarmingProducers();
            Database.DatabaseHandler.LoadAllServerJobs();
            Database.DatabaseHandler.LoadAllServerLicenses();
            Database.DatabaseHandler.LoadAllServerFuelStations();
            Database.DatabaseHandler.LoadALlServerFuelStationSpots();
            Database.DatabaseHandler.LoadAllServerTabletAppData();
            Database.DatabaseHandler.LoadAllCharactersTabletApps();
            Database.DatabaseHandler.LoadAllCharactersTabletTutorialEntrys();
            Database.DatabaseHandler.LoadAllServerTabletEvents();
            Database.DatabaseHandler.LoadAllServerTabletNotes();
            Database.DatabaseHandler.LoadAllServerCompanys();
            Database.DatabaseHandler.LoadAllServerCompanyMember();
            Database.DatabaseHandler.LoadAllServerFactions();
            Database.DatabaseHandler.LoadAllServerFactionRanks();
            Database.DatabaseHandler.LoadAllServerFactionMembers();
            Database.DatabaseHandler.LoadAllServerFactionStorageItems();
            Database.DatabaseHandler.LoadAllServerDoors();
            Database.DatabaseHandler.LoadAllServerHotels();
            Database.DatabaseHandler.LoadAllServerHouses();
            Database.DatabaseHandler.LoadAllServerMinijobBusdriverRoutes();
            Database.DatabaseHandler.LoadAllServerMinijobBusdriverRouteSpots();
            Database.DatabaseHandler.LoadAllServerMinijobGarbageSpots();
            Database.DatabaseHandler.LoadAllServerLogsFaction();
            Database.DatabaseHandler.LoadAllServerLogsCompany();
            #endregion

            #region Minijobs Init
            Minijobs.Elektrolieferant.Main.Initialize();
            Minijobs.Pilot.Main.Initialize();
            Minijobs.Müllmann.Main.Initialize();
            Minijobs.Busfahrer.Main.Initialize();
            #endregion

            #region Register Events
            Alt.OnColShape += ColAction;
            //Alt.OnClient<IPlayer, string, string>("Server:Login:ValidateLoginCredentials", LoginHandler.ValidateLoginCredentials);
            //Alt.OnClient<IPlayer, int>("Server:Charselector:PreviewCharacter", LoginHandler.PreviewCharacter);
            //Alt.OnClient<IPlayer, string, string>("Server:Charselector:spawnChar", Handler.LoginHandler.CharacterSelectedSpawnPlace);
            //Alt.OnClient<IPlayer, int>("Server:Charselector:KillCharacter", Characters.KillCharacter);
            //Alt.OnClient<IPlayer, string, string, string, string>("Server:Register:RegisterNewPlayer", Handler.RegisterHandler.RegisterNewPlayer);
            //Alt.OnClient<IPlayer>("Server:Charcreator:CreateCEF", Handler.CharCreatorHandler.CreateCefBrowser);
            //Alt.OnClient<IPlayer, string, string, bool, string, string, string>("Server:Charcreator:CreateCharacter", Handler.CharCreatorHandler.CreateCharacter);
            /*Alt.OnClient<IPlayer, float, float, float>("ServerBlip:TpWayPoint", tptoWaypoint); //ToDo: entfernen*/
            //Alt.OnClient<IPlayer>("Server:Inventory:RequestInventoryItems", Handler.InventoryHandler.RequestInventoryItems);
            //Alt.OnClient<IPlayer, string, int, string, string>("Server:Inventory:switchItemToDifferentInv", Handler.InventoryHandler.switchItemToDifferentInv);
            //Alt.OnClient<IPlayer, string, int, string>("Server:Inventory:UseItem", Handler.InventoryHandler.UseItem);
            //Alt.OnClient<IPlayer, string, int, string>("Server:Inventory:DropItem", Handler.InventoryHandler.DropItem);
            //Alt.OnClient<IPlayer, string, int, string, int>("Server:Inventory:GiveItem", InventoryHandler.GiveItem);
            //Alt.OnClient<IPlayer>("Server:KeyHandler:PressE", Handler.KeyHandler.PressE);
            //Alt.OnClient<IPlayer>("Server:KeyHandler:PressU", KeyHandler.PressU);
            //Alt.OnClient<IPlayer, string>("Server:HUD:sendIdentityCardApplyForm", Handler.TownhallHandler.sendIdentityCardApplyForm);
            //Alt.OnClient<IPlayer, string>("Server:Bank:CreateNewBankAccount", Handler.BankHandler.CreateNewBankAccount);
            //Alt.OnClient<IPlayer, string, string>("Server:Bank:BankAccountAction", Handler.BankHandler.BankAccountAction);
            //Alt.OnClient<IPlayer>("Server:Inventory:closeCEF", Handler.InventoryHandler.CloseInventoryCEF);
            //Alt.OnClient<IPlayer, bool>("Server:CEF:setCefStatus", setCefStatus);
            //Alt.OnClient<IPlayer, int>("Server:ATM:requestBankData", Handler.BankHandler.requestATMBankData);
            //Alt.OnClient<IPlayer, int, int, string>("Server:ATM:WithdrawMoney", Handler.BankHandler.WithdrawATMMoney);
            //Alt.OnClient<IPlayer, int, int, string>("Server:ATM:DepositMoney", Handler.BankHandler.DepositATMMoney);
            //Alt.OnClient<IPlayer, int, int, int, string>("Server:ATM:TransferMoney", Handler.BankHandler.TransferATMMoney);
            //Alt.OnClient<IPlayer, string, int>("Server:ATM:TryPin", Handler.BankHandler.TryATMPin);
            //Alt.OnClient<IPlayer, int, int, string>("Server:Shop:buyItem", Handler.ShopHandler.buyShopItem);
            //Alt.OnClient<IPlayer, int, int, string>("Server:Shop:sellItem", ShopHandler.sellShopItem);
            //Alt.OnClient<IPlayer, string>("Server:Barber:finishBarber", Handler.CharCreatorHandler.finishBarber);
            //Alt.OnClient<IPlayer>("Server:Barber:RequestCurrentSkin", Handler.CharCreatorHandler.SetCorrectCharacterSkin);
            //Alt.OnClient<IPlayer, IVehicle>("Server:Raycast:LockVehicle", RaycastHandler.LockVehicle);
            //Alt.OnClient<IPlayer, IVehicle>("Server:Raycast:ToggleVehicleEngine", RaycastHandler.ToggleVehicleEngine);
            //Alt.OnClient<IPlayer, IVehicle>("Server:Raycast:OpenVehicleFuelMenu", RaycastHandler.OpenVehicleFuelMenu);
            //Alt.OnClient<IPlayer, IVehicle>("Server:Raycast:OpenCloseVehicleTrunk", RaycastHandler.OpenCloseVehicleTrunk);
            //Alt.OnClient<IPlayer, IVehicle>("Server:Raycast:ViewVehicleTrunk", RaycastHandler.ViewVehicleTrunk);
            //Alt.OnClient<IPlayer, IVehicle>("Server:Raycast:ViewVehicleGlovebox", RaycastHandler.ViewVehicleGlovebox);
            //Alt.OnClient<IPlayer, int, int, string, int, string, string>("Server:VehicleTrunk:StorageItem", VehicleHandler.VehicleTrunkStorageItem);
            //Alt.OnClient<IPlayer, int, int, string, int, string>("Server:VehicleTrunk:TakeItem", VehicleHandler.VehicleTrunkTakeItem);
            //Alt.OnClient<IPlayer, IPlayer>("Server:Raycast:showPlayerSupportId", RaycastHandler.showPlayerSupportId);
            //Alt.OnClient<IPlayer, IPlayer, string>("Server:Raycast:OpenGivePlayerBillCEF", RaycastHandler.OpenGivePlayerBillCEF);
            //Alt.OnClient<IPlayer, IPlayer>("Server:Raycast:RevivePlayer", Factions.LSFD.Functions.RevivePlayer);
            //Alt.OnClient<IPlayer, string, string, int, int>("Server:PlayerBill:giveBill", RaycastHandler.PlayerBillGiveBill);
            //Alt.OnClient<IPlayer, string, string, int, int, string, int>("Server:PlayerBill:BillAction", RaycastHandler.PlayerBillAction);
            //Alt.OnClient<IPlayer, IPlayer>("Server:Raycast:givePlayerItemRequest", RaycastHandler.givePlayerItemRequest);
            //Alt.OnClient<IPlayer, IPlayer>("Server:Raycast:GiveTakeHandcuffs", RaycastHandler.GiveTakeHandcuffs);
            //Alt.OnClient<IPlayer, IPlayer>("Server:Raycast:GiveTakeRopeCuffs", RaycastHandler.GiveTakeRopeCuffs);
            //Alt.OnClient<IPlayer, IPlayer>("Server:Raycast:SearchPlayerInventory", RaycastHandler.SearchPlayerInventory);
            //Alt.OnClient<IPlayer, int, string, string, int>("Server:PlayerSearch:TakeItem", InventoryHandler.PlayerSearchTakeItem);
            //Alt.OnClient<IPlayer, IPlayer>("Server:Raycast:openGivePlayerLicenseCEF", RaycastHandler.openGivePlayerLicenseCEF);
            //Alt.OnClient<IPlayer, int, string, int>("Server:Garage:DoAction", GarageHandler.DoGarageAction);
            //Alt.OnClient<IPlayer, int, string>("Server:VehicleShop:BuyVehicle", ShopHandler.BuyVehicle);
            //Alt.OnClient<IPlayer, float>("Server:Vehicle:UpdateVehicleKM", HUDHandler.UpdateVehicleKM);
            //Alt.OnClient<IPlayer, string>("Server:Jobcenter:SelectJob", TownhallHandler.SelectJobcenterJob);
            //Alt.OnClient<IPlayer, int, int, string, int, int>("Server:FuelStation:FuelVehicleAction", FuelStationHandler.FuelVehicle);
            //Alt.OnClient<IPlayer>("Server:ClothesShop:RequestCurrentSkin", Characters.SetCharacterCorrectClothes);
            //Alt.OnClient<IPlayer, int, int, string>("Server:ClothesShop:buyItem", ShopHandler.buyShopItem);
            //Alt.OnClient<IPlayer>("Server:Tablet:openCEF", TabletHandler.openCEF);
            //Alt.OnClient<IPlayer>("Server:Tablet:RequestTabletData", TabletHandler.RequestTabletData);
            //Alt.OnClient<IPlayer, string, bool>("Server:Tablet:AppStoreInstallUninstallApp", TabletHandler.AppStoreInstallUninstallApp);
            //Alt.OnClient<IPlayer, int, string, int>("Server:Tablet:BankingAppNewTransaction", TabletHandler.BankingAppNewTransaction);
            //Alt.OnClient<IPlayer, string, string, string, string, string, string, string>("Server:Tablet:EventsAppNewEntry", TabletHandler.EventsAppNewEntry);
            //Alt.OnClient<IPlayer, string, string, string>("Server:Tablet:NotesAppNewNote", TabletHandler.NotesAppNewNote);
            //Alt.OnClient<IPlayer, int>("Server:Tablet:NotesAppDeleteNote", TabletHandler.NotesAppDeleteNote);
            //Alt.OnClient<IPlayer, string, int, string>("Server:Tablet:VehicleStoreBuyVehicle", TabletHandler.VehicleStoreBuyVehicle);
            //Alt.OnClient<IPlayer, string, int>("Server:Tablet:CompanyAppInviteNewMember", TabletHandler.CompanyAppInviteNewMember);
            //Alt.OnClient<IPlayer>("Server:Tablet:CompanyAppLeaveCompany", TabletHandler.CompanyAppLeaveCompany);
            //Alt.OnClient<IPlayer, int, int>("Server:Tablet:CompanyAppRankAction", TabletHandler.CompanyAppRankAction);
            //Alt.OnClient<IPlayer, string, int, int>("Server:FactionBank:DepositMoney", BankHandler.DepositFactionMoney);
            //Alt.OnClient<IPlayer, string, int, int>("Server:FactionBank:WithdrawMoney", BankHandler.WithdrawFactionMoney);
            //Alt.OnClient<IPlayer, string, int, int>("Server:Tablet:FactionManagerAppInviteNewMember", TabletHandler.FactionManagerAppInviteNewMember);
            //Alt.OnClient<IPlayer, string, int>("Server:Tablet:FactionManagerRankAction", TabletHandler.FactionManagerRankAction);
            //Alt.OnClient<IPlayer, int, int>("Server:Tablet:FactionManagerSetRankPaycheck", TabletHandler.FactionManagerSetRankPaycheck);
            //Alt.OnClient<IPlayer, int, int, string, int, string>("Server:FactionStorage:StorageItem", FactionHandler.FactionStorageStorageItem);
            //Alt.OnClient<IPlayer, int, int, string, int>("Server:FactionStorage:TakeItem", FactionHandler.FactionStorageTakeItem);
            //Alt.OnClient<IPlayer, string, IVehicle>("Server:InteractionMenu:GetMenuVehicleItems", RaycastHandler.GetMenuVehicleItems);
            //Alt.OnClient<IPlayer, string, IPlayer>("Server:InteractionMenu:GetMenuPlayerItems", RaycastHandler.GetMenuPlayerItems);
            //Alt.OnClient<IPlayer, string, int, string, string>("Server:VehicleLicensing:LicensingAction", VehicleHandler.LicensingAction);
            //Alt.OnClient<IPlayer, string>("Server:Tablet:LSPDAppSearchPerson", Factions.LSPD.Functions.LSPDAppSearchPerson);
            //Alt.OnClient<IPlayer, string>("Server:Tablet:LSPDAppSearchVehiclePlate", Factions.LSPD.Functions.LSPDAppSearchVehiclePlate);
            //Alt.OnClient<IPlayer, string>("Server:Tablet:LSPDAppSearchLicense", Factions.LSPD.Functions.LSPDAppSearchLicense);
            //Alt.OnClient<IPlayer, string, string>("Server:Tablet:LSPDAppTakeLicense", Factions.LSPD.Functions.LSPDAppTakeLicense);
            //Alt.OnClient<IPlayer, int, string>("Server:GivePlayerLicense:GiveLicense", Factions.LSFS.Functions.GiveLicense);
            //Alt.OnClient<IPlayer, string>("Server:Tablet:JusticeAppGiveWeaponLicense", Factions.Justice.Functions.GiveWeaponLicense);
            //Alt.OnClient<IPlayer, string>("Server:Tablet:JusticeAppSearchBankAccounts", Factions.Justice.Functions.SearchBankAccounts);
            //Alt.OnClient<IPlayer, int>("Server:Tablet:JusticeAppViewBankTransactions", Factions.Justice.Functions.ViewBankTransactions);
            //Alt.OnClient<IPlayer, int>("Server:MinijobPilot:StartJob", Minijobs.Pilot.Main.StartMiniJob);
            //Alt.OnClient<IPlayer, int>("Server:MinijobBusdriver:StartJob", Minijobs.Busfahrer.Main.StartMiniJob);
            //Alt.OnClient<IPlayer, int, int>("Server:Hotel:RentHotel", HotelHandler.RentHotel);
            //Alt.OnClient<IPlayer, int, int>("Server:Hotel:LockHotel", HotelHandler.LockHotel);
            //Alt.OnClient<IPlayer, int, int>("Server:Hotel:EnterHotel", HotelHandler.EnterHotel);
            //Alt.OnClient<IPlayer, int, string, int, string>("Server:HotelStorage:StorageItem", HotelHandler.StorageHotelItem);
            //Alt.OnClient<IPlayer, int, string, int>("Server:HotelStorage:TakeItem", HotelHandler.TakeHotelItem);
            //Alt.OnClient<IPlayer, int>("Server:House:BuyHouse", HouseHandler.BuyHouse);
            //Alt.OnClient<IPlayer, int>("Server:House:EnterHouse", HouseHandler.EnterHouse);
            //Alt.OnClient<IPlayer, int, string, int, string>("Server:HouseStorage:StorageItem", HouseHandler.StorageItem);
            //Alt.OnClient<IPlayer, int, string, int>("Server:HouseStorage:TakeItem", HouseHandler.TakeItem);
            //Alt.OnClient<IPlayer, int>("Server:House:RentHouse", HouseHandler.RentHouse);
            //Alt.OnClient<IPlayer, int>("Server:House:UnrentHouse", HouseHandler.UnrentHouse);
            //Alt.OnClient<IPlayer, int, int>("Server:HouseManage:setRentPrice", HouseHandler.setRentPrice);
            //Alt.OnClient<IPlayer, int, string>("Server:HouseManage:setRentState", HouseHandler.setRentState);
            //Alt.OnClient<IPlayer, int, int>("Server:HouseManage:RemoveRenter", HouseHandler.RemoveRenter);
            //Alt.OnClient<IPlayer, int, string>("Server:HouseManage:BuyUpgrade", HouseHandler.BuyUpgrade);
            //Alt.OnClient<IPlayer, int, int>("Server:HouseManage:WithdrawMoney", HouseHandler.WithdrawMoney);
            //Alt.OnClient<IPlayer, int, int>("Server:HouseManage:DepositMoney", HouseHandler.DepositMoney);
            //Alt.OnClient<IPlayer, int, string>("Server:Tablet:sendDispatchToFaction", TabletHandler.sendDispatchToFaction);
            //Alt.OnClient<IPlayer, int, int>("Server:Tablet:DeleteFactionDispatch", TabletHandler.DeleteFactionDispatch);
            //Alt.OnClient<IPlayer, IPlayer>("Server:Raycast:showIdcard", RaycastHandler.showIdCard);
            //Alt.OnClient<IPlayer, int>("Server:House:SellHouse", HouseHandler.SellHouse);
            //Alt.OnClient<IPlayer, int>("Server:House:setMainHouse", HouseHandler.setMainHouse);
            //Alt.OnClient<IPlayer, IPlayer>("Server:Raycast:healPlayer", Factions.LSFD.Functions.HealPlayer);
            //Alt.OnClient<IPlayer, IVehicle>("Server:Raycast:RepairVehicle", Factions.ACLS.Functions.RepairVehicle);
            //Alt.OnClient<IPlayer, IVehicle>("Server:Raycast:towVehicle", Factions.ACLS.Functions.TowVehicle);
            //Alt.OnClient<IPlayer, IVehicle>("Server:Raycast:tuneVehicle", Factions.ACLS.Functions.openTuningMenu);
            //Alt.OnClient<IPlayer, IVehicle, string, string, int, int, int>("Server:Tuning:switchTuningColor", Factions.ACLS.Functions.switchTuningColor);
            //Alt.OnClient<IPlayer, IVehicle>("Server:Tuning:resetToNormal", Factions.ACLS.Functions.resetTuningToNormal);
            //Alt.OnClient<IPlayer, IVehicle, string, int, string>("Server:Tuning:switchTuning", Factions.ACLS.Functions.switchTuning);
            Alt.OnClient<ClassicPlayer, string, int, int>("Server:Utilities:createNewMod", CreateMod);
            Alt.OnClient<ClassicPlayer, string>("Server:Utilities:BanMe", BanPlayer);
            #endregion

            //WTF ?!!!!!!!! 
            //WTF ?!!!!!!!!
            //WTF ?!!!!!!!!
            //WTF ?!!!!!!!!
            //WTF ?!!!!!!!!
            /*
            System.Timers.Timer entityTimer = new System.Timers.Timer();
            //System.Timers.Timer desireTimer = new System.Timers.Timer();
            System.Timers.Timer VehicleAutomaticParkFetchTimer = new System.Timers.Timer();
            entityTimer.Elapsed += new ElapsedEventHandler(TimerHandler.OnEntityTimer);
            //desireTimer.Elapsed += new ElapsedEventHandler(TimerHandler.OnDesireTimer);
            VehicleAutomaticParkFetchTimer.Elapsed += new ElapsedEventHandler(TimerHandler.VehicleAutomaticParkFetch);
            entityTimer.Interval += 60000;
            //desireTimer.Interval += 300000;
            VehicleAutomaticParkFetchTimer.Interval += 60000 * 5;
            entityTimer.Enabled = true;
            //desireTimer.Enabled = true;
            VehicleAutomaticParkFetchTimer.Enabled = true;
            #endregion
*/
            Timer entityTimer = new Timer(TimerHandler.OnMinuteSpent, null, 60000, 60000);
            Console.WriteLine($"Main-Thread = {Thread.CurrentThread.ManagedThreadId}");
        }

        #region Event Functions
        private void BanPlayer(ClassicPlayer player, string msg)
        {
            try
            {
                if (player == null || !player.Exists)
                {
                    return;
                }

                Alt.Log($"Ban Me: {player.Name} - {DateTime.Now.ToString()}");

                int charId = User.GetPlayerOnline(player);
                if (charId <= 0)
                {
                    return;
                }

                player.Kick("");

                User.SetPlayerBanned(Characters.GetCharacterAccountId(charId), true, $"Grund: {msg}");
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }

        private void CreateMod(ClassicPlayer player, string MName, int MType, int MID)
        {
            try
            {
                if (player == null || !player.Exists)
                {
                    return;
                }

                if (player.IsInVehicle)
                {
                    if (player.Vehicle == null || !player.Vehicle.Exists)
                    {
                        return;
                    }
                    uint vehHash = player.Vehicle.Model;
                    bool success = ServerVehicles.AddVehicleMods(vehHash, MName, MType, MID);

                    if (success)
                    {
                        HUDHandler.SendNotification(player, 2, 2500, $"Mod gespeichert: {vehHash} - {MName} - {MType} - {MID}");
                        Alt.Log($"Mod erfolgreich gespeichert");
                    }
                    else
                    {
                        HUDHandler.SendNotification(player, 4, 2500, $"Mod konnte nicht gespeichert werden (existiert er schon?): {vehHash} - {MName} - {MType} - {MID}");
                        Alt.Log($"FEHLER: MOD NICHT GESPEICHERT {MType} - {MID} - {MName}");
                    }
                }
            }
            catch (Exception e)
            {
                Core.Debug.CatchExceptions(e);
            }
        }

        private void ColAction(IColShape colShape, IEntity targetEntity, bool state)
        {
            if (colShape == null)
            {
                return;
            }
            if (!colShape.Exists)
            {
                return;
            }
            IPlayer client = targetEntity as IPlayer;

            if (client == null || !client.Exists)
            {
                return;
            }
            string colshapeName = colShape.GetColShapeName();
            ulong colshapeId = colShape.GetColShapeId();

            if (colshapeName == "Cardealer" && state == true)
            {
                ulong vehprice = colShape.GetColshapeCarDealerVehPrice();
                string vehname = colShape.GetColshapeCarDealerVehName();
                HUDHandler.SendNotification(client, 1, 2500, $"Name: {vehname}<br>Preis: {vehprice}$");
                return;
            }
            else if (colshapeName == "DoorShape" && state)
            {
                var doorData = ServerDoors.ServerDoors_.FirstOrDefault(x => x.id == (int)colshapeId);
                if (doorData == null) return;
                client.EmitLocked("Client:DoorManager:ManageDoor", doorData.hash, new Position(doorData.posX, doorData.posY, doorData.posZ), (bool)doorData.state);
            }
        }

        private void TeleportToWaypoint(ClassicPlayer player, float x, float y, float z)
        {
            if (player == null) return;
            player.Position = new Position(x, y, z);
        }

        public override void OnStop()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (ClassicVehicle Veh in Alt.Server.GetVehicles().ToList())
            {
                if (Veh == null || !Veh.Exists) continue;
                using var vRef = new VehicleRef(Veh);
                if (!vRef.Exists) continue;
                lock (Veh)
                {
                    if (Veh == null || !Veh.Exists) continue;
                    int vehID = Veh.id;
                    if (vehID <= 0) continue;
                    if (Veh.EngineOn == true) Veh.Fuel -= 0.03f;
                    int currentGarageId = ServerVehicles.GetVehicleGarageId(Veh);
                    if (currentGarageId <= 0) continue;
                    ServerVehicles.SetVehicleInGarage(Veh, true, currentGarageId);
                    Database.DatabaseHandler.UpdateVehicle(Veh);
                }
            }

            stopwatch.Reset();
            stopwatch.Start();

            foreach (ClassicPlayer player in Alt.Server.GetPlayers().ToList())
            {
                if (player == null) continue;

                using var playerReference = new PlayerRef(player);
                if (!playerReference.Exists) return;
                if (player == null || !player.Exists) continue;

                lock (player)
                {
                    if (player == null || !player.Exists) continue;

                    int charId = User.GetPlayerOnline(player);
                    if (charId > 0)
                    {
                        Characters.SetCharacterLastPosition(charId, player.Position, player.Dimension);

                        if (User.IsPlayerBanned(player))
                        {
                            player.kickWithMessage($"Du bist gebannt. (Grund: {User.GetPlayerBanReason(player)}).");
                        }
                        Characters.SetCharacterHealth(charId, player.Health);
                        Characters.SetCharacterArmor(charId, player.Armor);
                        WeatherHandler.SetRealTime(player);
                        if (player.IsInVehicle)
                        {
                            player.EmitLocked("Client:HUD:GetDistanceForVehicleKM");
                            HUDHandler.SendInformationToVehicleHUD(player);
                        }
                        Characters.IncreaseCharacterPaydayTime(charId);

                        if (Characters.IsCharacterUnconscious(charId))
                        {
                            int unconsciousTime = Characters.GetCharacterUnconsciousTime(charId);
                            if (unconsciousTime > 0)
                            {
                                Characters.SetCharacterUnconscious(charId, true, unconsciousTime - 1);
                            }
                            else if (unconsciousTime <= 0)
                            {
                                Characters.SetCharacterUnconscious(charId, false, 0);
                                DeathHandler.closeDeathscreen(player);
                                player.Spawn(new Position(355.54285f, -596.33405f, 28.75768f), 0);
                                player.Health = player.MaxHealth;
                            }
                        }

                        if (Characters.IsCharacterFastFarm(charId))
                        {
                            int fastFarmTime = Characters.GetCharacterFastFarmTime(charId);
                            if (fastFarmTime > 0)
                            {
                                Characters.SetCharacterFastFarm(charId, true, fastFarmTime - 1);
                            }
                            else if (fastFarmTime <= 0)
                            {
                                Characters.SetCharacterFastFarm(charId, false, 0);
                            }
                        }

                        if (Characters.IsCharacterInJail(charId))
                        {
                            int jailTime = Characters.GetCharacterJailTime(charId);
                            if (jailTime > 0)
                            {
                                Characters.SetCharacterJailTime(charId, true, jailTime - 1);
                            }
                            else if (jailTime <= 0)
                            {
                                if (CharactersWanteds.HasCharacterWanteds(charId))
                                {
                                    int jailTimes = CharactersWanteds.GetCharacterWantedFinalJailTime(charId);
                                    int jailPrice = CharactersWanteds.GetCharacterWantedFinalJailPrice(charId);

                                    if (CharactersBank.HasCharacterBankMainKonto(charId))
                                    {
                                        int accNumber = CharactersBank.GetCharacterBankMainKonto(charId);
                                        int bankMoney = CharactersBank.GetBankAccountMoney(accNumber);

                                        CharactersBank.SetBankAccountMoney(accNumber, bankMoney - jailPrice);
                                        HUDHandler.SendNotification(player, 1, 7500, $"Durch deine Inhaftierung wurden dir {jailPrice}$ vom Konto abgezogen.");
                                    }
                                    HUDHandler.SendNotification(player, 1, 7500, $"Du sitzt nun für {jailTimes} Minuten im Gefängnis.");
                                    Characters.SetCharacterJailTime(charId, true, jailTimes);
                                    CharactersWanteds.RemoveCharacterWanteds(charId);
                                    player.Position = new Position(1691.4594f, 2565.7056f, 45.556763f);
                                    if (Characters.GetCharacterGender(charId) == false)
                                    {
                                        player.EmitLocked("Client:SpawnArea:setCharClothes", 11, 5, 0);
                                        player.EmitLocked("Client:SpawnArea:setCharClothes", 3, 5, 0);
                                        player.EmitLocked("Client:SpawnArea:setCharClothes", 4, 7, 15);
                                        player.EmitLocked("Client:SpawnArea:setCharClothes", 6, 7, 0);
                                        player.EmitLocked("Client:SpawnArea:setCharClothes", 8, 1, 88);
                                    }
                                }
                                else
                                {
                                    Characters.SetCharacterJailTime(charId, false, 0);
                                    Characters.SetCharacterCorrectClothes(player);
                                    player.Position = new Position(1846.022f, 2585.8945f, 45.657f);
                                    HUDHandler.SendNotification(player, 1, 2500, "Du wurdest aus dem Gefängnis entlassen.");
                                }
                            }
                        }

                        if (Characters.GetCharacterPaydayTime(charId) >= 60)
                        {
                            Characters.IncreaseCharacterPlayTimeHours(charId);
                            Characters.ResetCharacterPaydayTime(charId);

                            if (CharactersBank.HasCharacterBankMainKonto(charId))
                            {
                                int accountNumber = CharactersBank.GetCharacterBankMainKonto(charId);
                                CharactersBank.SetBankAccountMoney(accountNumber, CharactersBank.GetBankAccountMoney(accountNumber) + 250); //250$ Stütze
                                ServerBankPapers.CreateNewBankPaper(accountNumber, DateTime.Now.ToString("d", CultureInfo.CreateSpecificCulture("de-DE")), DateTime.Now.ToString("t", CultureInfo.CreateSpecificCulture("de-DE")), "Eingehende Überweisung", "Staat", "Arbeitslosengeld", "+250$", "Unbekannt");

                                if (!Characters.IsCharacterCrimeFlagged(charId) && Characters.GetCharacterJob(charId) != "None" && DateTime.Now.Subtract(Convert.ToDateTime(Characters.GetCharacterLastJobPaycheck(charId))).TotalHours >= 12 && !ServerFactions.IsCharacterInAnyFaction(charId))
                                {
                                    if (Characters.GetCharacterJobHourCounter(charId) >= ServerJobs.GetJobNeededHours(Characters.GetCharacterJob(charId)) - 1)
                                    {
                                        int jobCheck = ServerJobs.GetJobPaycheck(Characters.GetCharacterJob(charId));
                                        Characters.SetCharacterLastJobPaycheck(charId, DateTime.Now);
                                        Characters.ResetCharacterJobHourCounter(charId);
                                        CharactersBank.SetBankAccountMoney(accountNumber, CharactersBank.GetBankAccountMoney(accountNumber) + jobCheck);
                                        ServerBankPapers.CreateNewBankPaper(accountNumber, DateTime.Now.ToString("d", CultureInfo.CreateSpecificCulture("de-DE")), DateTime.Now.ToString("t", CultureInfo.CreateSpecificCulture("de-DE")), "Eingehende Überweisung", "Arbeitsamt", $"Gehalt: {Characters.GetCharacterJob(charId)}", $"+{jobCheck}$", "Unbekannt");
                                        HUDHandler.SendNotification(player, 1, 5000, $"Gehalt erhalten (Beruf: {Characters.GetCharacterJob(charId)} | Gehalt: {jobCheck}$)");
                                    }
                                    else
                                    {
                                        Characters.IncreaseCharacterJobHourCounter(charId);
                                    }
                                }

                                if (ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId))
                                {
                                    int factionid = ServerFactions.GetCharacterFactionId(charId);
                                    int factionPayCheck = ServerFactions.GetFactionRankPaycheck(factionid, ServerFactions.GetCharacterFactionRank(charId));

                                    if (ServerFactions.GetFactionBankMoney(factionid) >= factionPayCheck)
                                    {
                                        ServerFactions.SetFactionBankMoney(factionid, ServerFactions.GetFactionBankMoney(factionid) - factionPayCheck);
                                        CharactersBank.SetBankAccountMoney(accountNumber, CharactersBank.GetBankAccountMoney(accountNumber) + factionPayCheck);
                                        HUDHandler.SendNotification(player, 1, 5000, $"Du hast deinen Lohn i.H.v. {factionPayCheck}$ erhalten ({ServerFactions.GetFactionRankName(factionid, ServerFactions.GetCharacterFactionRank(charId))})");
                                        ServerBankPapers.CreateNewBankPaper(accountNumber, DateTime.Now.ToString("d", CultureInfo.CreateSpecificCulture("de-DE")), DateTime.Now.ToString("t", CultureInfo.CreateSpecificCulture("de-DE")), "Eingehende Überweisung", $"{ServerFactions.GetFactionFullName(factionid)}", $"Gehalt: {ServerFactions.GetFactionRankName(factionid, ServerFactions.GetCharacterFactionRank(charId))}", $"+{factionPayCheck}$", "Dauerauftrag");
                                        LoggingService.NewFactionLog(factionid, charId, 0, "paycheck", $"{Characters.GetCharacterName(charId)} hat seinen Lohn i.H.v. {factionPayCheck}$ erhalten ({ServerFactions.GetFactionRankName(factionid, ServerFactions.GetCharacterFactionRank(charId))}).");
                                    }
                                    else
                                    {
                                        HUDHandler.SendNotification(player, 3, 5000, $"Deine Fraktion hat nicht genügend Geld um dich zu bezahlen ({factionPayCheck}$).");
                                    }
                                }

                                var playerVehicles = ServerVehicles.ServerVehicles_.Where(x => x.id > 0 && x.charid == charId && x.plate.Contains("NL"));
                                int taxMoney = 0;

                                foreach (var i in playerVehicles)
                                {
                                    if (!i.plate.Contains("NL")) continue;
                                    taxMoney += ServerAllVehicles.GetVehicleTaxes(i.hash);
                                }

                                if (playerVehicles != null && taxMoney > 0)
                                {
                                    if (CharactersBank.GetBankAccountMoney(accountNumber) < taxMoney)
                                    {
                                        HUDHandler.SendNotification(player, 3, 5000, $"Deine Fahrzeugsteuern konnten nicht abgebucht werden ({taxMoney}$)");
                                    }
                                    else
                                    {
                                        CharactersBank.SetBankAccountMoney(accountNumber, CharactersBank.GetBankAccountMoney(accountNumber) - taxMoney);
                                        ServerBankPapers.CreateNewBankPaper(accountNumber, DateTime.Now.ToString("d", CultureInfo.CreateSpecificCulture("de-DE")), DateTime.Now.ToString("t", CultureInfo.CreateSpecificCulture("de-DE")), "Ausgehende Überweisung", "Zulassungsamt", $"Fahrzeugsteuer", $"-{taxMoney}$", "Bankeinzug");
                                        HUDHandler.SendNotification(player, 1, 5000, $"Du hast deine Fahrzeugsteuern i.H.v. {taxMoney}$ bezahlt.");
                                    }
                                }
                            }
                            else
                            {
                                HUDHandler.SendNotification(player, 3, 5000, $"Dein Einkommen konnte nicht überwiesen werden da du kein Hauptkonto hast.");
                            }
                        }
                    }
                }
            }

            stopwatch.Stop();
            Alt.Log($"OnEntityTimer: Player Foreach benötigte: {stopwatch.Elapsed}");

            stopwatch.Stop();
            Alt.Log($"Saved: Vehicle Foreach benötigte: {stopwatch.Elapsed}");

            foreach (var player in Alt.Server.GetPlayers().Where(p => p != null && p.Exists)) player.Kick("");
            Alt.Log("Server ist gestoppt.");
        }
        #endregion
    }
}
