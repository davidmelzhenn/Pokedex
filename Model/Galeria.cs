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

namespace Pokedex.Model
{
    public class Galeria
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [Indexed]
        public double Produto { get; set; }
        [Indexed]
        public int Item { get; set; }
        [MaxLength(50)]
        public string Descricao { get; set; }
        [MaxLength(100)]
        public string Arquivo { get; set; }
        public DateTime DataHoraArquivo { get; set; }

    }
    
}