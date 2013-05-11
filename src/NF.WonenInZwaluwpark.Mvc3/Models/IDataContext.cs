using System.Data.Linq;

namespace NF.WonenInZwaluwpark.Mvc3.Models
{
    public interface IDataContext
    {
        Table<TEntity> GetTable<TEntity>() where TEntity : class;
    }
}
