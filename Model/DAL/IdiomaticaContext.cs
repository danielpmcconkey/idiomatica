using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Model.DAL
{
    
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
        public DbSet<WordUserProgressTotal> WordUserProgressTotals { get; set; }
        public DbSet<WordUser> WordUsers { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<IdentityRole> IdentityRoles { get; set; }
        public DbSet<IdentityRoleClaim<string>> IdentityRoleClaims { get; set; }
        public DbSet<IdentityUserClaim<string>> IdentityUserClaims { get; set; }
        public DbSet<IdentityUserLogin<string>> IdentityUserLogins { get; set; }
        public DbSet<IdentityUserRole<string>> IdentityUserRoles { get; set; }
        public DbSet<IdentityUserToken<string>> IdentityUserTokens { get; set; }
        public DbSet<LogMessage> LogMessages { get; set; }

        #endregion

        public IdiomaticaContext(DbContextOptions<IdiomaticaContext> options)
        : base(options)
        {
        }

#if DEBUG

        /*
         * this section is here entirely for debugging when we have
         * DB update issues. we could eventually use it to
         * auto-resolve issues. but, so far, isuses have always 
         * been cases of errors in code logic (I think)
         * */

        public override int SaveChanges()
        {
            var maxRetries = 3;
            var retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    return base.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
 
                    retries++;
                    if (retries >= maxRetries) throw;
                    foreach (var entry in ex.Entries)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            Console.WriteLine($"proposed value: {proposedValue}");
                            if (databaseValues != null)
                            {
                                var databaseValue = databaseValues[property];
                                if (databaseValue is not null)
                                {
                                    Console.WriteLine($"database value: {databaseValue}");
                                }
                            }
                        }
                    }
                }
                catch {
                    throw;
                }
            }
            throw new InvalidDataException("Unknown data issues while saving context");
        }
