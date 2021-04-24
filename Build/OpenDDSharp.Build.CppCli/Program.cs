using Cake.Frosting;
using OpenDDSharp.Build.CppCli;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}
