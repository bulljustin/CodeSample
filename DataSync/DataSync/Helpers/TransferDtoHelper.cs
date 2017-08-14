using AutoMapper;
using Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSync.Helpers
{
	public abstract class TransferDtoHelper<T> where T : ITransferDto<T>
	{
		public static T FindExisting(IEnumerable<T> searchList, T finder)
		{
			var found = searchList.FirstOrDefault(w =>
				(w.Id > 0 && w.Id == finder.Id)
				||
				(w.ExternalId ?? "This is a fake external ID.").Equals(finder.ExternalId ?? string.Empty, StringComparison.OrdinalIgnoreCase)
			);
			return found != null ? found.Clone() : default(T);
		}

		public static T Update(IEnumerable<T> searchList, T updater)
		{
			var existing = FindExisting(searchList, updater);
			if (existing != null)
			{
				var Id = existing.Id > 0 ? existing.Id : updater.Id;
				var externalId = string.IsNullOrWhiteSpace(updater.ExternalId) ? existing.ExternalId : updater.ExternalId;
				Mapper.Map(updater, existing);
				return existing;
			}
			return updater;
		}

		/// <summary>
		/// Merges 2 lists of transfer item dtos into a single list and updates according to the transfer object's rules
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="internalItems"></param>
		/// <param name="externalItems"></param>
		/// <returns></returns>
		public static CombinedTransfer<T> CombineTransferDtos(IEnumerable<T> internalItems, IEnumerable<T> externalItems)
		{
			var combinedItems = new ConcurrentDictionary<Tuple<long, string>, T>();
#if DEBUG
			var totalItems = internalItems.Count() + externalItems.Count();
			var processedItems = 0;
			var typeName = typeof(T).Name;
			var lockObject = new object();
#endif
			var tasks = new List<Task>();
			internalItems.ToList().ForEach(intrn =>
			{
				tasks.Add(Task.Run(() =>
				{
					var extrn = Update(externalItems, intrn);
					var item = (extrn != null ? extrn : intrn).Clone();
					combinedItems.TryAdd(new Tuple<long, string>(item.Id, item.ExternalId), item);
#if DEBUG
					lock (lockObject)
					{
						processedItems++;
						ShowProgressOnConsole(processedItems, totalItems, typeName);
					}
#endif
				}));
			});
			Task.WaitAll(tasks.ToArray());
			externalItems.ToList().ForEach(ext =>
			{
				tasks.Add(Task.Run(() =>
				{
					if (FindExisting(combinedItems.Values, ext) == null)
					{
						var pvc = Update(internalItems, ext);
						var item = (pvc != null ? pvc : ext).Clone();
						combinedItems.TryAdd(new Tuple<long, string>(item.Id, item.ExternalId), item);
					}
#if DEBUG
					lock (lockObject)
					{
						processedItems++;
						ShowProgressOnConsole(processedItems, totalItems, typeName);
					}
#endif
				}));
			});
			Task.WaitAll(tasks.ToArray());
#if DEBUG
			Console.WriteLine();
#endif

			var combinedList = combinedItems.Values.ToList();
			var internalProcessed = combinedList.Select(item => (T)item.Clone()).ToList();
			var externalProcessed = combinedList.Select(item => (T)item.Clone()).ToList();
			//remove the ones that haven't changed
			if (internalItems.Any() && externalItems.Any())
			{
				Parallel.Invoke(
					() =>
					{
						internalProcessed.RemoveAll(all => internalItems.Any(all.Equals));
					},
					() =>
					{
						externalProcessed.RemoveAll(all => externalItems.Any(all.Equals));
					}
				);
			}

#if DEBUG
			Console.WriteLine("Combining {0} completed.", typeName);
#endif

			var returnDto = new CombinedTransfer<T>(externalProcessed, internalProcessed, combinedList);
			return returnDto;
		}

		private static void ShowProgressOnConsole(int complete, int total, string transferDto)
		{
			if (total > 0 && (complete % 15 == 0 || complete == total))
			{
				var result = string.Format("\r{0}: {1} of {2} ({3}%)", transferDto, complete, total, Math.Round(100 * complete / (decimal)total, 2));
				if (result.Length > 80)
				{
					result = result.Substring(0, 80);
				}
				Console.Write(result.PadRight(80, ' '));
			}
		}
	}

	public class CombinedTransfer<T>
	{
		public ICollection<T> ExternalProcessed { get; set; }
		public ICollection<T> InternalProcessed { get; set; }
		public ICollection<T> Combined { get; set; }

		public CombinedTransfer(ICollection<T> extProcessed, ICollection<T> intProcessed, ICollection<T> combined)
		{
			ExternalProcessed = extProcessed;
			InternalProcessed = intProcessed;
			Combined = combined;
		}
	}
}