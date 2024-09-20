using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Enums;
using Logic.Services.API;
using Xunit;
using Logic.Telemetry;
using Logic;
using Logic.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using Logic.Conjugator.English;

namespace TestDataPopulator
{
    internal class TestDataPopulator
    {
        private Language? _spanishLanguage;
        private Language? _englishLanguage;
        private User? _user;
        private LanguageUser? _spanishLanguageUser;
        private LanguageUser? _englishLanguageUser;

        internal bool PopulateData()
        {
            try
            {
                LogMessage("Beginning data population");
                if (!BuildLanguages()) return false;
                LogMessage("Languages populated");
                if (!BuildWordsAndWordRanks()) return false;
                LogMessage("Words populated");
                if (!BuildUser()) return false;
                LogMessage("User populated");
                if (!BuildBooks()) return false;
                LogMessage("Books populated");
                if (!BuildParagraphTranslations()) return false;
                LogMessage("Paragraph translations populated");
                if (!BuildFlashCards()) return false;
                LogMessage("Flash cards populated");
                LogMessage("About to build verb conjugations...this may take a while.");
                if (!BuildVerbs()) return false;
                LogMessage("Verbs populated");
                if (!AugmentWordUsers()) return false;
                LogMessage("Word users augmented");
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private ServiceProvider GetServiceProvider()
        {
            var connectionstring = "Server=localhost;Database=IdiomaticaFresh;Trusted_Connection=True;TrustServerCertificate=true;";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddScoped<AuthenticationStateProvider, RevalidatingProviderForUnitTesting>();
            services.AddScoped<LoginService>();
            services.AddDbContextFactory<IdiomaticaContext>(options => {
                options.UseSqlServer(connectionstring, b => b.MigrationsAssembly("TestDataPopulator"));
            });
            return services.BuildServiceProvider();
        }
        /// <summary>
        /// The GetRequiredService method was taken from this guide
        /// https://gkama.medium.com/dependency-injection-di-in-net-core-and-net-5-c-unit-tests-935651a99a2d .
        /// This is copied from the test project. So centralize it someday?
        /// </summary>
        public T GetRequiredService<T>() where T : notnull
        {
            var provider = GetServiceProvider();
            var service = provider.GetRequiredService<T>();
            if (service is null) { ErrorHandler.LogAndThrow(); }
#pragma warning disable CS8603 // Possible null reference return.
            return service;
#pragma warning restore CS8603 // Possible null reference return.
        }
        
        internal void LogMessage(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message}");
        }
        private bool BuildWordsAndWordRanks()
        {
            if (_spanishLanguage is null) throw new InvalidDataException();
            if (_englishLanguage is null) throw new InvalidDataException();

            var dbContextFactory = GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            var spanishLanguage = context.Languages.Where(x => x.Id == _spanishLanguage.Id).FirstOrDefault();
            var englishLanguage = context.Languages.Where(x => x.Id == _englishLanguage.Id).FirstOrDefault();
            foreach (var word in WordRanksToPopulate.words)
            {
                var wordKey = Guid.NewGuid();
                var language = _spanishLanguage;
                
                if (word.language == "English") language = _englishLanguage;
                var modelWord = new Word()
                {
                    Id = wordKey,
                    LanguageId = language.Id,
                    Text = word.word,
                    TextLowerCase = word.word,
                    Romanization = word.word,
                };
                //context.Entry(modelWord.Language).State = EntityState.;
                context.Words.Add(modelWord);

                context.WordRanks.Add(new Model.WordRank() {
                    Id = Guid.NewGuid(),
                    LanguageId = language.Id,
                    WordId = wordKey,
                    DifficultyScore = word.difficulty,
                    Ordinal = word.ordinal
                });
            }
            context.SaveChanges();
            return true;
        }
        private bool BuildLanguages()
        {
            var dbContextFactory = GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            foreach (var l in LanguagesToPopulate.languages)
            {
                var newLang = new Language()
                {
                    Id = Guid.NewGuid(),
                    Name = l.Name,
                    Code = l.Code,
                    ParserType = l.ParserType,
                    IsImplementedForLearning = l.IsImplementedForLearning,
                    IsImplementedForUI = l.IsImplementedForUI,
                    IsImplementedForTranslation = l.IsImplementedForTranslation,
                };
                context.Languages.Add(newLang);
                if (newLang.Code == AvailableLanguageCode.ES) _spanishLanguage = newLang;
                if (newLang.Code == AvailableLanguageCode.EN_US) _englishLanguage = newLang;
            }
            context.SaveChanges();
            return true;
        }
        private bool BuildUser()
        {
            if (_spanishLanguage is null) throw new InvalidDataException();
            if (_englishLanguage is null) throw new InvalidDataException();

            var dbContextFactory = GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            ApplicationUser appUser = new ApplicationUser()
            {
                Id = "603daac1-43a2-436b-b133-28c2a516f9f3",
                AccessFailedCount = 0,
                ConcurrencyStamp = "81e72e98-8119-4edc-a6c0-b56277f875da",
                Email = "testDev@testDev.com",
                EmailConfirmed = true,
                LockoutEnabled = true,
                NormalizedEmail = "TESTDEV@TESTDEV.COM",
                NormalizedUserName = "TESTDEV@TESTDEV.COM",
                PasswordHash = "AQAAAAIAAYagAAAAEBYW72yXuoNDlFhJYzf91DJawcjN28lwGKbD5h+f+Z3qlZIEMQb2cccw2SWHAvLJgA==",
                PhoneNumberConfirmed = true,
                SecurityStamp = "CIU2R3EB5UPPVGZ4BUNADKDJRZ53OVUE",
                TwoFactorEnabled = false,
                UserName = "testDev@testDev.com",
            };
            context.Add(appUser);
            _user = new()
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = appUser.Id,
                Name = "Test / Dev user",
            };
            context.Add(_user);
            UserSetting userSettingUiLang = new()
            {
                Key = AvailableUserSetting.UILANGUAGE,
                UserId = _user.Id,
                //User = _user,
                Value = _englishLanguage.Id.ToString()
            };
            context.Add(userSettingUiLang);
            UserSetting userSettingLearningLang = new()
            {
                Key = AvailableUserSetting.CURRENTLEARNINGLANGUAGE,
                UserId = _user.Id,
                //User = _user,
                Value = _spanishLanguage.Id.ToString()
            };
            context.Add(userSettingLearningLang);
            LanguageUser languageUserSp = new()
            {
                Id = Guid.NewGuid(),
                LanguageId = _spanishLanguage.Id,
                //Language = _spanishLanguage,
                UserId = _user.Id,
                //User = _user,
                TotalWordsRead = 0,
            };
            context.Add(languageUserSp);
            _spanishLanguageUser = languageUserSp;

            LanguageUser languageUserEn = new()
            {
                Id = Guid.NewGuid(),
                LanguageId = _englishLanguage.Id,
                //Language = _englishLanguage,
                UserId = _user.Id,
                //User = _user,
                TotalWordsRead = 0,
            };
            context.Add(languageUserEn);
            _englishLanguageUser = languageUserEn;


            context.SaveChanges();
            return true;
        }
        private bool BuildBooks()
        {
            if (_spanishLanguage is null) throw new InvalidDataException();
            if (_englishLanguage is null) throw new InvalidDataException();
            if (_user is null) throw new InvalidDataException();

            var dbContextFactory = GetRequiredService<IDbContextFactory<IdiomaticaContext>>();

            foreach (var book in BooksToPopulate.books)
            {
                AvailableLanguageCode code = AvailableLanguageCode.ES;
                if (book.language == "English") code = AvailableLanguageCode.EN_US;
                var newBook = OrchestrationApi.OrchestrateBookCreationAndSubProcesses(
                    dbContextFactory, _user.Id, book.title, code, book.Uri, book.text);
                if (newBook is null) throw new Exception("book creation returned null");

                // build the page user for page 1 to pass unit tests
                var firstPage = PageApi.PageReadFirstByBookId(dbContextFactory, newBook.Id);
                if (firstPage is null)
                {
                    ErrorHandler.LogAndThrow();
                    return false;
                }
                PageUserApi.PageUserCreateForPageIdAndUserId(
                    dbContextFactory, firstPage, _user);
            }
            return true;
        }
        private bool BuildVerbs()
        {
            if (_spanishLanguage is null) throw new InvalidDataException();
            if (_englishLanguage is null) throw new InvalidDataException();
            if (_user is null) throw new InvalidDataException();

            var dbContextFactory = GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            for (int i = 0; i < VerbsToPopulate.verbs.Length; i++)
            {
                var v = VerbsToPopulate.verbs[i];

                if (v.sp_Infinitive is null) throw new InvalidDataException();
                if (v.sp_Conjugator is null) throw new InvalidDataException();
                if (v.en_Infinitive is null) throw new InvalidDataException();
                if (v.en_Conjugator is null) throw new InvalidDataException();

                Verb spanishVerb = new() 
                {
                    Id = Guid.NewGuid(),
                    //Language = _spanishLanguage,
                    LanguageId = _spanishLanguage.Id,
                    DisplayName = v.sp_DisplayName,
                    Conjugator = $"Logic.Conjugator.Spanish.{v.sp_Conjugator}",
                    Infinitive = v.sp_Infinitive,
                    Core1 = v.sp_Core1,
                    Core2 = v.sp_Core2,
                    Core3 = v.sp_Core3,
                    Core4 = v.sp_Core4,
                    Gerund = v.sp_Gerund,
                    PastParticiple = v.sp_PastParticiple,
                }; 
                Verb englishVerb = new() 
                {
                    Id = Guid.NewGuid(),
                    //Language = _englishLanguage,
                    LanguageId = _englishLanguage.Id,
                    DisplayName = v.en_DisplayName,
                    Conjugator = v.en_Conjugator,
                    Infinitive = v.en_Infinitive,
                    Core1 = v.en_Core1,
                    Core2 = v.en_Core2,
                    Core3 = v.en_Core3,
                    Core4 = v.en_Core4,
                    Gerund = v.en_Gerund,
                    PastParticiple = v.en_PastParticiple,
                };
                OrchestrateVerbConjugationAndTranslationSpanishToEnglish(
                    dbContextFactory, spanishVerb, englishVerb);

                if ((i + 1) % 20 == 0)
                {
                    LogMessage($"{i + 1} / {VerbsToPopulate.verbs.Length} verbs conjugated, translated, and written to the DB.");
                }
            }
            return true;
        }
        private bool BuildParagraphTranslations()
        {
            if (_spanishLanguage is null) throw new InvalidDataException();
            if (_englishLanguage is null) throw new InvalidDataException();
            if (_spanishLanguageUser is null) throw new InvalidDataException();
            if (_user is null) throw new InvalidDataException();

            //  de, que, en, la
            string pp14590 = "Había una vez una pareja que quería un hijo. La esposa, que estaba esperando, tenía antojo de una planta llamada rapunzel, que crecía en un jardín cercano perteneciente a una hechicera.";
            string pp14590Translation = "Once upon a time there was a couple who wanted a child. The wife, who was expecting, had a craving for a plant called rapunzel, which grew in a nearby garden belonging to a sorceress.";
            //  el, en, la, que
            string pp14591 = "El esposo, queriendo complacer a su esposa, se coló en el jardín para conseguir la rapunzel. Sin embargo, fue descubierto por la hechicera. Ella accedió a dejarlo llevar tanta rapunzel como su esposa quisiera, pero a cambio, debía prometer darle el hijo una vez que naciera.";
            string pp14591Translation = "The husband, wanting to please his wife, sneaked into the garden to get the rapunzel. However, he was discovered by the sorceress. She agreed to let him take as much rapunzel as his wife wanted, but in return, he had to promise to give her the child once it was born.";
            //  la, el, de, en, 
            string pp14592 = "La pareja tuvo una niña, y la hechicera vino a reclamarla. Le puso el nombre de Rapunzel y la encerró en una torre sin puertas. La única entrada era una ventana en la parte superior, y la hechicera la visitaba llamándola: \"Rapunzel, Rapunzel, baja tu cabello\".";
            string pp14592Translation = "The couple had a baby girl, and the sorceress came to claim her. She named her Rapunzel and locked her in a tower with no doors. The only entrance was a window at the top, and the enchantress visited her by calling, \"Rapunzel, Rapunzel, put your hair down.\"";
            //   la, en, el, de
            string pp14593 = "Rapunzel tenía cabello largo y hermoso, y la hechicera lo usaba como una escalera. Un día, un príncipe escuchó a Rapunzel cantar en la torre y quedó cautivado por su voz. Observó cómo la hechicera usaba el cabello de Rapunzel para subir y decidió intentarlo él mismo.";
            string pp14593Translation = "Rapunzel had long, beautiful hair, and the enchantress used it as a ladder. One day, a prince heard Rapunzel singing in the tower and was captivated by her voice. He watched as the enchantress used Rapunzel's hair to climb up and decided to try it himself.";
            //   el, en
            string pp14594 = "El príncipe aprendió las palabras mágicas y visitó a Rapunzel en secreto. Se enamoraron, y él prometió rescatarla. Rapunzel aceptó escapar con él cuando regresara.";
            string pp14594Translation = "The prince learned the magic words and visited Rapunzel in secret. They fell in love, and he promised to rescue her. Rapunzel agreed to run away with him when he returned.";
            //   la, de, en, el
            string pp14595 = "Desafortunadamente, la hechicera descubrió su plan. Enojada, cortó el cabello de Rapunzel y la desterró al desierto. Cuando el príncipe vino a visitarla, la hechicera bajó el cabello cortado de Rapunzel, y él subió. Para su sorpresa, encontró a la hechicera en lugar de Rapunzel.";
            string pp14595Translation = "Unfortunately, the sorceress discovered her plan. Angered, she cut off Rapunzel's hair and banished her to the desert. When the prince came to visit her, the sorceress put Rapunzel's cut hair down, and he went upstairs. To his surprise, he found the enchantress instead of Rapunzel.";
            //   el, de, la, que
            string pp14597 = "Y así, el cuento de Rapunzel nos enseña sobre el amor, la lealtad y el poder de la bondad que triunfa sobre el mal.";
            string pp14597Translation = "And so Rapunzel's tale teaches us about love, loyalty and the power of goodness triumphing over evil.";
            //   la, que, el, de
            string pp14600 = "Un día, un hombre extraño llegó a la ciudad. Este hombre era un mago malo. Dijo que era el tío de Aladino y lo llevó al desierto. Allí, el mago le mostró una cueva mágica.";
            string pp14600Translation = "One day, a strange man came to town. This man was an evil magician. He said he was Aladdin's uncle and took him to the desert. There, the wizard showed him a magic cave.";
            //   en, la, que, el
            string pp14601 = "—Aladino, entra en la cueva y trae la lámpara que está dentro—, dijo el mago.";
            string pp14601Translation = "\"Aladdin, go into the cave and bring the lamp that is inside,\" said the magician.";
            
            var dbContextFactory = GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            Paragraph paragraph14590 = GetParagraph14590(context);            
            context.ParagraphTranslations.Add(new ParagraphTranslation()
            {
                Id = Guid.NewGuid(),
                ParagraphId =     paragraph14590.Id,
                //Paragraph =       paragraph14590,
                TranslationText =        pp14590Translation,
                LanguageId = _englishLanguage.Id,
                //Language = _englishLanguage,
            });

            Paragraph paragraph14591 = GetParagraph14591(context);            
            context.ParagraphTranslations.Add(new ParagraphTranslation()
            {
                Id = Guid.NewGuid(),
                ParagraphId =     paragraph14591.Id,
                //Paragraph =       paragraph14591,
                TranslationText =        pp14591Translation,
                LanguageId = _englishLanguage.Id,
                //Language = _englishLanguage,
            });

            Paragraph paragraph14592 = GetParagraph14592(context);            
            context.ParagraphTranslations.Add(new ParagraphTranslation()
            {
                Id = Guid.NewGuid(),
                ParagraphId =     paragraph14592.Id,
                //Paragraph =       paragraph14592,
                TranslationText =        pp14592Translation,
                LanguageId = _englishLanguage.Id,
                //Language = _englishLanguage,
            });

            Paragraph paragraph14593 = GetParagraph14593(context);            
            context.ParagraphTranslations.Add(new ParagraphTranslation()
            {
                Id = Guid.NewGuid(),
                ParagraphId =     paragraph14593.Id,
                //Paragraph =       paragraph14593,
                TranslationText =        pp14593Translation,
                LanguageId = _englishLanguage.Id,
                //Language = _englishLanguage,
            });

            Paragraph paragraph14594 = GetParagraph14594(context);            
            context.ParagraphTranslations.Add(new ParagraphTranslation()
            {
                Id = Guid.NewGuid(),
                ParagraphId =     paragraph14594.Id,
                //Paragraph =       paragraph14594,
                TranslationText =        pp14594Translation,
                LanguageId = _englishLanguage.Id,
                //Language = _englishLanguage,
            });

            Paragraph paragraph14595 = GetParagraph14595(context);            
            context.ParagraphTranslations.Add(new ParagraphTranslation()
            {
                Id = Guid.NewGuid(),
                ParagraphId =     paragraph14595.Id,
                //Paragraph =       paragraph14595,
                TranslationText =        pp14595Translation,
                LanguageId = _englishLanguage.Id,
                //Language = _englishLanguage,
            });

            Paragraph paragraph14597 = GetParagraph14597(context);            
            context.ParagraphTranslations.Add(new ParagraphTranslation()
            {
                Id = Guid.NewGuid(),
                ParagraphId =     paragraph14597.Id,
                //Paragraph =       paragraph14597,
                TranslationText =        pp14597Translation,
                LanguageId = _englishLanguage.Id,
                //Language = _englishLanguage,
            });

            Paragraph paragraph14600 = GetParagraph14600(context);
            context.ParagraphTranslations.Add(new ParagraphTranslation()
            {
                Id = Guid.NewGuid(),
                ParagraphId =     paragraph14600.Id,
                //Paragraph =       paragraph14600,
                TranslationText =        pp14600Translation,
                LanguageId = _englishLanguage.Id,
                //Language = _englishLanguage,
            });

            Paragraph paragraph14601 = GetParagraph14601(context);            
            context.ParagraphTranslations.Add(new ParagraphTranslation()
            {
                Id = Guid.NewGuid(),
                ParagraphId =     paragraph14601.Id,
                //Paragraph =       paragraph14601,
                TranslationText =        pp14601Translation,
                LanguageId = _englishLanguage.Id,
                //Language = _englishLanguage,
            });

            context.SaveChanges();
            return true;
        }
        private bool BuildFlashCards()
        {
            if (_spanishLanguage is null) throw new InvalidDataException();
            if (_englishLanguage is null) throw new InvalidDataException();
            if (_spanishLanguageUser is null) throw new InvalidDataException();
            if (_user is null) throw new InvalidDataException();

            string[] wordsToUse = ["de", "la", "que", "el", "en"];

            var dbContextFactory = GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();


            foreach (var w in wordsToUse)
            {
                var word = WordApi.WordReadByLanguageIdAndText(dbContextFactory, _spanishLanguage.Id, w);
                if (word is null) throw new InvalidDataException();
                ErrorHandler.LogMessage(
                    AvailableLogMessageTypes.DEBUG, $"About to create a flash card for {word.TextLowerCase}.", dbContextFactory);

                var wordUser = context.WordUsers
                    .Where(x => x.WordId == word.Id && x.LanguageUserId == _spanishLanguageUser.Id)
                    .FirstOrDefault();
                if (wordUser is null) throw new InvalidDataException();


                FlashCardApi.FlashCardCreate(dbContextFactory, wordUser.Id, _englishLanguage.Code);
            }
            return true;
        }
        private bool AugmentWordUsers()
        {
            var dbContextFactory = GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            var wordUser = context.WordUsers
                .Where(x => x.Word != null && 
                    x.Word.TextLowerCase == "niña" &&
                    x.LanguageUser != null &&
                    x.LanguageUser.Language != null &&
                    x.LanguageUser.Language.Code == AvailableLanguageCode.ES
                    )
                .FirstOrDefault();
            if (wordUser is null ) throw new InvalidDataException();
            wordUser.Status = AvailableWordUserStatus.LEARNED;
            wordUser.Translation = "girl";
            context.SaveChanges();
            return true;
        }
        private Book GetBookByTitle(IdiomaticaContext context, string title)
        {
            var book = context.Books
                .Where(x => x.Title == title)
                .Include(x => x.Pages).ThenInclude(x => x.Paragraphs)
                .FirstOrDefault();
            if (book is null) throw new InvalidDataException();
            return book;
        }
        private Book GetRapunzel(IdiomaticaContext context)
        {
            return GetBookByTitle(context, "Rapunzel");
        }
        private Book GetAladdin(IdiomaticaContext context)
        {
            return GetBookByTitle(context, "Aladino y la lámpara mágica");
        }
        private Paragraph GetParagraph14590(IdiomaticaContext context)
        {
            var book = GetRapunzel(context);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            if (page is null) throw new InvalidDataException();
            var pp = page.Paragraphs.Where(x => x.Ordinal == 0).FirstOrDefault();
            if (pp is null) throw new InvalidDataException();
            return pp;
        }
        private Paragraph GetParagraph14591(IdiomaticaContext context)
        {
            var book = GetRapunzel(context);
            if (book is null) throw new InvalidDataException();
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            if (page is null) throw new InvalidDataException();
            var pp = page.Paragraphs.Where(x => x.Ordinal == 1).FirstOrDefault();
            if (pp is null) throw new InvalidDataException();
            return pp;
        }
        private Paragraph GetParagraph14592(IdiomaticaContext context)
        {
            var book = GetRapunzel(context);
            if (book is null) throw new InvalidDataException();
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            if (page is null) throw new InvalidDataException();
            var pp = page.Paragraphs.Where(x => x.Ordinal == 2).FirstOrDefault();
            if (pp is null) throw new InvalidDataException();
            return pp;
        }
        private Paragraph GetParagraph14593(IdiomaticaContext context)
        {
            var book = GetRapunzel(context);
            if (book is null) throw new InvalidDataException();
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            if (page is null) throw new InvalidDataException();
            var pp = page.Paragraphs.Where(x => x.Ordinal == 3).FirstOrDefault();
            if (pp is null) throw new InvalidDataException();
            return pp;
        }
        private Paragraph GetParagraph14594(IdiomaticaContext context)
        {
            var book = GetRapunzel(context);
            if (book is null) throw new InvalidDataException();
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            if (page is null) throw new InvalidDataException();
            var pp = page.Paragraphs.Where(x => x.Ordinal == 4).FirstOrDefault();
            if (pp is null) throw new InvalidDataException();
            return pp;
        }
        private Paragraph GetParagraph14595(IdiomaticaContext context)
        {
            var book = GetRapunzel(context);
            if (book is null) throw new InvalidDataException();
            var page = book.Pages.Where(x => x.Ordinal == 2).FirstOrDefault();
            if (page is null) throw new InvalidDataException();
            var pp = page.Paragraphs.Where(x => x.Ordinal == 0).FirstOrDefault();
            if (pp is null) throw new InvalidDataException();
            return pp;
        }
        private Paragraph GetParagraph14597(IdiomaticaContext context)
        {
            var book = GetRapunzel(context);
            if (book is null) throw new InvalidDataException();
            var page = book.Pages.Where(x => x.Ordinal == 2).FirstOrDefault();
            if (page is null) throw new InvalidDataException();
            var pp = page.Paragraphs.Where(x => x.Ordinal == 2).FirstOrDefault();
            if (pp is null) throw new InvalidDataException();
            return pp;
        }
        private Paragraph GetParagraph14600(IdiomaticaContext context)
        {
            var book = GetAladdin(context);
            if (book is null) throw new InvalidDataException();
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            if (page is null) throw new InvalidDataException();
            var pp = page.Paragraphs.Where(x => x.Ordinal == 2).FirstOrDefault();
            if (pp is null) throw new InvalidDataException();
            return pp;
        }
        private Paragraph GetParagraph14601(IdiomaticaContext context)
        {
            var book = GetAladdin(context);
            if (book is null) throw new InvalidDataException();
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            if (page is null) throw new InvalidDataException();
            var pp = page.Paragraphs.Where(x => x.Ordinal == 3).FirstOrDefault();
            if (pp is null) throw new InvalidDataException();
            return pp;
        }

