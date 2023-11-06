using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace NoteApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const string filePath = "notes.json";
            List<Note> ListOfNotes;
            
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                ListOfNotes = JsonConvert.DeserializeObject<List<Note>>(json);
            }
            else
            {
                ListOfNotes = new List<Note>();
            }
            

            while (true)
            {
                Console.WriteLine("0-exit 1-new note, number of note to mark it as completed");
                
                // print all notes
                var i = 2;
                foreach (var note in ListOfNotes) 
                {
                    Console.WriteLine($"({i})\tCreation date:{note.CreationDate.ToString("MM/dd/yyyy")}\n\tDue date: {note.DueDate.ToString("MM/dd/yyyy")}\n\tNote: {note.Text}");
                    i++;
                }
                
                
                int.TryParse(Console.ReadLine(), out var choice);

                switch (choice)
                {
                    case 1:
                        ListOfNotes.Add(Utils.CreateNote());
                        break;
                    case 0:
                        break;
                    default:
                        ListOfNotes.RemoveAt(choice-2);
                        break;
                }
                
                // save into a file
                using (var sw = new StreamWriter(filePath))
                {
                    var json = JsonConvert.SerializeObject(ListOfNotes);
                    sw.WriteLine(json);
                }
            }
        }
    }

    public class Note
    {
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Text { get; set; }

        public Note(DateTime dueDate, string text)
        {
            CreationDate = DateTime.Now;
            DueDate = dueDate;
            Text = text;
        }
    }


    public static class Utils
    {
        public static Note CreateNote()
        {
            while (true)
            {
                Console.WriteLine("Due date: (MM/dd/yyyy)");
                var input = Console.ReadLine();

                if (!DateTime.TryParse(input, out var date)) continue;
            
                Console.WriteLine("Text inside of the note: ");
                var text = Console.ReadLine();

                return new Note(date, text);
            }
        } 
    }
}