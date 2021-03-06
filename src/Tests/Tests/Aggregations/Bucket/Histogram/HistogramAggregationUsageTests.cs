﻿using System;
using FluentAssertions;
using Nest;
using Tests.Core.Extensions;
using Tests.Core.ManagedElasticsearch.Clusters;
using Tests.Domain;
using Tests.Framework.Integration;
using static Nest.Infer;

namespace Tests.Aggregations.Bucket.Histogram
{
	public class HistogramAggregationUsageTests : AggregationUsageTestBase
	{
		public HistogramAggregationUsageTests(ReadOnlyCluster i, EndpointUsage usage) : base(i, usage) { }

		protected override object AggregationJson => new
		{
			commits = new
			{
				histogram = new
				{
					field = "numberOfCommits",
					interval = 100.0,
					missing = 0.0,
					order = new
					{
						_key = "desc"
					},
					offset = 1.1
				}
			}
		};

		protected override Func<AggregationContainerDescriptor<Project>, IAggregationContainer> FluentAggs => a => a
			.Histogram("commits", h => h
				.Field(p => p.NumberOfCommits)
				.Interval(100)
				.Missing(0)
				.Order(HistogramOrder.KeyDescending)
				.Offset(1.1)
			);

		protected override AggregationDictionary InitializerAggs =>
			new HistogramAggregation("commits")
			{
				Field = Field<Project>(p => p.NumberOfCommits),
				Interval = 100,
				Missing = 0,
				Order = HistogramOrder.KeyDescending,
				Offset = 1.1
			};

		protected override void ExpectResponse(SearchResponse<Project> response)
		{
			response.ShouldBeValid();
			var commits = response.Aggregations.Histogram("commits");
			commits.Should().NotBeNull();
			commits.Buckets.Should().NotBeNull();
			commits.Buckets.Count.Should().BeGreaterThan(0);
			foreach (var item in commits.Buckets)
				item.DocCount.Should().BeGreaterThan(0);
		}
	}
}
