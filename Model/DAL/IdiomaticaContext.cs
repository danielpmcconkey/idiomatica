using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Model.DAL
{
    /*
     * 
     * you are here and are about to go to toronto
     * 
     * you are in the middle of ripping off the band-aid and complete
     * refactoring the database. You've done a few things already. You've
     * already changed the prod DB over to use GUIDs. But your various local
     * DBs are in various states of disarray.
     * 
     * your ultimate goal is to have a database that can be maintained via
     * dotnet migrations (need to test if we can still do that in prod) and
     * have a "build a fresh DB" script that new devs can run from their
     * desktop.
     * 
     * you've started on the script but got distracted with the DB migration
     * stuff. you just made a bunch of fields required and cleaned up a bit of
     * dodgy DB design. that gives you errors you need to fix in code.
     * 
     * order of operations
     *     1. fix the build errors
     *     add unique constraints to the model
     *     2. get this running in a new DB (probably by finishing the fresh DB
     *        script)
     *     3. get all unit tests to pass
     *     4. finish the fresh db if you haven't
     *     5. figure out what to do about prod options are:
     *          a. figure out the deltas and manually apply them (might be 
     *             needed anyway if you can't get migrations to work)
     *          b. rename the existing db and install a new one, then migrate
     *             data
     *     6. update documentation and archi for justin
     * 
     * 
     * */
    public class IdiomaticaContext : DbContext
    {
        #region DBSets

        public DbSet<Book> Books { get; set; }
        public DbSet<BookListRow> BookListRows { get; set; }
        public DbSet<BookStat> BookStats { get; set; }
        public DbSet<BookTag> BookTags { get; set; }
        public DbSet<BookUser> BookUsers { get; set; }
        public DbSet<BookUserStat> BookUserStats { get; set; }
        public DbSet<FlashCard> FlashCards { get; set; }
        public DbSet<FlashCardAttempt> FlashCardAttempts { get; set; }
        public DbSet<FlashCardParagraphTranslationBridge> FlashCardParagraphTranslationBridges { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LanguageUser> LanguageUsers { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageUser> PageUsers { get; set; }
        public DbSet<Paragraph> Paragraphs { get; set; }
        public DbSet<ParagraphTranslation> ParagraphTranslations { get; set; }
        public DbSet<Sentence> Sentences { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBreadCrumb> UserBreadCrumbs { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<Verb> Verbs { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<WordRank> WordRanks { get; set; }
        public DbSet<WordTranslation> WordTranslations { get; set; }
        public DbSet<WordUser> WordUsers { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<IdentityRole> IdentityRoles { get; set; }
        public DbSet<IdentityRoleClaim<string>> IdentityRoleClaims { get; set; }
        public DbSet<IdentityUserClaim<string>> IdentityUserClaims { get; set; }
        public DbSet<IdentityUserLogin<string>> IdentityUserLogins { get; set; }
        public DbSet<IdentityUserRole<string>> IdentityUserRoles { get; set; }
        public DbSet<IdentityUserToken<string>> IdentityUserTokens { get; set; }

        #endregion

        public IdiomaticaContext(DbContextOptions<IdiomaticaContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity<Book>(e => {
                e.HasKey(b => b.UniqueKey);
                e.HasOne(b => b.Language).WithMany(l => l.Books).HasForeignKey(b => b.LanguageKey)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasMany(b => b.BookStats).WithOne(bs => bs.Book).HasForeignKey(bs => bs.BookKey);
                e.HasMany(b => b.Pages).WithOne(p => p.Book).HasForeignKey(p => p.BookKey);
                e.HasMany(b => b.BookUsers).WithOne(bu => bu.Book).HasForeignKey(bu => bu.BookKey);
                e.HasMany(b => b.BookTags).WithOne(bt => bt.Book).HasForeignKey(bt => bt.BookKey);
            });
            modelBuilder.Entity<BookStat>(e => {
                e.HasKey(bs => new { bs.BookKey, bs.Key });
                e.HasOne(bs => bs.Book).WithMany(b => b.BookStats).HasForeignKey(bs => bs.BookKey)
                    .OnDelete(DeleteBehavior.Cascade);
                e.Property(bs => bs.Key).HasConversion<int>();
            });
            modelBuilder.Entity<BookTag>(e => {
                e.HasKey(bt => bt.UniqueKey);
                e.HasOne(bt => bt.Book).WithMany(b => b.BookTags)
                    .HasForeignKey(bt => bt.BookKey).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(bt => bt.User).WithMany(b => b.BookTags)
                    .HasForeignKey(bt => bt.UserKey).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<BookUser>(e => {
                e.HasKey(bu => bu.UniqueKey);
                e.HasOne(bu => bu.Book).WithMany(b => b.BookUsers)
                    .HasForeignKey(bu => bu.BookKey).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(bu => bu.LanguageUser).WithMany(lu => lu.BookUsers)
                    .HasForeignKey(bu => bu.LanguageUserKey).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(bu => bu.PageUsers).WithOne(pu => pu.BookUser)
                    .HasForeignKey(pu => pu.BookUserKey);
                e.HasOne(bu => bu.CurrentPage).WithMany(p => p.BookUsersBookMarks)
                    .HasForeignKey(bu => bu.CurrentPageKey).OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<BookUserStat>(e => {
                e.HasKey(bus => new {bus.BookKey, bus.LanguageUserKey, bus.Key});
                e.HasOne(bus => bus.Book).WithMany(b => b.BookUserStats)
                    .HasForeignKey(bus => bus.BookKey).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(bus => bus.LanguageUser).WithMany(lu => lu.BookUsersStats)
                    .HasForeignKey(bus => bus.LanguageUserKey).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<FlashCard>(e => {
                e.HasKey(fc => fc.UniqueKey);
                e.HasOne(fc => fc.WordUser).WithOne(wu => wu.FlashCard)
                    .HasForeignKey<FlashCard>(fc => fc.WordUserKey).OnDelete(DeleteBehavior.Cascade);  
                e.HasMany(fc => fc.FlashCardParagraphTranslationBridges).WithOne(fcptb => fcptb.FlashCard)
                    .HasForeignKey(fcpbt => fcpbt.FlashCardKey);
                e.HasMany(fc => fc.Attempts).WithOne(fca => fca.FlashCard).HasForeignKey(fca => fca.FlashCardKey);
                e.Property(fc => fc.Status).HasConversion<int>();
            });
            modelBuilder.Entity<FlashCardAttempt>(e => {
                e.HasKey(fca => fca.UniqueKey);
                e.HasOne(fca => fca.FlashCard).WithMany(fc => fc.Attempts)
                    .HasForeignKey(fca => fca.FlashCardKey).OnDelete(DeleteBehavior.Cascade);
                e.Property(fca => fca.Status).HasConversion<int>();
            });
            modelBuilder.Entity<FlashCardParagraphTranslationBridge>(e => {
                e.HasKey(fcptb => fcptb.UniqueKey);
                e.HasOne(fcptb => fcptb.FlashCard).WithMany(fc => fc.FlashCardParagraphTranslationBridges)
                    .HasForeignKey(fcptb => fcptb.FlashCardKey).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(fcptb => fcptb.ParagraphTranslation).WithMany(pt => pt.FlashCardParagraphTranslationBridges)
                    .HasForeignKey(fcptb => fcptb.ParagraphTranslationKey).OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Language>(e => {
                e.HasKey(l => l.UniqueKey);
                e.HasMany(l => l.LanguageUsers).WithOne(lu => lu.Language)
                    .HasForeignKey(lu => lu.LanguageKey);
                e.HasMany(l => l.Books).WithOne(b => b.Language).HasForeignKey(b => b.LanguageKey);
                e.HasMany(l => l.Words).WithOne(w => w.Language).HasForeignKey(w => w.LanguageKey);
                e.HasMany(l => l.WordRanks).WithOne(wr => wr.Language).HasForeignKey(wr => wr.LanguageKey);
                e.HasMany(l => l.ParagraphTranslations).WithOne(pt => pt.Language)
                    .HasForeignKey(pt => pt.LanguageKey);
                e.Property(l => l.Code).HasConversion<int>();
            });
            modelBuilder.Entity<LanguageUser>(e => {
                e.HasKey(lu => lu.UniqueKey);
                e.HasOne(lu => lu.User).WithMany(u => u.LanguageUsers)
                    .HasForeignKey(lu => lu.UserKey).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(lu => lu.Language).WithMany(l => l.LanguageUsers)
                    .HasForeignKey(lu => lu.LanguageKey).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(l => l.BookUsers).WithOne(bu => bu.LanguageUser)
                    .HasForeignKey(bu => bu.LanguageUserKey);
                e.HasMany(l => l.WordUsers).WithOne(w => w.LanguageUser)
                    .HasForeignKey(w => w.LanguageUserKey);
            });
            modelBuilder.Entity<Page>(e => {
                e.HasKey(p => p.UniqueKey);
                e.HasOne(p => p.Book).WithMany(b => b.Pages)
                    .HasForeignKey(p => p.BookKey).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(p => p.Paragraphs).WithOne(pp => pp.Page).HasForeignKey(pp => pp.PageKey);
                e.HasMany(p => p.PageUsers).WithOne(pu => pu.Page).HasForeignKey(pu => pu.PageKey);
                e.HasMany(p => p.UserBreadCrumbs).WithOne(ubc => ubc.Page).HasForeignKey(ubc => ubc.PageKey);
                e.HasMany(p => p.BookUsersBookMarks)
                    .WithOne(bu => bu.CurrentPage)
                    .HasForeignKey(bu => bu.CurrentPageKey);
            });
            modelBuilder.Entity<PageUser>(e => {
                e.HasKey(pu => pu.UniqueKey);
                e.HasOne(pu => pu.BookUser).WithMany(bu => bu.PageUsers)
                    .HasForeignKey(pu => pu.BookUserKey).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(pu => pu.Page).WithMany(p => p.PageUsers)
                    .HasForeignKey(pu => pu.PageKey).OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Paragraph>(e => {
                e.HasKey(pp => pp.UniqueKey);
                e.HasOne(pp => pp.Page).WithMany(p => p.Paragraphs)
                    .HasForeignKey(pp => pp.PageKey).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(pp => pp.Sentences).WithOne(s => s.Paragraph).HasForeignKey(s => s.ParagraphKey);
                e.HasMany(pp => pp.ParagraphTranslations)
                    .WithOne(ppt => ppt.Paragraph)
                    .HasForeignKey(ppt => ppt.ParagraphKey);
            });
            modelBuilder.Entity<ParagraphTranslation>(e =>
            {
                e.HasKey(ppt => ppt.UniqueKey);
                e.HasOne(ppt => ppt.Language)
                    .WithMany(s => s.ParagraphTranslations)
                    .HasForeignKey(ppt => ppt.LanguageKey).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(ppt => ppt.Paragraph)
                    .WithMany(pp => pp.ParagraphTranslations)
                    .HasForeignKey(ppt => ppt.ParagraphKey).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Sentence>(e => {
                e.HasKey(s => s.UniqueKey);
                e.HasOne(s => s.Paragraph).WithMany(pp => pp.Sentences)
                    .HasForeignKey(s => s.ParagraphKey).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(s => s.Tokens).WithOne(t => t.Sentence).HasForeignKey(t => t.SentenceKey);
            });
            modelBuilder.Entity<Token>(e => {
                e.HasKey(t => t.UniqueKey);
                e.HasOne(t => t.Sentence).WithMany(t => t.Tokens)
                    .HasForeignKey(t => t.SentenceKey).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(t => t.Word).WithMany(w => w.Tokens)
                    .HasForeignKey(t => t.WordKey).OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<User>(e => {
                e.HasKey(u => u.UniqueKey);
                e.HasMany(u => u.LanguageUsers).WithOne(lu => lu.User).HasForeignKey(u => u.UserKey);
                e.HasMany(u => u.UserSettings).WithOne(us => us.User).HasForeignKey(us => us.UserKey);
                e.HasMany(u => u.UserBreadCrumbs).WithOne(ubc => ubc.User).HasForeignKey(ubc => ubc.UserKey);
            });
            modelBuilder.Entity<UserBreadCrumb>(e =>
            {
                e.HasKey(ubc => ubc.UniqueKey);
                e.HasOne(ubc => ubc.User).WithMany(u => u.UserBreadCrumbs)
                    .HasForeignKey(ubc => ubc.UserKey).OnDelete(DeleteBehavior.Cascade); ;
                e.HasOne(ubc => ubc.Page).WithMany(p => p.UserBreadCrumbs)
                    .HasForeignKey(ubc => ubc.PageKey).OnDelete(DeleteBehavior.Cascade); ;
            });
            modelBuilder.Entity<UserSetting>(e => {
                e.HasKey(us => new { us.UserKey, us.Key });
                e.HasOne(us => us.User).WithMany(u => u.UserSettings).HasForeignKey(us => us.UserKey)
                    .OnDelete(DeleteBehavior.Cascade);
                e.Property(bs => bs.Key).HasConversion<int>();
            });
            modelBuilder.Entity<Verb>(e => {
                e.HasKey(v => v.UniqueKey);
                e.HasOne(v => v.Language).WithMany(l => l.Verbs)
                    .HasForeignKey(v => v.LanguageKey).OnDelete(DeleteBehavior.Cascade); 
                e.HasMany(v => v.WordTranslations).WithOne(wt => wt.Verb).HasForeignKey(wt => wt.VerbKey);
            });
            modelBuilder.Entity<Word>(e => {
                e.HasKey(w => w.UniqueKey);
                e.HasOne(w => w.Language).WithMany(l => l.Words)
                    .HasForeignKey(w => w.LanguageKey).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(w => w.Tokens).WithOne(t => t.Word).HasForeignKey(t => t.WordKey);
                e.HasMany(w => w.WordUsers).WithOne(wu => wu.Word).HasForeignKey(wu => wu.WordKey);
                e.HasOne(w => w.WordRank).WithOne(wr => wr.Word)
                    .HasForeignKey<WordRank>(wr => wr.WordKey).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<WordRank>(e =>
            {
                e.HasKey(wr => wr.UniqueKey);
                e.HasOne(wr => wr.Language).WithMany(l => l.WordRanks)
                    .HasForeignKey(wr => wr.LanguageKey).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(wr => wr.Word).WithOne(w => w.WordRank)
                    .HasForeignKey<WordRank>(wr => wr.WordKey).OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<WordTranslation>(e => {
                e.HasKey(wt => wt.UniqueKey);
                e.HasOne(wt => wt.LanguageTo).WithMany(l => l.WordTranslations)
                    .HasForeignKey(wt => wt.LanguageToKey).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(wt => wt.Word).WithMany(w => w.WordTranslations)
                    .HasForeignKey(wt => wt.WordKey).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(wt => wt.Verb).WithMany(v => v.WordTranslations)
                    .HasForeignKey(wt => wt.VerbKey).OnDelete(DeleteBehavior.NoAction);
                e.Property(wt => wt.PartOfSpeech).HasConversion<int>();
            });
            modelBuilder.Entity<WordUser>(e => {
                e.HasKey(wu => wu.UniqueKey);
                e.HasOne(wu => wu.LanguageUser).WithMany(lu => lu.WordUsers)
                    .HasForeignKey(w => w.LanguageUserKey).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(wu => wu.Word).WithMany(w => w.WordUsers)
                    .HasForeignKey(wu => wu.WordKey).OnDelete(DeleteBehavior.NoAction);
                e.Property(wu => wu.Status).HasConversion<int>();
            });

            // created by Microsoft.AspNetCore.Identity.EntityFrameworkCore
            modelBuilder.Entity("ApplicationUser", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("nvarchar(450)");

                b.Property<int>("AccessFailedCount")
                    .HasColumnType("int");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Email")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<bool>("EmailConfirmed")
                    .HasColumnType("bit");

                b.Property<bool>("LockoutEnabled")
                    .HasColumnType("bit");

                b.Property<DateTimeOffset?>("LockoutEnd")
                    .HasColumnType("datetimeoffset");

                b.Property<string>("NormalizedEmail")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<string>("NormalizedUserName")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<string>("PasswordHash")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("PhoneNumber")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("PhoneNumberConfirmed")
                    .HasColumnType("bit");

                b.Property<string>("SecurityStamp")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("TwoFactorEnabled")
                    .HasColumnType("bit");

                b.Property<string>("UserName")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                    .HasDatabaseName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasDatabaseName("UserNameIndex")
                    .HasFilter("[NormalizedUserName] IS NOT NULL");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                b.ToTable("AspNetUsers", (string)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<string>("NormalizedName")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasDatabaseName("RoleNameIndex")
                    .HasFilter("[NormalizedName] IS NOT NULL");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                b.ToTable("AspNetRoles", (string)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("ClaimType")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ClaimValue")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("RoleId")
                    .IsRequired()
                    .HasColumnType("nvarchar(450)");

                b.HasKey("Id");

                b.HasIndex("RoleId");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                b.ToTable("AspNetRoleClaims", (string)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("ClaimType")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ClaimValue")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("nvarchar(450)");

                b.HasKey("Id");

                b.HasIndex("UserId");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                b.ToTable("AspNetUserClaims", (string)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.Property<string>("LoginProvider")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("ProviderKey")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("ProviderDisplayName")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("nvarchar(450)");

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                b.ToTable("AspNetUserLogins", (string)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("RoleId")
                    .HasColumnType("nvarchar(450)");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                b.ToTable("AspNetUserRoles", (string)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("LoginProvider")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("Value")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("UserId", "LoginProvider", "Name");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                b.ToTable("AspNetUserTokens", (string)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.HasOne("ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.HasOne("ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.HasOne("ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        }
    }
}
