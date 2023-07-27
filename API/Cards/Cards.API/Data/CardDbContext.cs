using Microsoft.EntityFrameworkCore;
using Cards.API.Models;

namespace Cards.API.Data
{
	public class CardDbContext : DbContext
	{
		// DbSets
		public DbSet<Card> Cards { get; set; }

		public CardDbContext(DbContextOptions options) :base(options) {}
	}
}
