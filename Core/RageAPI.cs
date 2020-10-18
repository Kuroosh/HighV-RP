using AltV.Net.Elements.Entities;
using System;

namespace Altv_Roleplay.Core
{
    public static class RageAPI
    {
        public static T rpGetElementData<T>(this IBaseObject element, string key)
        {
            try
            {
                if (element == null) return default;
                if (element.GetData(key, out T value)) return value;
                return default;
            }
            catch { return default; }
        }
        public static void rpSetElementData(this IBaseObject element, string key, object value)
        {
            try
            {
                if (element == null) return;
                element.SetData(key, value);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
        public static void rpSetSharedElementData<T>(this IEntity element, string key, T value)
        {
            try
            {
                if (element == null) return;
                element.SetData(key, value);
                element.SetSyncedMetaData(key, value);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
        public static void rpSetStreamSharedElementData<T>(this IEntity element, string key, T value)
        {
            try
            {
                if (element == null) return;
                element.SetData(key, value);
                element.SetStreamSyncedMetaData(key, value);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
    }
}
