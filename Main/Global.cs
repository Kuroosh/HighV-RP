using AltV.Net.Async;
using Altv_Roleplay.Factories;
using System;

class Global : Altv_Roleplay.Main
{
    public static class mGlobal
    {
        public class VirtualAPI
        {
            /// <summary>
            /// Permet d'effectuer un appel API 100% Thread-Safe via le taskmanager interne de alt:V
            /// </summary>
            /// <param name="function"></param>
            public void RunThreadSafe(Action function)
            {
                AltAsync.Do(()
                =>
                {
                    function.Invoke();
                });
            }

            public static void TriggerClientEventSafe(ClassicPlayer entity, string eventName, params object[] args)
            {
                if (entity != null && entity.Exists)
                    entity.EmitLocked(eventName, args);
            }
        }
    }
}