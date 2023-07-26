namespace Server.Services;



public interface IHeroService
{
    public void DoSomething();
    

}

public class HeroService: IHeroService
{
    public void DoSomething()
    {
        Console.WriteLine("hey");
    }
}

public class MockHeroService: IHeroService
{
    public void DoSomething()
    {
        Console.WriteLine("hey from the mock");
    }
}