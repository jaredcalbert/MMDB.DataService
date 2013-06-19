﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using MMDB.DataService.Data;

namespace MMDB.DataService.WindowsService
{
	partial class WinService : ServiceBase
	{
		private IJobScheduler JobScheduler { get; set; }
		private Thread ProcessingThread { get; set; }
		private volatile bool _stopRequested;

		public WinService(IJobScheduler jobScheduler)
		{
			InitializeComponent();
			this.JobScheduler = jobScheduler;
		}

		protected override void OnStart(string[] args)
		{
			this._stopRequested = false;
			this.ProcessingThread = new Thread(this.ThreadProc);
			this.ProcessingThread.Start();
		}

		protected override void OnStop()
		{
			// TODO: Add code here to perform any tear-down necessary to stop your service.
			this._stopRequested = true;
			for(int i = 0; i < 600; i++)
			{
				if(this.ProcessingThread.IsAlive)
				{
					Thread.Sleep(100);
				}
				else
				{
					break;
				}
			}
			if(this.ProcessingThread.IsAlive)
			{
				this.ProcessingThread.Abort();
				this.ProcessingThread.Join();
			}
			System.Environment.Exit(0);
		}

		public void DebugStart()
		{
			this._stopRequested = false;
			this.ThreadProc();
		}

		private void ThreadProc()
		{
			this.JobScheduler.StartJobs();
			while(!this._stopRequested)
			{
				Thread.Sleep(1000);
			}
			this.JobScheduler.StopJobs();
		}
	}
}
