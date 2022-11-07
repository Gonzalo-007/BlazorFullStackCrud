using BlazorFullStackCrud.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorFullStackCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;

        public SuperHeroController(DataContext context)
        {
            _context = context;
        }
        /*********************************************************************/
        /* LIST all heroes from our database.
         * we use iclude method to get as well all comics from another table */
        /*********************************************************************/
        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetSuperHeroes()
        {
            var heroes = await _context.SuperHeroes.Include(sh => sh.Comic).ToListAsync();
            return Ok(heroes);
        }
        /*************************************/
        /* List all comics from our database */
        /*************************************/
        [HttpGet("comics")]
        public async Task<ActionResult<List<Comic>>> GetComics()
        {
            var comics = await _context.Comics.ToListAsync();
            return Ok(comics);
        }

        /********************************************************************/
        /* List a single hero by id from our database
         * We use iclude method to get as well all comics from another table*/
        /********************************************************************/
        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> GetSingleHero(int id)
        {
            var hero = await _context.SuperHeroes
                .Include(hero => hero.Comic)
                .FirstOrDefaultAsync(hero => hero.Id == id);
            if (hero == null)
            {
                return BadRequest("Sorry no hero here. :/");
            }
            return Ok(hero);
        }

        /*********************************************/
        /* CREATE a new SuperHeero into our database */
        /*********************************************/
        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> CreateSuperHero(SuperHero hero)
        {
            hero.Comic = null;
            _context.SuperHeroes.Add(hero);
            await _context.SaveChangesAsync();
            return Ok(await GetDbHeroes());
        }

        /*********************************************/
        /* List SuperHero from database  */
        /*********************************************/
        private async Task<List<SuperHero>> GetDbHeroes()
        {
            return await _context.SuperHeroes.Include(sh => sh.Comic).ToListAsync();
        }

        /*********************************************/
        /* UPDATE a new SuperHeero into our database */
        /*********************************************/
        [HttpPut("{id}")]
        public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(SuperHero hero, int id)
        {
            var dbHero = await _context.SuperHeroes
                .Include(sh => sh.Comic)
                .FirstOrDefaultAsync(sh => sh.Id == id);
            if(dbHero == null)
            {
                return NotFound($"The SuperHero with id number {id} does not exist");
            }

            dbHero.FirstName = hero.FirstName;
            dbHero.LastName = hero.LastName;
            dbHero.HeroName = hero.HeroName;
            dbHero.ComicId = hero.ComicId;

            await _context.SaveChangesAsync();
            return Ok(await GetDbHeroes());
        }

        /*********************************************/
        /* DELETE a new SuperHeero into our database */
        /*********************************************/
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero( int id)
        {
            var dbHero = await _context.SuperHeroes
                .Include(sh => sh.Comic)
                .FirstOrDefaultAsync(sh => sh.Id == id);
            if (dbHero == null)
            {
                return NotFound($"The SuperHero with id number {id} does not exist");
            }

            _context.SuperHeroes.Remove(dbHero);           

            await _context.SaveChangesAsync();
            return Ok(await GetDbHeroes());
        }
    }
}
