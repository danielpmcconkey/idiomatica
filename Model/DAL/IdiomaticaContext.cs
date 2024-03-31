using Microsoft.EntityFrameworkCore;

namespace Model.DAL
{
    public class IdiomaticaContext : DbContext
    {
        #region DBSets
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookStat> BookStats { get; set; }
        public DbSet<BookTag> BookTags { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Sentence> Sentences { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<WordFlashMessage> WordFlashMessages { get; set; }
        public DbSet<WordImage> WordImages { get; set; }
        public DbSet<WordTag> WordTags { get; set; }
		#endregion

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var dbPath = @"E:\Lute\backups\lute_backup_2024-03-22_075709.db.gz_2024-03-22_075827.db";
			dbPath = "C:\\Users\\Dan\\AppData\\Local\\Packages\\PythonSoftwareFoundation.Python.3.12_qbz5n2kfra8p0\\LocalCache\\Local\\Lute3\\Lute3\\lute.db";
			optionsBuilder.UseSqlite($"Data Source={dbPath}");

			// only turn on query logging when debugging
			//optionsBuilder.LogTo(Console.WriteLine);
		}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>(e => {
                e.HasMany(l => l.Books)
                    .WithOne(b => b.Language)
                    .HasForeignKey(b => b.LanguageId);
                e.HasMany(l => l.Words)
                    .WithOne(w => w.Language)
                    .HasForeignKey(w => w.LanguageId);
            });
            modelBuilder.Entity<Book>(e => {
                e.HasOne(b => b.BookStat)
                    .WithOne(bs => bs.Book)
                    .HasForeignKey<BookStat>(bs => bs.BookId);
                e.HasMany(b => b.BookTags)
                    .WithOne(bt => bt.Book)
                    .HasForeignKey(bt => bt.BookId);
                e.HasOne(b => b.User)
                    .WithMany(u => u.Books)
                    .HasForeignKey(b => b.UserId);
				e.HasOne(b => b.Language)
					.WithMany(l => l.Books)
					.HasForeignKey(b => b.LanguageId);
				e.HasMany(b => b.Pages)
					.WithOne(p => p.Book)
					.HasForeignKey(p => p.BookId);
            });

            modelBuilder.Entity<BookStat>(e => { });
            modelBuilder.Entity<BookTag>(e => {
                e.HasOne(b => b.Book)
                    .WithMany(b => b.BookTags)
                    .HasForeignKey(x => x.BookId);
                e.HasOne(t => t.Tag)
                    .WithMany(t => t.BookTags)
                    .HasForeignKey(x => x.TagId);
            });
            modelBuilder.Entity<Sentence>(e => { });
            modelBuilder.Entity<Setting>(e => { });
            modelBuilder.Entity<Status>(e => { });
            modelBuilder.Entity<Tag>(e => { });
            modelBuilder.Entity<Word>(e => {
                e.HasMany(w => w.ParentWords)
                    .WithMany(w => w.ChildWords)
                    .UsingEntity(
                        "wordparents",
                        l => l.HasOne(typeof(Word)).WithMany().HasForeignKey("WpParentWoID").HasPrincipalKey(nameof(Word.Id)),
                        r => r.HasOne(typeof(Word)).WithMany().HasForeignKey("WpWoID").HasPrincipalKey(nameof(Word.Id)),
                        j => j.HasKey("WpParentWoID", "WpWoID"));
                e.HasMany(w => w.ChildWords)
                    .WithMany(w => w.ParentWords)
                    .UsingEntity(
                        "wordparents",
                        l => l.HasOne(typeof(Word)).WithMany().HasForeignKey("WpWoID").HasPrincipalKey(nameof(Word.Id)),
                        r => r.HasOne(typeof(Word)).WithMany().HasForeignKey("WpParentWoID").HasPrincipalKey(nameof(Word.Id)),
                        j => j.HasKey("WpWoID", "WpParentWoID"));
                e.HasOne(w => w.Language)
                    .WithMany(l => l.Words)
                    .HasForeignKey(x => x.LanguageId);
                e.HasOne(w => w.Status)
                    .WithMany(s => s.Words)
                    .HasForeignKey(x => x.StatusId);
            });
            modelBuilder.Entity<WordFlashMessage>(e => { });
            modelBuilder.Entity<WordImage>(e => { });
            //modelBuilder.Entity<ParentChildWordRelationship>(e => { });
            modelBuilder.Entity<WordTag>(e => { });
            modelBuilder.Entity<Page>(e =>
            {
                e.HasOne(p => p.Book)
                    .WithMany(b => b.Pages)
                    .HasForeignKey(p => p.BookId);
                e.HasMany(p => p.Sentences)
                    .WithOne(s => s.Text)
                    .HasForeignKey(s => s.PageId);

            });

        }
    }
}
