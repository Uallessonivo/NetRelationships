using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetRelationships.Data;
using NetRelationships.Dtos;
using NetRelationships.Models;

namespace NetRelationships.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public TController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetCharacterById(int id)
        {
            var characters = await _dataContext.Characters
                .Include(b => b.Backpack)
                .Include(w => w.Weapons)
                .Include(f => f.Factions)
                .FirstOrDefaultAsync(c => c.Id == id);

            return Ok(characters);
        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> CreateCharacter(CreateCharacterDto request)
        {
            var newCharacter = new Character
            {
                Name = request.Name,
            };

            var backpack = new Backpack
            {
                Description = request.BackpackDto.Description,
                Character = newCharacter
            };

            var weapons = request.WeaponDtos.Select(w => new Weapon { Name = w.Name, Character = newCharacter }).ToList();
            var factions = request.FactionDtos.Select(f => new Faction { 
                Name = f.Name, 
                Characters = new List<Character> { newCharacter }}
            ).ToList();

            newCharacter.Backpack = backpack;
            newCharacter.Weapons = weapons;
            newCharacter.Factions = factions;

            _dataContext.Characters.Add(newCharacter);
            await _dataContext.SaveChangesAsync();

            var response = await _dataContext.Characters
                .Include(b => b.Backpack)
                .Include(w => w.Weapons)
                .Include(f => f.Factions)
                .ToListAsync();

            return Ok(response);
        }
    }
}
