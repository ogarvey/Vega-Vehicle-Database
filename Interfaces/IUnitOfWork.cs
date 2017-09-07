using System.Threading.Tasks;

namespace Vega.Interfaces
{
    public interface IUnitOfWork
    {
         Task CompleteAsync();
    }
}