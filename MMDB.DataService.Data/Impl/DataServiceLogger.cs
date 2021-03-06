﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using MMDB.DataService.Data.Dto.Logging;

namespace MMDB.DataService.Data.Impl
{
	public class DataServiceLogger : IDataServiceLogger
	{
		private IDocumentSession DocumentSession { get; set; }

		public DataServiceLogger(IDocumentSession documentSession)
		{
			this.DocumentSession = documentSession;
		}

		public virtual ServiceMessage Trace(string message)
		{
			return this.TraceForObject(message, null);
		}

		public virtual ServiceMessage TraceForObject(string message, object dataObject)
		{
			var traceMessage = new ServiceMessage
			{
				Level = EnumServiceMessageLevel.Trace,
				Message = message,
				Detail = message,
				MessageDateTimeUtc = DateTime.UtcNow,
				DataObjectJson = GetDataObjectJson(dataObject)
			};
            //this.DocumentSession.Store(traceMessage);
            this.DocumentSession.SaveChanges();
			return traceMessage;
		}

        private static string GetDataObjectJson(object dataObject)
        {
            string dataObjectJson = null;
            try
            {
                dataObjectJson = (dataObject != null) ? dataObject.ToJson() : null;
            }
            catch (Exception err)
            {
                dataObjectJson = "Error Generating JSON: " + err.Message;
            }
            return dataObjectJson;
        }

		public virtual ServiceMessage InfoForObject(string message, object dataObject)
		{
			var infoMessage = new ServiceMessage
			{
				Level = EnumServiceMessageLevel.Info,
				Message = message,
				Detail = message,
				MessageDateTimeUtc = DateTime.UtcNow,
				DataObjectJson = GetDataObjectJson(dataObject)
			};
            //this.DocumentSession.Store(infoMessage);
            this.DocumentSession.SaveChanges();
			return infoMessage;
		}

		public virtual ServiceMessage Info(string message)
		{
			return this.InfoForObject(message, null);
		}

		public virtual ServiceMessage WarningForObject(string message, object dataObject)
		{
			var warningMessage = new ServiceMessage
			{
				Level = EnumServiceMessageLevel.Warning,
				Message = message,
				Detail = message,
				MessageDateTimeUtc = DateTime.UtcNow,
				DataObjectJson = GetDataObjectJson(dataObject)
			};
            //this.DocumentSession.Store(warningMessage);
            this.DocumentSession.SaveChanges();
			return warningMessage;
		}

		public virtual ServiceMessage Exception(Exception err, object dataObject = null)
		{
			var exceptionMessage = new ServiceMessage
			{
				Level = EnumServiceMessageLevel.Error,
				Message = err.Message,
				Detail = err.ToString(),
				MessageDateTimeUtc = DateTime.UtcNow,
                DataObjectJson = GetDataObjectJson(dataObject)
			};
			this.DocumentSession.Store(exceptionMessage);
			this.DocumentSession.SaveChanges();
			return exceptionMessage;
		}

		public virtual int GetEventCount(EnumServiceMessageLevel? level)
		{
			if (level.HasValue)
			{
				return this.DocumentSession.Query<ServiceMessage>().Where(i => i.Level == level.Value).Count();
			}
			else
			{
				return this.DocumentSession.Query<ServiceMessage>().Count();
			}
		}

		public virtual IQueryable<ServiceMessage> GetEventList(int pageIndex, int pageSize, EnumServiceMessageLevel? level)
		{
			if(level.HasValue)
			{
				return this.DocumentSession.Query<ServiceMessage>().Where(i=>i.Level == level.Value).OrderByDescending(i => i.MessageDateTimeUtc).Skip((pageIndex) * pageSize).Take(pageSize).OrderByDescending(i=>i.MessageDateTimeUtc);
			}
			else 
			{
				return this.DocumentSession.Query<ServiceMessage>().OrderByDescending(i => i.MessageDateTimeUtc).Skip((pageIndex) * pageSize).Take(pageSize);
			}
		}

		public virtual ServiceMessage GetEventItem(int id)
		{
			return this.DocumentSession.Load<ServiceMessage>(id);
		}


	}
}