        private WordTranslation? VerbWordTranslationSave(
            IdiomaticaContext context,
            Verb verb, Language fromLanguage, Language toLanguage, string wordTextLower,
            string translation, int ordinal, bool saveContext = true)
        {
            // look up the existing word
            var word = context.Words
                .Where(x => x.TextLowerCase == wordTextLower &&
                    x.LanguageId == fromLanguage.Id)
                .FirstOrDefault();
            if (word is null)
            {
                // create it
                word = new Word()
                {
                    Id = Guid.NewGuid(),
                    LanguageId = fromLanguage.Id,
                    TextLowerCase = wordTextLower,
                    Romanization = wordTextLower
                };
                context.Words.Add(word);

            }
            if (word is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            WordTranslation? wordTranslation = new()
            {
                Id = Guid.NewGuid(),
                LanguageToId = toLanguage.Id,
                WordId = word.Id,
                PartOfSpeech = AvailablePartOfSpeech.VERB,
                Translation = translation.Trim(),
                VerbId = verb.Id,
                Ordinal = ordinal,
            };
            context.WordTranslations.Add(wordTranslation);
            return wordTranslation;
        }
        public Verb? OrchestrateVerbConjugationAndTranslationSpanishToEnglish(
            IDbContextFactory<IdiomaticaContext> dbContextFactory,
            Verb learningVerb, Verb translationVerb)
        {
            if (learningVerb.Conjugator is null) { ErrorHandler.LogAndThrow(); return null; }
            Logic.Conjugator.English.EnglishVerbTranslator translator = new();
            if (translationVerb.Conjugator == "InvariableVerbTranslator")
            {
                translator = new InvariableVerbTranslator();
            }

            var conjugator = Logic.Conjugator.Factory.Get(
                learningVerb.Conjugator, translator, learningVerb, translationVerb);
            if (conjugator is null) { ErrorHandler.LogAndThrow(); return null; }

            var conjugations = conjugator.Conjugate();
            if (conjugations.Count < 1)
            {
                ErrorHandler.LogAndThrow(); return null;
            }
            var verb = VerbCreateAndSaveTranslations(
                dbContextFactory, learningVerb, translationVerb, conjugations);
            if (verb is null)
            {
                ErrorHandler.LogAndThrow(); return null;
            }
            return verb;
        }

        public Verb? VerbCreateAndSaveTranslations(
            IDbContextFactory<IdiomaticaContext> dbContextFactory,
            Verb learningVerb, Verb translationVerb, List<VerbConjugation> conjugations)
        {
            var englishLang = LanguageApi.LanguageReadByCode(
                dbContextFactory, AvailableLanguageCode.EN_US);
            if (englishLang == null) { ErrorHandler.LogAndThrow(); return null; }
            Guid englishLangId = englishLang.Id;

            var context = dbContextFactory.CreateDbContext();

            context.Verbs.Add(learningVerb);
            context.SaveChanges();

            var languageLearning = LanguageApi.LanguageRead(dbContextFactory, learningVerb.LanguageId);
            if (languageLearning is null) { ErrorHandler.LogAndThrow(); return null; }
            var languageTranslation = LanguageApi.LanguageRead(dbContextFactory, translationVerb.LanguageId);
            if (languageTranslation is null) { ErrorHandler.LogAndThrow(); return null; }

            // save the infinitive translation
            VerbWordTranslationSave(context, learningVerb,
                languageLearning, languageTranslation,
                learningVerb.Infinitive, translationVerb.Infinitive, 1, false);

            if (learningVerb.Gerund is not null && translationVerb.Gerund is not null)
            {
                // save the gerund translation
                var gerundTranslation = translationVerb.Gerund;
                if (translationVerb.LanguageId == englishLangId)
                {
                    gerundTranslation = $"{translationVerb.Gerund}: gerund of {learningVerb.Infinitive}";
                }

                VerbWordTranslationSave(context, learningVerb,
                    languageLearning, languageTranslation,
                    learningVerb.Gerund, gerundTranslation, 100, false);
            }
            if (learningVerb.PastParticiple is not null && translationVerb.PastParticiple is not null)
            {
                // save the past participle translation
                var participleTranslation = translationVerb.PastParticiple;
                if (translationVerb.LanguageId == englishLangId)
                {
                    participleTranslation = $"{translationVerb.PastParticiple}: past participle of {learningVerb.Infinitive}";
                }
                VerbWordTranslationSave(context, learningVerb,
                    languageLearning, languageTranslation,
                    learningVerb.PastParticiple, participleTranslation, 100, false);
            }
            context.SaveChanges();
            // save the conjugation translations
            foreach (var conjugation in conjugations)
            {
                var wordInLearningLang = conjugation.ToString();
                var wordInKnownLang = conjugation.Translation;
                if (string.IsNullOrEmpty(wordInLearningLang))
                { ErrorHandler.LogAndThrow(); return null; }
                if (string.IsNullOrEmpty(wordInKnownLang))
                { ErrorHandler.LogAndThrow(); return null; }
                VerbWordTranslationSave(context, learningVerb,
                    languageLearning, languageTranslation,
                    wordInLearningLang, wordInKnownLang, conjugation.Ordinal, false);
                context.SaveChanges();
            }
            context.SaveChanges();
            return learningVerb;
        }

    }

}
