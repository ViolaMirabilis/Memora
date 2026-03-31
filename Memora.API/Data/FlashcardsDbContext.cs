using SimpleAUTH.Models;
using Microsoft.EntityFrameworkCore;

namespace SimpleAUTH.Data
{
    public class FlashcardsDbContext : DbContext
    {
        /// <summary>
        /// Four tables that the DB consists of for now.
        /// </summary>
        public DbSet<User> Users { get; set; }
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<FlashcardSet> FlashcardSets { get; set; }
        public DbSet<FlashcardFolder> FlashcardFolders { get; set; }

        public FlashcardsDbContext(DbContextOptions<FlashcardsDbContext> options) : base(options)
        {

        }
    }
}
