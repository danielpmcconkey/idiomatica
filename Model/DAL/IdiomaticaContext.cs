using Microsoft.EntityFrameworkCore;

namespace Model.DAL
{
    public class IdiomaticaContext : DbContext
    {
        #region DBSets

        public DbSet<Book> Books { get; set; }
        public DbSet<BookStat> BookStats { get; set; }
        public DbSet<BookUser> BookUsers { get; set; }
        public DbSet<BookUserStat> BookUserStats { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LanguageUser> LanguageUsers { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageUser> PageUsers { get; set; }
        public DbSet<Paragraph> Paragraphs { get; set; }
        public DbSet<Sentence> Sentences { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSetting> Settings { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<WordUser> WordUsers { get; set; }

        #endregion

        public IdiomaticaContext(DbContextOptions<IdiomaticaContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(e => {
                e.HasKey(b => b.Id);
                e.HasOne(b => b.Language).WithMany(l => l.Books).HasForeignKey(b => b.LanguageId);
                e.HasMany(b => b.BookStats).WithOne(bs => bs.Book).HasForeignKey(bs => bs.BookId);
                e.HasMany(b => b.Pages).WithOne(p => p.Book).HasForeignKey(p => p.BookId);
                e.HasMany(b => b.BookUsers).WithOne(bu => bu.Book).HasForeignKey(bu => bu.BookId);
            });
            modelBuilder.Entity<BookStat>(e => {
                e.HasKey(bs => new { bs.BookId, bs.Key });
                e.HasOne(bs => bs.Book).WithMany(b => b.BookStats).HasForeignKey(bs => bs.BookId);
                e.Property(bs => bs.Key).HasConversion<int>();
            });
            modelBuilder.Entity<BookUser>(e => {
                e.HasKey(bu => bu.Id);
                e.HasOne(bu => bu.Book).WithMany(b => b.BookUsers).HasForeignKey(bu => bu.BookId);
                e.HasOne(bu => bu.LanguageUser).WithMany(lu => lu.BookUsers)
                    .HasForeignKey(bu => bu.LanguageUserId);
                e.HasMany(bu => bu.BookUserStats).WithOne(bus => bus.BookUser)
                    .HasForeignKey(bus => bus.BookUserId);
                e.HasMany(bu => bu.PageUsers).WithOne(pu => pu.BookUser)
                    .HasForeignKey(pu => pu.BookUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<BookUserStat>(e => {
                e.HasKey(bus => new { bus.BookUserId, bus.Key });
                e.HasOne(bus => bus.BookUser).WithMany(bu => bu.BookUserStats)
                    .HasForeignKey(bus => bus.BookUserId);
                e.Property(bus => bus.Key).HasConversion<int>();
            });
            modelBuilder.Entity<Language>(e => {
                e.HasKey(l => l.Id);
                e.HasMany(l => l.LanguageUsers).WithOne(lu => lu.Language)
                    .HasForeignKey(lu => lu.LanguageId);
                e.HasMany(l => l.Books).WithOne(b => b.Language).HasForeignKey(b => b.LanguageId);
                e.HasMany(l => l.Words).WithOne(w => w.Language).HasForeignKey(w => w.LanguageId);
            });
            modelBuilder.Entity<LanguageUser>(e => {
                e.HasKey(lu => lu.Id);
                e.HasOne(lu => lu.User).WithMany(u => u.LanguageUsers).HasForeignKey(lu => lu.UserId);
                e.HasOne(lu => lu.Language).WithMany(l => l.LanguageUsers)
                    .HasForeignKey(lu => lu.LanguageId);
                e.HasMany(l => l.BookUsers).WithOne(bu => bu.LanguageUser)
                    .HasForeignKey(bu => bu.LanguageUserId)
                    .OnDelete(DeleteBehavior.NoAction);
                e.HasMany(l => l.WordUsers).WithOne(w => w.LanguageUser)
                    .HasForeignKey(w => w.LanguageUserId);
            });
            modelBuilder.Entity<Page>(e => {
                e.HasKey(p => p.Id);
                e.HasOne(p => p.Book).WithMany(b => b.Pages).HasForeignKey(p => p.BookId);
                e.HasMany(p => p.Paragraphs).WithOne(pp => pp.Page).HasForeignKey(pp => pp.PageId);
                e.HasMany(p => p.PageUsers).WithOne(pu => pu.Page).HasForeignKey(pu => pu.PageId);
            });
            modelBuilder.Entity<PageUser>(e => {
                e.HasKey(pu => pu.Id);
                e.HasOne(pu => pu.BookUser).WithMany(bu => bu.PageUsers).HasForeignKey(pu => pu.BookUserId);
                e.HasOne(pu => pu.Page).WithMany(p => p.PageUsers).HasForeignKey(pu => pu.PageId);
            });
            modelBuilder.Entity<Paragraph>(e => {
                e.HasKey(pp => pp.Id);
                e.HasOne(pp => pp.Page).WithMany(p => p.Paragraphs).HasForeignKey(pp => pp.PageId);
                e.HasMany(pp => pp.Sentences).WithOne(s => s.Paragraph).HasForeignKey(s => s.ParagraphId);
            });
            modelBuilder.Entity<Sentence>(e => {
                e.HasKey(s => s.Id);
                e.HasOne(s => s.Paragraph).WithMany(pp => pp.Sentences).HasForeignKey(s => s.ParagraphId);
                e.HasMany(s => s.Tokens).WithOne(t => t.Sentence).HasForeignKey(t => t.SentenceId);
            });
            modelBuilder.Entity<Token>(e => {
                e.HasKey(t => t.Id);
                e.HasOne(t => t.Sentence).WithMany(t => t.Tokens).HasForeignKey(t => t.SentenceId);
                e.HasOne(t => t.Word).WithMany(w => w.Tokens).HasForeignKey(t => t.WordId);
            });
            modelBuilder.Entity<User>(e => {
                e.HasKey(u => u.Id);
                e.HasMany(u => u.LanguageUsers).WithOne(lu => lu.User).HasForeignKey(u => u.UserId);
                e.HasMany(u => u.UserSettings).WithOne(us => us.User).HasForeignKey(us => us.UserId);
            });
            modelBuilder.Entity<UserSetting>(e => {
                e.HasKey(us => new { us.UserId, us.Key });
                e.HasOne(us => us.User).WithMany(u => u.UserSettings).HasForeignKey(us => us.UserId);
            });
            modelBuilder.Entity<Word>(e => {
                e.HasKey(w => w.Id);
                e.HasOne(w => w.Language).WithMany(l => l.Words).HasForeignKey(w => w.LanguageId);
                e.HasMany(w => w.Tokens).WithOne(t => t.Word).HasForeignKey(t => t.WordId)
                    .OnDelete(DeleteBehavior.NoAction);
                e.HasMany(w => w.WordUsers).WithOne(wu => wu.Word).HasForeignKey(wu => wu.WordId);
            });
            modelBuilder.Entity<WordUser>(e => {
                e.HasKey(wu => wu.Id);
                e.HasOne(wu => wu.LanguageUser).WithMany(lu => lu.WordUsers).HasForeignKey(w => w.LanguageUserId);
                e.HasOne(wu => wu.Word).WithMany(w => w.WordUsers).HasForeignKey(wu => wu.WordId)
                    .OnDelete(DeleteBehavior.NoAction);
                e.Property(wu => wu.Status).HasConversion<int>();
            });
        }
    }
}
