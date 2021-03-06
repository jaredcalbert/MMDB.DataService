﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Autofac.Features.OwnedInstances;
using Quartz;

namespace MMDB.DataService.AutofacModules
{
	public class AutofacJobWrapper<T> : IJob where T : IJob
	{
		private Func<Owned<T>> _jobFactory;

		public AutofacJobWrapper(Func<Owned<T>> jobFactory)
		{
			_jobFactory = jobFactory;
		}

		public void Execute(IJobExecutionContext context)
		{
			try 
			{
				using (var ownedJob = _jobFactory())
				{
					var theJob = ownedJob.Value;
					theJob.Execute(context);
				}
			}
			catch(Exception err)
			{
				Debug.WriteLine(err.ToString());
				throw;
			}
		}
	}
}
