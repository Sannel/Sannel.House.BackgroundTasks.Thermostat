using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sannel.House.BackgroundTasks.Thermostat
{
	public class Startup
	{
		private static Startup current = null;
		private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

		internal ServiceProvider Provider
		{
			get;
			private set;
		}

		internal static async Task<Startup> GetStartupAsync()
		{
			await semaphoreSlim.WaitAsync();
			try
			{
				if (current != null)
				{
					return current;
				}

				current = new Startup();
				await current.InitAsync();
				return current;
			}
			finally
			{
				semaphoreSlim.Release();
			}
		}

		private Startup()
		{

		}

		internal async Task InitAsync()
		{
			var serviceCollection = new ServiceCollection();

			serviceCollection.AddLogging();

			Provider = serviceCollection.BuildServiceProvider();
		}

	}
}
