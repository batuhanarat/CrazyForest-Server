using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Services;
using SharedLibrary;
using SharedLibrary.Requests;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class HeroController: ControllerBase
{
    
    private readonly IHeroService _heroService;
    private readonly GameDbContext _context;

    public HeroController(IHeroService heroService,GameDbContext context)
    {
        _heroService = heroService;
        _context = context;

        
    }
    
    
    
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var heroIdsAvailableClaim = User.FindFirst("heroes");

        if (heroIdsAvailableClaim == null)
            return Unauthorized();

        var heroIdsAvailable = JsonConvert.DeserializeObject<List<int>>(heroIdsAvailableClaim.Value);

        if (!heroIdsAvailable.Contains(id))
            return Unauthorized();

        var hero = _context.Heroes.FirstOrDefault(h => h.Id == id);

        if (hero == null)
            return NotFound();

        return Ok(hero);
    }


    
    [HttpPost("{id}")]
    public IActionResult Edit([FromRoute] int id,[FromBody] CreateHeroRequest request)
    {
        var heroIdsAvailable = JsonConvert.DeserializeObject<List<int>>(User.FindFirst("heroes").Value);


        if (!heroIdsAvailable.Contains(id)) return Unauthorized();

        var hero = _context.Heroes.First(h => h.Id == id);
       
       
        hero.Name = request.Name;
        _context.SaveChanges();


        return Ok();
    }

    [HttpPost]
    public Hero Post(CreateHeroRequest request)
    {

        var userId = int.Parse(User.FindFirst("id").Value);

        var user = _context.Users.Include(u => u.Heroes).First(u => u.Id == userId);
        

         var hero = new Hero()
        {
            Name = request.Name,
            User = user
        };

        _context.Add(hero);
        _context.SaveChanges();
        // hero.User = null;
        //_context.Entry(user).Collection(u => u.Heroes).Load();

        return hero;
        
    }
    
    
    
}