using System;
using System.Runtime.CompilerServices;

namespace Altv_Roleplay.Core
{
    public class Debug
    {
        public static bool DEBUG_MODE_ENABLED = true;
        public static bool DEBUG_MODE_LOG = true;
        public static void OutputDebugString(string textt)
        {
            try
            {
                if (!DEBUG_MODE_ENABLED) return;
                Console.WriteLine("[" + DateTime.Now.Hour + " : " + DateTime.Now.Minute + "] : " + textt);
                string[] text = new string[] { "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~", textt };
                /*if (DEBUG_MODE_LOG)
                {
                    _Gamemodes_.Reallife.vnx_stored_files.logfile.WriteLogs("DebugStrings", text[0]);
                    _Gamemodes_.Reallife.vnx_stored_files.logfile.WriteLogs("DebugStrings", text[1]);
                    _Gamemodes_.Reallife.vnx_stored_files.logfile.WriteLogs("DebugStrings", text[0]);
                }*/
            }
            catch { }
        }
        public static void CatchExceptions(Exception ex, [CallerMemberName] string FunctionName = "")
        {
            if (!DEBUG_MODE_ENABLED) return;
            string[] text = new string[] { "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~", "[EXCEPTION " + FunctionName + "] " + ex.Message, "[EXCEPTION " + FunctionName + "] " + ex.StackTrace };
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text[0]);
            Console.WriteLine(text[1]);
            Console.WriteLine(text[2]);
            Console.WriteLine(text[0]);
            Console.ResetColor();
            /*if (DEBUG_MODE_LOG)
            {
                _Gamemodes_.Reallife.vnx_stored_files.logfile.WriteLogs("Exceptions", text[0]);
                _Gamemodes_.Reallife.vnx_stored_files.logfile.WriteLogs("Exceptions", text[1]);
                _Gamemodes_.Reallife.vnx_stored_files.logfile.WriteLogs("Exceptions", text[2]);
                _Gamemodes_.Reallife.vnx_stored_files.logfile.WriteLogs("Exceptions", text[0]);
            }
            */
        }
    }
}
