using Services;

namespace TestingGround
{
    class Program
    {
        static void Main(string[] args)
        {
            HockeyFantasyService service = new HockeyFantasyService();

            service.Test();
        }
    }
}
