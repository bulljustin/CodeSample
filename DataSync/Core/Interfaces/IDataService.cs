using System;
using System.Collections.Generic;

namespace Core.Interfaces
{
	public interface IDataService<T> : IDisposable
	{
		IEnumerable<T> Get(IFilter filter);

		T Get(long id);

		bool Save(T item);

		bool Save(IEnumerable<T> item);

		bool Delete(long id);
	}
}