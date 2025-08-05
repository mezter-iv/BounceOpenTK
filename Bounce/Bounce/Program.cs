namespace Bounce
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(1000, 1000)) {
                game.Run();
            }
        }
    }
}
