using System;
using System.IO;
using Xamarin.Forms;
using NxApp.Models;
using NxApp.ModelDbContexts;

namespace NxApp
{
    public partial class NoteEntryPage : ContentPage
    {
        private readonly NoteDbContext.Helper<NoteDbContext> NoteHelper = new NoteDbContext.Helper<NoteDbContext>();

        public NoteEntryPage()
        {
            InitializeComponent();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var note = (Note)BindingContext;

            if (string.IsNullOrWhiteSpace(note.Filename))
            {
                // Save
                note.Filename = Path.GetRandomFileName();
                note.Date = DateTime.Now;
                await NoteHelper.AddOrUpdateNotesAsync(note, true);
            }
            else
            {
                // Update
                await NoteHelper.AddOrUpdateNotesAsync(note, false);
            }

            await Navigation.PopAsync();
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var note = (Note)BindingContext;

            if (File.Exists(note.Filename))
            {
                File.Delete(note.Filename);
            }

            await Navigation.PopAsync();
        }
    }
}
