﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMDB.DataService.Data.Jobs
{
	public class NullJobConfiguration : JobConfigurationBase
	{
		public static NullJobConfiguration Instance { get { return new NullJobConfiguration(); } }
	}
}
