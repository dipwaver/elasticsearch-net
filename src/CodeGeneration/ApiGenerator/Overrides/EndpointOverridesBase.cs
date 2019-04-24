﻿using System.Collections.Generic;
using System.Linq;
using ApiGenerator.Domain;
using ApiGenerator.Overrides.Descriptors;

namespace ApiGenerator.Overrides
{
	public abstract class EndpointOverridesBase : IEndpointOverrides
	{
		public virtual IDictionary<string, string> ObsoleteQueryStringParams { get; set; } = new SortedDictionary<string, string>();

		public virtual IDictionary<string, string> RenameQueryStringParams { get; } = new SortedDictionary<string, string>();

		public virtual IEnumerable<string> RenderPartial { get; } = Enumerable.Empty<string>();
		public virtual IEnumerable<string> SkipQueryStringParams { get; } = Enumerable.Empty<string>();


		public virtual CsharpMethod PatchMethod(CsharpMethod method) => method;
	}
}
