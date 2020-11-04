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
    public class GetPokemonItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ItemSprites Sprites { get; set; }
        public List<PokemonType> Types { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
    }
}