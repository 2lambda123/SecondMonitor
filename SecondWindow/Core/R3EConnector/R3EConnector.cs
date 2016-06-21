﻿using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using SecondWindow.Core.R3EConnector.Data;
using System.Threading;

namespace SecondWindow.Core.R3EConnector
{    

    public class R3EConnector : IR3EConnector
    {

        private MemoryMappedFile sharedMemory;
        private MemoryMappedViewAccessor sharedMemoryAccessor;
        private Thread daemonThread;
        private bool disconnect;

        private static readonly string r3EExcecutable = "RRRE";
        private static readonly string SharedMemoryName = "$Race$";

        private TimeSpan timeInterval = TimeSpan.FromMilliseconds(100);

        public event EventHandler<R3EDataEventArgs> DataLoaded;

        public R3EConnector()
        {
            TickTime = 100;
        }

        public bool Connected
        {
            get { return (sharedMemory != null && sharedMemoryAccessor != null); }
        }

        public int TickTime
        {
            get
            {
                return timeInterval.Milliseconds;
            }
            set
            {
                timeInterval = TimeSpan.FromMilliseconds(value);
            }
        }

        public static bool IsRrreRunning()
        {
            return Process.GetProcessesByName(r3EExcecutable).Length > 0;
        }

        public void AsynConnect()
        {
            new Thread(AsynConnector).Start();
        }

        public bool TryConnect()
        {
            return Connect();
        }

        private bool Connect()
        {
            if (!IsRrreRunning())
                return false;
            try
            {
                sharedMemory = MemoryMappedFile.OpenExisting(SharedMemoryName);
                sharedMemoryAccessor = sharedMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(R3ESharedData)));
                StartDaemon();
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        private void AsynConnector()
        {
            while(!TryConnect())
            {
                Thread.Sleep(10);
            }
        }

        private void StartDaemon()
        {
            if (daemonThread != null)
                throw new InvalidOperationException("Daemon is already running");
            daemonThread = new Thread(DaemonMethod);
            daemonThread.Start();
        }

        private void DaemonMethod()
        {
            TimeSpan _timeAlive = TimeSpan.FromMinutes(10);
            DateTime timeReset = DateTime.UtcNow;
            DateTime timeLast = timeReset;
            while (!disconnect && sharedMemoryAccessor.CanRead)
            {

                DateTime timeNow = DateTime.UtcNow;
                if (timeNow.Subtract(timeReset) > _timeAlive)
                {
                    break;
                }

                if (timeNow.Subtract(timeLast) < timeInterval)
                {
                    Thread.Sleep(5);
                    continue;
                }
                R3ESharedData data = Load();
            }
            sharedMemoryAccessor = null;
            sharedMemory = null;
            disconnect = false;
        }

        private void RaiseDataLoadedEvent(R3ESharedData data)
        {
            R3EDataEventArgs args = new R3EDataEventArgs(data);
            EventHandler<R3EDataEventArgs> handler = DataLoaded;
            if(handler!=null)
            {
                handler(this, args);
            }
        }

        private void Disconnect()
        {
            disconnect = true;
            sharedMemoryAccessor = null;
            sharedMemory = null;
        }

        private R3ESharedData Load()
        {
            lock (sharedMemoryAccessor)
            {
                if (!Connected)
                    throw new InvalidOperationException("Not connected");
                R3ESharedData data;
                sharedMemoryAccessor.Read(0, out data);
                return data;
            }
        }
    }
}
