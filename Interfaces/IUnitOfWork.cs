using System.Threading.Tasks;

namespace vega.Interfaces
{
    public interface IUnitOfWork
    {
         Task CompleteAsync();
    }
}