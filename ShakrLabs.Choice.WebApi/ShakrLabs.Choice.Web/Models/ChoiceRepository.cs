using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakrLabs.Choice.Web.Models
{
    public class ChoiceRepository : IChoiceRepository
    {

        public IEnumerable<ChoiceViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public ChoiceViewModel Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public ChoiceViewModel Add(ChoiceViewModel item)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Update(ChoiceViewModel item)
        {
            throw new NotImplementedException();
        }
    }
}