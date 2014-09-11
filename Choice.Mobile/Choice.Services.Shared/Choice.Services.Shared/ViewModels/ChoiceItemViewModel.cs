using System;
using System.Collections.Generic;
using System.Text;
using Choice.Services.Shared.Models;

namespace Choice.Services.Shared.ViewModels
{
    public class ChoiceItemViewModel
    {
        public string Name { get; set; }
        public string Tags { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string OptionId1 { get; set; }
        public string OptionId2 { get; set; }
        public int TotalVotes { get; set; }
        public int Option1Votes { get; set; }
        public int Option2Votes { get; set; }
        public string SelectedOptionId { get; set; }

        public int Option1Percentage
        {
            get
            {
                return (int)Math.Truncate(((decimal)Option1Votes / (decimal)TotalVotes) * 100);
            }
        }
        public int Option2Percentage
        {
            get
            {
                return (int)Math.Truncate(((decimal)Option2Votes / (decimal)TotalVotes) * 100);
            }
        }
        public ChoiceItemViewModel() { }
        public ChoiceItemViewModel(ChoiceItem choice)
        {
            Name = choice.Name;
            Tags = choice.Tags;
            ImageUrl1 = choice.Option1.ImageUrl;
            ImageUrl2 = choice.Option2.ImageUrl;
            OptionId1 = choice.Option1.OptionId.ToString();
            OptionId2 = choice.Option2.OptionId.ToString();
            Option1Votes = 56;
            Option2Votes = 60;
            TotalVotes = Option1Votes + Option2Votes;


        }

    }
}
