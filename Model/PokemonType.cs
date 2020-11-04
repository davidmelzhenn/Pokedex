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

namespace Pokedex.Model
{
    public class PokemonType
    {
        public int slot { get; set; }
        public NameAPIResource type { get; set; }
    }
}