partial class Program
{
    private static void Main(string[] args)
    {
        Welcome4511();
        Welcome0595();

    }
    static partial void Welcome0595();
    private static void Welcome4511()
    {
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();
        Console.Write(name);
        Console.Write(", welcome to my first console application");
    }
}