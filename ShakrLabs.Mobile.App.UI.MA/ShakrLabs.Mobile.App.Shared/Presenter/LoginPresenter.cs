using System;
using System.Collections.Generic;
using System.Text;

namespace ShakrLabs.Mobile.App.Shared.Presenter
{
    public class LoginPresenter
    {

        private static LoginPresenter _presenter;
        public static LoginPresenter Current
        {
            get
            {
                if (_presenter == null)
                {
                    _presenter = new LoginPresenter();
                }
                return _presenter;
            }
        }

        public bool IsLoggedIn { get; set; }

        public int FacebookId { get; set; }
        public string Location { get; set; }
        public string Gender { get; set; }

    }
}