#endif

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            #region our tables
            modelBuilder.Entity<Book>(e =>
            {
                e.HasKey(b => b.Id);
                e.HasOne(b => b.Language).WithMany(l => l.Books).HasForeignKey(b => b.LanguageId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasMany(b => b.BookStats).WithOne(bs => bs.Book).HasForeignKey(bs => bs.BookId);
                e.HasMany(b => b.Pages).WithOne(p => p.Book).HasForeignKey(p => p.BookId);
                e.HasMany(b => b.BookUsers).WithOne(bu => bu.Book).HasForeignKey(bu => bu.BookId);
                e.HasMany(b => b.BookTags).WithOne(bt => bt.Book).HasForeignKey(bt => bt.BookId);
            });
            modelBuilder.Entity<BookStat>(e =>
            {
                e.HasKey(bs => new { bs.BookId, bs.Key });
                e.HasOne(bs => bs.Book).WithMany(b => b.BookStats).HasForeignKey(bs => bs.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.Property(bs => bs.Key).HasConversion<int>();
            });
            modelBuilder.Entity<BookTag>(e =>
            {
                e.HasKey(bt => bt.Id);
                e.HasOne(bt => bt.Book).WithMany(b => b.BookTags)
                    .HasForeignKey(bt => bt.BookId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(bt => bt.User).WithMany(b => b.BookTags)
                    .HasForeignKey(bt => bt.UserId).OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(bt => new { bt.UserId, bt.BookId, bt.Tag }).IsUnique();
            });
            modelBuilder.Entity<BookUser>(e =>
            {
                e.HasKey(bu => bu.Id);
                e.HasOne(bu => bu.Book).WithMany(b => b.BookUsers)
                    .HasForeignKey(bu => bu.BookId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(bu => bu.LanguageUser).WithMany(lu => lu.BookUsers)
                    .HasForeignKey(bu => bu.LanguageUserId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(bu => bu.PageUsers).WithOne(pu => pu.BookUser)
                    .HasForeignKey(pu => pu.BookUserId);
                e.HasOne(bu => bu.CurrentPage).WithMany(p => p.BookUsersBookMarks)
                    .HasForeignKey(bu => bu.CurrentPageId).OnDelete(DeleteBehavior.NoAction);
                e.HasIndex(bu => new { bu.BookId, bu.LanguageUserId }).IsUnique();
            });
            modelBuilder.Entity<BookUserStat>(e =>
            {
                e.HasKey(bus => new { bus.BookId, bus.LanguageUserId, bus.Key });
                e.HasOne(bus => bus.Book).WithMany(b => b.BookUserStats)
                    .HasForeignKey(bus => bus.BookId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(bus => bus.LanguageUser).WithMany(lu => lu.BookUsersStats)
                    .HasForeignKey(bus => bus.LanguageUserId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<FlashCard>(e =>
            {
                e.HasKey(fc => fc.Id);
                e.HasOne(fc => fc.WordUser).WithOne(wu => wu.FlashCard)
                    .HasForeignKey<FlashCard>(fc => fc.WordUserId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(fc => fc.FlashCardParagraphTranslationBridges).WithOne(fcptb => fcptb.FlashCard)
                    .HasForeignKey(fcpbt => fcpbt.FlashCardId);
                e.HasMany(fc => fc.Attempts).WithOne(fca => fca.FlashCard).HasForeignKey(fca => fca.FlashCardId);
                e.Property(fc => fc.Status).HasConversion<int>();
                e.HasIndex(fc => fc.WordUserId).IsUnique();
            });
            modelBuilder.Entity<FlashCardAttempt>(e =>
            {
                e.HasKey(fca => fca.Id);
                e.HasOne(fca => fca.FlashCard).WithMany(fc => fc.Attempts)
                    .HasForeignKey(fca => fca.FlashCardId).OnDelete(DeleteBehavior.Cascade);
                e.Property(fca => fca.Status).HasConversion<int>();
            });
            modelBuilder.Entity<FlashCardParagraphTranslationBridge>(e =>
            {
                e.HasKey(fcptb => fcptb.Id);
                e.HasOne(fcptb => fcptb.FlashCard).WithMany(fc => fc.FlashCardParagraphTranslationBridges)
                    .HasForeignKey(fcptb => fcptb.FlashCardId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(fcptb => fcptb.ParagraphTranslation).WithMany(pt => pt.FlashCardParagraphTranslationBridges)
                    .HasForeignKey(fcptb => fcptb.ParagraphTranslationId).OnDelete(DeleteBehavior.NoAction);
                e.HasIndex(fcptb => new { fcptb.ParagraphTranslationId, fcptb.FlashCardId }).IsUnique();
            });
            modelBuilder.Entity<Language>(e =>
            {
                e.HasKey(l => l.Id);
                e.HasMany(l => l.LanguageUsers).WithOne(lu => lu.Language)
                    .HasForeignKey(lu => lu.LanguageId);
                e.HasMany(l => l.Books).WithOne(b => b.Language).HasForeignKey(b => b.LanguageId);
                e.HasMany(l => l.Words).WithOne(w => w.Language).HasForeignKey(w => w.LanguageId);
                e.HasMany(l => l.WordRanks).WithOne(wr => wr.Language).HasForeignKey(wr => wr.LanguageId);
                e.HasMany(l => l.ParagraphTranslations).WithOne(pt => pt.Language)
                    .HasForeignKey(pt => pt.LanguageId);
                e.Property(l => l.Code).HasConversion<int>();
                e.HasIndex(l => l.Code).IsUnique();
            });
            modelBuilder.Entity<LanguageUser>(e =>
            {
                e.HasKey(lu => lu.Id);
                e.HasOne(lu => lu.User).WithMany(u => u.LanguageUsers)
                    .HasForeignKey(lu => lu.UserId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(lu => lu.Language).WithMany(l => l.LanguageUsers)
                    .HasForeignKey(lu => lu.LanguageId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(lu => lu.BookUsers).WithOne(bu => bu.LanguageUser)
                    .HasForeignKey(bu => bu.LanguageUserId);
                e.HasMany(lu => lu.WordUsers).WithOne(w => w.LanguageUser)
                    .HasForeignKey(w => w.LanguageUserId);
                e.HasIndex(lu => new { lu.LanguageId, lu.UserId }).IsUnique();
            });
            modelBuilder.Entity<LogMessage>(e =>
            {
                e.HasKey(lm => lm.Id);
                e.Property(lm => lm.MessageType).HasConversion<int>();
            });
            modelBuilder.Entity<Page>(e =>
            {
                e.HasKey(p => p.Id);
                e.HasOne(p => p.Book).WithMany(b => b.Pages)
                    .HasForeignKey(p => p.BookId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(p => p.Paragraphs).WithOne(pp => pp.Page).HasForeignKey(pp => pp.PageId);
                e.HasMany(p => p.PageUsers).WithOne(pu => pu.Page).HasForeignKey(pu => pu.PageId);
                e.HasMany(p => p.UserBreadCrumbs).WithOne(ubc => ubc.Page).HasForeignKey(ubc => ubc.PageId);
                e.HasMany(p => p.BookUsersBookMarks)
                    .WithOne(bu => bu.CurrentPage)
                    .HasForeignKey(bu => bu.CurrentPageId);
                e.HasIndex(p => new { p.BookId, p.Ordinal }).IsUnique();
            });
            modelBuilder.Entity<PageUser>(e =>
            {
                e.HasKey(pu => pu.Id);
                e.HasOne(pu => pu.BookUser).WithMany(bu => bu.PageUsers)
                    .HasForeignKey(pu => pu.BookUserId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(pu => pu.Page).WithMany(p => p.PageUsers)
                    .HasForeignKey(pu => pu.PageId).OnDelete(DeleteBehavior.NoAction);
                e.HasIndex(pu => new { pu.PageId, pu.BookUserId }).IsUnique();
            });
            modelBuilder.Entity<Paragraph>(e =>
            {
                e.HasKey(pp => pp.Id);
                e.HasOne(pp => pp.Page).WithMany(p => p.Paragraphs)
                    .HasForeignKey(pp => pp.PageId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(pp => pp.Sentences).WithOne(s => s.Paragraph).HasForeignKey(s => s.ParagraphId);
                e.HasMany(pp => pp.ParagraphTranslations)
                    .WithOne(ppt => ppt.Paragraph)
                    .HasForeignKey(ppt => ppt.ParagraphId);
                e.HasIndex(pp => new { pp.PageId, pp.Ordinal }).IsUnique();
            });
            modelBuilder.Entity<ParagraphTranslation>(e =>
            {
                e.HasKey(ppt => ppt.Id);
                e.HasOne(ppt => ppt.Language)
                    .WithMany(s => s.ParagraphTranslations)
                    .HasForeignKey(ppt => ppt.LanguageId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(ppt => ppt.Paragraph)
                    .WithMany(pp => pp.ParagraphTranslations)
                    .HasForeignKey(ppt => ppt.ParagraphId).OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(ppt => new { ppt.ParagraphId, ppt.LanguageId }).IsUnique();
            });
            modelBuilder.Entity<Sentence>(e =>
            {
                e.HasKey(s => s.Id);
                e.HasOne(s => s.Paragraph).WithMany(pp => pp.Sentences)
                    .HasForeignKey(s => s.ParagraphId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(s => s.Tokens).WithOne(t => t.Sentence).HasForeignKey(t => t.SentenceId);
                e.HasIndex(s => new { s.ParagraphId, s.Ordinal }).IsUnique();
            });
            modelBuilder.Entity<Token>(e =>
            {
                e.HasKey(t => t.Id);
                e.HasOne(t => t.Sentence).WithMany(t => t.Tokens)
                    .HasForeignKey(t => t.SentenceId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(t => t.Word).WithMany(w => w.Tokens)
                    .HasForeignKey(t => t.WordId).OnDelete(DeleteBehavior.NoAction);
                e.HasIndex(t => new { t.SentenceId, t.Ordinal }).IsUnique();
            });
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.HasMany(u => u.LanguageUsers).WithOne(lu => lu.User).HasForeignKey(u => u.UserId);
                e.HasMany(u => u.UserSettings).WithOne(us => us.User).HasForeignKey(us => us.UserId);
                e.HasMany(u => u.UserBreadCrumbs).WithOne(ubc => ubc.User).HasForeignKey(ubc => ubc.UserId);
                e.HasIndex(u => u.ApplicationUserId).IsUnique();
            });
            modelBuilder.Entity<UserBreadCrumb>(e =>
            {
                e.HasKey(ubc => ubc.Id);
                e.HasOne(ubc => ubc.User).WithMany(u => u.UserBreadCrumbs)
                    .HasForeignKey(ubc => ubc.UserId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(ubc => ubc.Page).WithMany(p => p.UserBreadCrumbs)
                    .HasForeignKey(ubc => ubc.PageId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<UserSetting>(e =>
            {
                e.HasKey(us => new { us.UserId, us.Key });
                e.HasOne(us => us.User).WithMany(u => u.UserSettings).HasForeignKey(us => us.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.Property(bs => bs.Key).HasConversion<int>();
            });
            modelBuilder.Entity<Verb>(e =>
            {
                e.HasKey(v => v.Id);
                e.HasOne(v => v.Language).WithMany(l => l.Verbs)
                    .HasForeignKey(v => v.LanguageId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(v => v.WordTranslations).WithOne(wt => wt.Verb).HasForeignKey(wt => wt.VerbId);
            });
            modelBuilder.Entity<Word>(e =>
            {
                e.HasKey(w => w.Id);
                e.HasOne(w => w.Language).WithMany(l => l.Words)
                    .HasForeignKey(w => w.LanguageId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(w => w.Tokens).WithOne(t => t.Word).HasForeignKey(t => t.WordId);
                e.HasMany(w => w.WordUsers).WithOne(wu => wu.Word).HasForeignKey(wu => wu.WordId);
                e.HasOne(w => w.WordRank).WithOne(wr => wr.Word)
                    .HasForeignKey<WordRank>(wr => wr.WordId).OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(w => new { w.LanguageId, w.TextLowerCase }).IsUnique();
            });
            modelBuilder.Entity<WordRank>(e =>
            {
                e.HasKey(wr => wr.Id);
                e.HasOne(wr => wr.Language).WithMany(l => l.WordRanks)
                    .HasForeignKey(wr => wr.LanguageId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(wr => wr.Word).WithOne(w => w.WordRank)
                    .HasForeignKey<WordRank>(wr => wr.WordId).OnDelete(DeleteBehavior.NoAction);
                e.HasIndex(wr => new { wr.LanguageId, wr.WordId }).IsUnique();
                e.HasIndex(wr => new { wr.LanguageId, wr.Ordinal }).IsUnique();
            });
            modelBuilder.Entity<WordTranslation>(e =>
            {
                e.HasKey(wt => wt.Id);
                e.HasOne(wt => wt.LanguageTo).WithMany(l => l.WordTranslations)
                    .HasForeignKey(wt => wt.LanguageToId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(wt => wt.Word).WithMany(w => w.WordTranslations)
                    .HasForeignKey(wt => wt.WordId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(wt => wt.Verb).WithMany(v => v.WordTranslations)
                    .HasForeignKey(wt => wt.VerbId).OnDelete(DeleteBehavior.NoAction);
                e.Property(wt => wt.PartOfSpeech).HasConversion<int>();
            });
            modelBuilder.Entity<WordUser>(e =>
            {
                e.HasKey(wu => wu.Id);
                e.HasOne(wu => wu.LanguageUser).WithMany(lu => lu.WordUsers)
                    .HasForeignKey(w => w.LanguageUserId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(wu => wu.Word).WithMany(w => w.WordUsers)
                    .HasForeignKey(wu => wu.WordId).OnDelete(DeleteBehavior.NoAction);
                e.Property(wu => wu.Status).HasConversion<int>();
                e.HasIndex(wu => new { wu.WordId, wu.LanguageUserId }).IsUnique();
            });
            modelBuilder.Entity<WordUserProgressTotal>(e => 
            {
                e.HasKey(wu => wu.Id);
                e.HasOne(wu => wu.LanguageUser).WithMany(lu => lu.WordUserProgressTotals)
                    .HasForeignKey(wu => wu.LanguageUserId).OnDelete(DeleteBehavior.Cascade);
                e.Property(wu => wu.Status).HasConversion<int>();
            });
            #endregion

            #region Identity (tables built by MSFT Identity stuff
#pragma warning disable 612, 618, CS8600

            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

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

                b.ToTable("AspNetUsers", (string)null);
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

                b.ToTable("AspNetRoles", (string)null);
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

                b.ToTable("AspNetRoleClaims", (string)null);
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

                b.ToTable("AspNetUserClaims", (string)null);
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

                b.ToTable("AspNetUserLogins", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("RoleId")
                    .HasColumnType("nvarchar(450)");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles", (string)null);
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

                b.ToTable("AspNetUserTokens", (string)null);
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
#pragma warning restore 612, 618, CS8600
            #endregion

           
        }
    }
}
