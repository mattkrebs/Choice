using Choice.Services.Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Choice.Services.Shared.ViewModels
{
    public class ChoiceListViewModel
    {
        public bool IsInitialized { get; set; }
        
        private int pageNumber;
        private int _currentPage;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {

                _currentPage = value;
                if (_currentPage + 1 >= Choices.Count)
                {
                    
                }
            }
        }

        private static ChoiceListViewModel _current;
        public static ChoiceListViewModel Current
        {
            get 
            {
                if (_current == null)
                    _current = new ChoiceListViewModel();

                return _current; 
            }
            set { _current = value; }
        }
        

        public ObservableCollection<ChoiceItemViewModel> Choices { get; set; }


        public ChoiceListViewModel()
        {
            Choices = new ObservableCollection<ChoiceItemViewModel>();
           

            ////Subscibe to insert expenses
            //MessagingCenter.Subscribe<TripExpense>(this, "NewExpense", (expense) =>
            //{
            //    Expenses.Add(expense);
            //});

            ////subscribe to update expenxes
            //MessagingCenter.Subscribe<TripExpense>(this, "UpdateExpense", (expense) =>
            //{
            //    ExecuteUpdateExpense(expense);
            //});


        }

        public async Task GetChoicesAsync()
        {
            
            var choices = await ChoiceServices.Instance.GetChoices();
            foreach (var choice in choices)
                Choices.Add(new ChoiceItemViewModel(choice));
        }


     

    }
}
