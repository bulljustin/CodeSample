using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataSync.Handlers;
using DataSync.Helpers;
using DataSync.Processors;

namespace DataSync
{
	internal class Program
	{
		private static void Main(string[] args)
		{
#if DEBUG
			Console.WriteLine("DEBUG MODE ENABLED");
			args = new[] { "externalName" };
#endif
			var tasks = new List<Task>();
			var exes = new ConcurrentBag<Exception>();


			foreach (var arg in args)
			{
				switch (arg.ToLower())
				{
					case "externalName":
						tasks.Add(Task.Run(() =>
							{
								try
								{
									var settings = new ProcessorSettings
									{
										InternalHandler = new InternalHandler(),
										ExternalHandler = new ExternalHandler()
									};
									using (var processor = new InternalExternalProcessor(settings))
									{
										processor.ProcessAll();
									}
								}
								catch (Exception ex)
								{
									exes.Add(ex);
#if DEBUG
									Debugger.Break();
#endif
								}
							}
						));
						break;

					case "otherOption":
						tasks.Add(Task.Run(() =>
							{
								try
								{
									var settings = new ProcessorSettings
									{
										InternalHandler = new InternalHandler(),
										ExternalHandler = new OtherOptionHandler()
									};
									using (var processor = new OtherOptionProcessor(settings))
									{
										processor.ProcessAll();
									}
								}
								catch (Exception ex)
								{
									exes.Add(ex);
#if DEBUG
									Debugger.Break();
#endif
								}
							}
						));
						break;
				}
			}
			Task.WaitAll(tasks.ToArray());
			if (exes.Any())
			{
				var ex = new AggregateException("Default sync error catcher", exes);
#if DEBUG
				Debugger.Break();
#else
				//handle error;
#endif
			}
#if DEBUG
			Console.WriteLine("Run completed");
			Console.ReadLine();
#endif
		}
	}
}