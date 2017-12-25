using Services;

namespace TestingGround
{
    class Program
    {
        static void Main(string[] args)
        {
            FantasyHockeyService service = new FantasyHockeyService();

            service.Test();
        }
    }
}
