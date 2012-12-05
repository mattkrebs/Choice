using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using ShakrLabs.Mobile.App.Data.ViewModels;

namespace ShakrLabs.Mobile.App.Data
{
    public class Database : SQLiteConnection
    {
        public Database(string path)
            : base(path)
        {
            CreateTable<ChoiceViewModel>();
            CreateTable<Category>();

        }

        //public ChoiceViewModel FindChoice(Guid PollId)
        //{
        //    return (from s in Table<ChoiceViewModel>()
        //            where s.PollId == PollId
        //            select s).FirstOrDefault();
        //}
        //public IEnumerable<ChoiceViewModel> GetAllChoices()
        //{
        //    return from s in Table<ChoiceViewModel>()
        //           orderby s.CreatedDate
        //           select s;
        //}

        public void InsertStock(ChoiceViewModel choice)
        {
            //
            // Ensure that there is a valid Stock in the DB
            //
            try
            {
                Insert(choice);
            }
            catch (System.Net.WebException ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}