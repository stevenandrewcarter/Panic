using System.Collections.Generic;
namespace Panic.Repository
{
  /// <summary>
  /// Default Interface for a Repository
  /// </summary>
  public interface IRepository<T>
  {
    T GetByID(int id);
    List<T> GetAll();
    bool Add(T entity);
    bool Remove(T entity);
  }
}
