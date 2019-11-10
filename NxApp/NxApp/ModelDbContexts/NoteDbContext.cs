using Microsoft.EntityFrameworkCore;
using NxApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NxApp.ModelDbContexts
{
    class NoteDbContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }

        private const string databaseName = "database.db";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var databasePath = GetDatabasePath();
            // Specify that we will use sqlite and the path of the database here
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }

        private static string GetDatabasePath()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    SQLitePCL.Batteries_V2.Init();
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", databaseName);
                case Device.Android:
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName);
                default:
                    throw new NotImplementedException("Platform not supported");
            }
        }

        public class Helper<T> where T : NoteDbContext
        {
            protected NoteDbContext CreateContext()
            {
                var noteDbContext = (T)Activator.CreateInstance(typeof(T));
                noteDbContext.Database.EnsureCreated();
                noteDbContext.Database.Migrate();
                return noteDbContext;
            }

            public async Task<List<Note>> getNotesAsync()
            {
                using var context = CreateContext();
                return await context.Notes
                                    .AsNoTracking()
                                    .OrderBy(note => note.Date)
                                    .ToListAsync();
            }

            public async Task AddOrUpdateNotesAsync(Note note, bool isNew)
            {
                using var context = CreateContext();
                if (isNew)
                {
                    // add posts that do not exist in the database
                    await context.Notes.AddAsync(note);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
