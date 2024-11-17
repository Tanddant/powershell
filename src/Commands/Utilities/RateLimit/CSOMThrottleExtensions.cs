using Microsoft.SharePoint.Client;
using PnP.Framework.Http;
using System;
using System.Net.Http;

namespace PnP.PowerShell.Commands.Utilities.RateLimit {
	internal static class CSOMThrottleExtensions {
		internal static ClientContext AddRateLimiter(this ClientContext ctx) {
			if (ctx == null)
				throw new ArgumentNullException(nameof(ctx));

			if (ctx.WebRequestExecutorFactory.GetType().FullName != "Microsoft.SharePoint.Client.DefaultWebRequestExecutorFactory")
				throw new InvalidOperationException("Custom WebRequestExecutor already set!");

			if (ctx.WebRequestExecutorFactory.GetType().FullName == "PnP.Framework.Http.HttpClientWebRequestExecutorFactory")
				throw new Exception("Rate limiter already added!");

			HttpClient client = new HttpClient(new ThrottlingHandler() {
				InnerHandler = new HttpClientHandler()
			});

			ctx.WebRequestExecutorFactory = new HttpClientWebRequestExecutorFactory(client);
			return ctx;
		}
	}
}
