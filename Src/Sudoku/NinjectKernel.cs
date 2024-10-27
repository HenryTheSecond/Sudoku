using Ninject;
using Sudoku.Interfaces.Services;
using Sudoku.Services;
using System.Net.Http;

namespace Sudoku;

public class NinjectKernel
{
    private static readonly IKernel kernel = new StandardKernel();

    static NinjectKernel()
    {
        kernel.Bind<HttpClient>().ToMethod(context => new HttpClient
        {
           BaseAddress = new Uri("https://localhost:7060")
        }).InSingletonScope();

        kernel.Bind<ISudokuService>().To<SudokuService>().InSingletonScope();
    }

    public static T Get<T>()
    {
        return kernel.Get<T>();
    }
}
