﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Document;
using MMDB.DataService.Data.DataProvider;
using Raven.Client.Indexes;

namespace MMDB.DataService.Data.Tests
{
	public class EmbeddedRavenProvider : IRavenProvider
	{
		private static IDocumentStore _documentStore;
		public static IDocumentStore DocumentStore 
		{
			get 
			{
				lock(typeof(IDocumentStore))
				{
					if(_documentStore == null)
					{
						_documentStore = new EmbeddableDocumentStore()
						{
							RunInMemory = true,
							Conventions = new DocumentConvention
							{
								DefaultQueryingConsistency =  ConsistencyOptions.QueryYourWrites,
							},
						};
						_documentStore.Initialize();
						IndexCreation.CreateIndexes(typeof(MMDB.DataService.Data.Jobs.DataServiceJobBase<>).Assembly, _documentStore);
					}
				}
				return _documentStore;
			}
		}

		public EmbeddedRavenProvider()
		{
			
		}

		public IDocumentSession CreateSession()
		{
			return EmbeddedRavenProvider.DocumentStore.OpenSession();
		}
	}
}
