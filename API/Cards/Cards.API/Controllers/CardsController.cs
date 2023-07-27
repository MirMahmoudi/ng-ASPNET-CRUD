using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CardsController : ControllerBase
	{
		private readonly CardDbContext _dbContext;

		public CardsController(CardDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		// GET: /api/Cards
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Card>))]
		public async Task<IActionResult> GetCards()
		{
			return Ok(await _dbContext.Cards.ToListAsync());
		}

		// GET: /api/Cards/:id
		[HttpGet]
		[ActionName("GetCard")]
		[Route("{id:guid}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Card))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetCard([FromRoute] Guid id)
		{
			var card = await _dbContext.Cards.SingleOrDefaultAsync(c => c.Id == id);
			return card is null ? NotFound() : Ok(card);
		}

		// POST: /api/Cards
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Card))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddCard([FromBody] Card card)
		{
			card.Id = Guid.NewGuid();

			if (!ModelState.IsValid) return BadRequest();

			await _dbContext.Cards.AddAsync(card);

			await _dbContext.SaveChangesAsync();

			return CreatedAtAction(nameof(GetCard), new { id = card.Id }, card);
		}

		// PUT: /api/Cards
		[HttpPut]
		[Route("{id:guid}")]
		[ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Card))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpdateCard([FromRoute] Guid id, [FromBody] Card card)
		{
			if (!ModelState.IsValid) return BadRequest();

			var cardInDb = await _dbContext.Cards.SingleOrDefaultAsync(c => c.Id == id);

			if (cardInDb is null) return NotFound();

			cardInDb.CardholderName = card.CardholderName;
			cardInDb.CardNumber = card.CardNumber;
			cardInDb.ExpiryMonth = card.ExpiryMonth;
			cardInDb.ExpiryYear = card.ExpiryYear;
			cardInDb.CVC = card.CVC;

			await _dbContext.SaveChangesAsync();

			return Accepted(new Uri($"{Request.GetDisplayUrl()}/{id}"), card);
		}

		// DELETE: /api/Cards/:id
		[HttpDelete]
		[Route("{id:guid}")]
		[ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Card))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
		{
			var cardInDb = await _dbContext.Cards.SingleOrDefaultAsync(c => c.Id == id);

			if (cardInDb is null) return NotFound();

			_dbContext.Cards.Remove(cardInDb);

			await _dbContext.SaveChangesAsync();

			return Accepted(new Uri($"{Request.GetDisplayUrl()}/{id}"), cardInDb);
		}
	}
}
