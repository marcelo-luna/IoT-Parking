﻿using System;
using System.Threading.Tasks;

namespace ParkingPanel.Services
{
    public class ParkingService
    {
        public int ParkingCount { get; private set; }
        public event EventHandler ParkingEventHandler;

        public ParkingService()
        {
        }

        public Task UpdateParking(int parkingCount)
        {
            ParkingCount = parkingCount;
            ParkingEventHandler?.Invoke(this, EventArgs.Empty);
           return Task.CompletedTask;
        }

        //private Task UpdateParking()
        //{
        //    Task.Run(async () => 
        //    {
        //        while (true)
        //        {
        //            ParkingCount++;
        //            ParkingEventHandler?.Invoke(this, EventArgs.Empty);
        //            await Task.Delay(1000);
        //        }
        //    });

        //    return Task.CompletedTask;
        //}
    }
}
