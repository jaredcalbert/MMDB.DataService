﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMDB.DataService.Data.Settings
{
	public abstract class SettingsBase 
	{
		public string TypeName 
		{ 
			get 
			{ 
				return this.GetType().FullName;
			} 
		}
	}
}
