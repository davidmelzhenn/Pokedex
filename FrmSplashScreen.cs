using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace Pokedex
{
    [Activity(Theme = "@style/SplashTheme", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class FrmSplashScreen : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            StartActivity(typeof(FrmListaPokedexAC));
            Finish();

        }
        
  
    }
}

