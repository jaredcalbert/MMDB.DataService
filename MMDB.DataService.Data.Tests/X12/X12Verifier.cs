﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MMDB.DataService.Data.Tests.X12
{
	public class X12Verifier
	{
		private char[] LineSeperators { get; set; }

		public X12Verifier(params char[] lineSeperators)
		{
			this.LineSeperators = lineSeperators;
		}

		public void VerifyLine(string expectedData, string actualData, string segmentID, IEnumerable<string> preceedingTags = null, int expectedOccurance = 0)
		{
			Assert.IsNotNullOrEmpty(actualData);
			string[] arrLines = actualData.Split(this.LineSeperators);
			Queue<string> preceedingQueue = new Queue<string>(preceedingTags ?? new string[] {});
			int actualOccurances = 0;
			foreach(string line in arrLines)
			{
				if(preceedingQueue.Count > 0)
				{
					string nextExpectedTag = preceedingQueue.Peek();
					if(line.StartsWith(nextExpectedTag + "*"))
					{
						preceedingQueue.Dequeue();
					}
				}
				if(line.StartsWith(segmentID + "*"))
				{
					if(expectedOccurance == actualOccurances)
					{
						Assert.AreEqual(expectedData, line);
						Assert.IsEmpty(preceedingQueue, "Previous tag not found: {0}",(preceedingQueue.Count()>0)?preceedingQueue.Peek():string.Empty);
						return;
					}
					else 
					{
						actualOccurances++;
					}
				}
			}
			Assert.Fail("Did not find segment " + segmentID);
		}
	}
}
