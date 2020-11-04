using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using Square.Picasso;
using static Pokedex.Geral;

namespace Pokedex
{
    class FragDetalhesAC : Android.Support.V4.App.DialogFragment
    {
        private ListView _listView;
        private List<Codigo_Nome> fLista = new List<Codigo_Nome>();
        private GenericAdapter<Codigo_Nome> _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = inflater.Inflate(Resource.Layout.frmFragment, container, false);

            TextView txtNome = view.FindViewById<TextView>(Resource.Id.txtNome);

            _listView = view.FindViewById<ListView>(Resource.Id.listView);

            if (Arguments.GetString("Imagem", "") != "")
            {

                ImageView img = view.FindViewById<ImageView>(Resource.Id.img);
                //img.Click -= Img_Click;
                //img.Click += Img_Click;
                Android.Net.Uri mUri;

                mUri = Android.Net.Uri.Parse(Arguments.GetString("Imagem", ""));

                Picasso.With(this.Activity)
                        .Load(mUri)
                        .NetworkPolicy(NetworkPolicy.NoCache)
                        .Error(Resource.Drawable.ic_block_white_24dp)
                        .Fit()
                        .CenterInside()
                        .Into(img);

            }

            if (Arguments.GetString("Nome", "") != "")
                txtNome.Text = Arguments.GetString("Nome", "");

            fLista = new List<Codigo_Nome>();

            Codigo_Nome item;


            if (Arguments.GetInt("Id", 0) != 0)
                fLista.Add(item = new Codigo_Nome { Codigo = 0, Nome = "Id: " + Arguments.GetInt("Id", 0) });

            if (Arguments.GetInt("Altura", 0) != 0)
                fLista.Add(item = new Codigo_Nome { Codigo = 1, Nome = "Altura: " + (Arguments.GetInt("Altura", 0) * 10) + " cm" }); // 1 decimetro = 10 centimetros

            if (Arguments.GetInt("Peso", 0) != 0)
                fLista.Add(item = new Codigo_Nome { Codigo = 2, Nome = "Peso: " + (Arguments.GetInt("Peso", 0) / 10) + " kg" }); // 1 hectograma = 0,1 quilograma

            if (Arguments.GetString("Tipo", "") != "")
                fLista.Add(item = new Codigo_Nome { Codigo = 3, Nome = "Tipos: " + Arguments.GetString("Tipo", "") }); // 1 hectograma = 0,1 quilograma

            _adapter = new GenericAdapter<Codigo_Nome>(fLista,
            (convertView) =>
            {
                LayoutInflater mInflater = (LayoutInflater)this.Activity.GetSystemService("layout_inflater");


                return mInflater.Inflate(Resource.Layout.layFragment, null);

            },

            (convertView, parent, _item) =>
            {

                ImageView img = convertView.FindViewById<ImageView>(Resource.Id.img);

                switch (_item.Codigo)
                {
                    case 0:
                        img.SetImageResource(Resource.Drawable.ic_edit_white_24dp);
                        break;
                    case 1:
                        img.SetImageResource(Resource.Drawable.baseline_height_white_24);
                        break;
                    case 2:
                        img.SetImageResource(Resource.Drawable.ic_package_variant_closed_white_24dp);
                        break;
                    case 3:
                        img.SetImageResource(Resource.Drawable.ic_list_white_24dp);
                        break;
                }

                TextView txtDescricao = convertView.FindViewById<TextView>(Resource.Id.txtDescricao);
                txtDescricao.SetText(_item.Nome.ToString(), TextView.BufferType.Normal);

                return convertView;

            });

            _listView.Adapter = _adapter;

            return view;
        }

    }


}