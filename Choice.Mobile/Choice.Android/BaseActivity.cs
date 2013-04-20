using Android.App;
using Android.OS;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Choice.Android
{
    public class BaseActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here


        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater inflater = MenuInflater;

            inflater.Inflate(Resource.Menu.choice_menu, menu);
            return true;

        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_create:
                    StartActivity(typeof(NewChoiceActivity));
                    break;
                case Resource.Id.menu_list:
                    break;
                default:
                    break;
            }
            return true;
        }
    }
}
