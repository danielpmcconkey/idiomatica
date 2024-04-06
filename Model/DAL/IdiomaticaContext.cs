using Microsoft.EntityFrameworkCore;

namespace Model.DAL
{
    public class IdiomaticaContext : DbContext
    {
        #region DBSets

        public DbSet<User> Users { get; set; }
        public DbSet<LanguageUser> LanguageUsers { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Paragraph> Paragraphs { get; set; }
        public DbSet<Sentence> Sentences { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<BookStat> BookStats { get; set; }        
        public DbSet<UserSetting> Settings { get; set; }
        public DbSet<Token> Tokens { get; set; }
        
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=localhost;Database=Idiomatica;Trusted_Connection=True;TrustServerCertificate=true;";
            //var dbPath = @"E:\Lute\backups\lute_backup_2024-03-22_075709.db.gz_2024-03-22_075827.db";
            //dbPath = "C:\\Users\\Dan\\AppData\\Local\\Packages\\PythonSoftwareFoundation.Python.3.12_qbz5n2kfra8p0\\LocalCache\\Local\\Lute3\\Lute3\\lute.db";
            optionsBuilder.UseSqlServer(connectionString);

            // only turn on query logging when debugging
            //optionsBuilder.LogTo(Console.WriteLine);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e => {
                e.HasKey(u => u.Id);
                e.HasMany(u => u.LanguageUsers)
                    .WithOne(lu => lu.User)
                    .HasForeignKey(u => u.UserId);
                e.HasMany(u => u.UserSettings)
                    .WithOne(us => us.User)
                    .HasForeignKey(us => us.UserId);
            });
            modelBuilder.Entity<LanguageUser>(e => {
                e.HasKey(lu => lu.Id);
                e.HasOne(lu => lu.User)
                    .WithMany(u => u.LanguageUsers)
                    .HasForeignKey(lu => lu.UserId);
                e.HasOne(lu => lu.Language)
                    .WithMany(l => l.LanguageUsers)
                    .HasForeignKey(lu => lu.LanguageId);                    
                e.HasMany(l => l.Books)
                    .WithOne(b => b.LanguageUser)
                    .HasForeignKey(b => b.LanguageUserId);
                e.HasMany(l => l.Words)
                    .WithOne(w => w.LanguageUser)
                    .HasForeignKey(w => w.LanguageUserId);
            });
            modelBuilder.Entity<Language>(e => {
                e.HasKey(l => l.Id);
                e.HasMany(l => l.LanguageUsers)
                    .WithOne(lu => lu.Language)
                    .HasForeignKey(lu => lu.LanguageId);
            });
            modelBuilder.Entity<Book>(e => {
                e.HasKey(b => b.Id);
                e.HasOne(b => b.LanguageUser)
                    .WithMany(lu => lu.Books)
                    .HasForeignKey(b => b.LanguageUserId);
                e.HasMany(b => b.BookStats)
                    .WithOne(bs => bs.Book)
                    .HasForeignKey(bs => bs.BookId);
                e.HasMany(b => b.Pages)
                    .WithOne(p => p.Book)
                    .HasForeignKey(p => p.BookId);
            });
            modelBuilder.Entity<Page>(e => {
                e.HasKey(p => p.Id);
                e.HasOne(p => p.Book)
                    .WithMany(b => b.Pages)
                    .HasForeignKey(p => p.BookId);
                e.HasMany(p => p.Paragraphs)
                    .WithOne(pp => pp.Page)
                    .HasForeignKey(pp => pp.PageId);
            });
            modelBuilder.Entity<Paragraph>(e => {
                e.HasKey(pp => pp.Id);
                e.HasOne(pp => pp.Page)
                    .WithMany(p => p.Paragraphs)
                    .HasForeignKey(pp => pp.PageId);
                e.HasMany(pp => pp.Sentences)
                    .WithOne(s => s.Paragraph)
                    .HasForeignKey(s => s.ParagraphId);
            });
            modelBuilder.Entity<Sentence>(e => {
                e.HasKey(s => s.Id);
                e.HasOne(s => s.Paragraph)
                    .WithMany(pp => pp.Sentences)
                    .HasForeignKey(s => s.ParagraphId);
                e.HasMany(s => s.Tokens)
                .WithOne(t => t.Sentence)
                .HasForeignKey(t => t.SentenceId);
            });
            modelBuilder.Entity<Word>(e => {
                e.HasMany(w => w.ParentWords)
                    .WithMany(w => w.ChildWords)
                    .UsingEntity(
                        "WordParent",
                        l => l.HasOne(typeof(Word)).WithMany().HasForeignKey("ParentWordId").HasPrincipalKey(nameof(Word.Id)),
                        r => r.HasOne(typeof(Word)).WithMany().HasForeignKey("Id").HasPrincipalKey(nameof(Word.Id)),
                        j => j.HasKey("ParentWordId", "Id"));
                e.HasMany(w => w.ChildWords)
                    .WithMany(w => w.ParentWords)
                    .UsingEntity(
                        "WordParent",
                        l => l.HasOne(typeof(Word)).WithMany().HasForeignKey("Id").HasPrincipalKey(nameof(Word.Id)),
                        r => r.HasOne(typeof(Word)).WithMany().HasForeignKey("ParentWordId").HasPrincipalKey(nameof(Word.Id)),
                        j => j.HasKey("Id", "ParentWordId"));
                e.HasOne(w => w.LanguageUser)
                    .WithMany(lu => lu.Words)
                    .HasForeignKey(w => w.LanguageUserId);
                e.HasMany(w => w.Tokens)
                    .WithOne(t => t.Word)
                    .HasForeignKey(t => t.WordId)
                    .OnDelete(DeleteBehavior.NoAction);
                e.Property(w => w.Status).HasConversion<int>();
            });
            modelBuilder.Entity<BookStat>(e => {
                e.HasKey(bs => new { bs.BookId, bs.Key });
                e.HasOne(bs => bs.Book)
                    .WithMany(b => b.BookStats)
                    .HasForeignKey(bs => bs.BookId);
                e.Property(bs => bs.Key).HasConversion<int>();
            });
            modelBuilder.Entity<UserSetting>(e => {
                e.HasKey(us => new { us.UserId, us.Key });
                e.HasOne(us => us.User)
                    .WithMany(u => u.UserSettings)
                    .HasForeignKey(us => us.UserId);
            });
            modelBuilder.Entity<Token>(e => {
                e.HasKey(t => t.Id);
                e.HasOne(t => t.Sentence)
                    .WithMany(t => t.Tokens)
                    .HasForeignKey(t => t.SentenceId);
                e.HasOne(t => t.Word)
                    .WithMany(w => w.Tokens)
                    .HasForeignKey(t => t.WordId);
                
            });
        }
    }
}
