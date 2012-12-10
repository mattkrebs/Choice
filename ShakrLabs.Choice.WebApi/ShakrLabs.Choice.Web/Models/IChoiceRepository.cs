using ShakrLabs.Choice.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakrLabs.Choice.Data.Models
{
    public interface IChoiceRepository
    {
        IEnumerable<ChoiceViewModel> GetAll();
        ChoiceViewModel Get(Guid id);
        ChoiceViewModel Add(ChoiceViewModel item);
        void Remove(Guid id);
        bool Update(ChoiceViewModel item);
    }
}