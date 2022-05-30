namespace RainDrops
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using(RainDropsGame game = new RainDropsGame())
            {
                game.Run();
            }
        }
    }
}
