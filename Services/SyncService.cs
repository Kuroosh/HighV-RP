using AltV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Altv_Roleplay.Services
{
    public class SyncService : IScript
    {
        private static Thread _mAsyncThread;
        private static List<Task> m_AsyncTasks = new List<Task>();
        private static object l_Lock = new object();

        public SyncService()
        {
            _mAsyncThread = new Thread(StartAsyncThread);
            _mAsyncThread.Start();
        }

        private void StartAsyncThread()
        {
            _mAsyncThread.IsBackground = true;
            _mAsyncThread.Priority = ThreadPriority.BelowNormal;

            AppDomain l_CurrentDomain = AppDomain.CurrentDomain;
            l_CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler);

            while (true)
            {
                try
                {
                    var l_List = new List<Task>();
                    lock (l_Lock)
                    {
                        l_List = m_AsyncTasks.ToList();
                        m_AsyncTasks.Clear();
                    }

                    foreach (var l_Task in l_List)
                        l_Task.RunSynchronously();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e}");
                }
            }
        }

        private void ExceptionHandler(object p_Sender, UnhandledExceptionEventArgs p_Args)
        {
            Exception l_E = (Exception)p_Args.ExceptionObject;
            Console.WriteLine($"{l_E}");
        }

        public static void AddToAsyncThread(Task task)
        {
            lock (l_Lock)
            {
                m_AsyncTasks.Add(task);
            }
        }
    }
}