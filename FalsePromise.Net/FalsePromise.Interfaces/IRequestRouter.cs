using System;
using System.Threading.Tasks;

namespace FalsePromise.Interfaces
{
    public interface IRequestRouter
    {
        Task<string> Process(string message);
    }
}
