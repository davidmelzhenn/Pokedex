using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Support.V4.View;
using Android.Widget;
using Android.Graphics;
using Java.IO;
using Square.Picasso;
using Android.Support.V4.Content;
using Android.Net;
using System.Net.Http;
using Pokedex.Model;
using Newtonsoft.Json;
using SQLite;
using System.Threading.Tasks;

namespace Pokedex
{
    [Activity(Theme = "@style/MyTheme.Azul", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class FrmGaleriaGrupoAC : AppCompatActivity
    {
        int fPosition;
        ListView _listGaleriaGrupo;
        GenericAdapter<GaleriaGrupo> _adapter;

        List<GaleriaGrupo> fGaleriaGrupo;

        string fCodigo = "", fNome = "";

        int fWidth;
        int fHeight;

        protected override async void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            if (bundle != null)
            {
                Toast.MakeText(this, "PROBLEMA: É necessário PERMITIR PROCESSOS EM SEGUNDO PLANO para que o APP não reinicie ao alternar. Opção geralmente desativada por APPs que gerenciam memória. Desative-os.", ToastLength.Long).Show();
                Finish();
                return;
            }


            SetContentView(Resource.Layout.frmGaleriaGrupo);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

            _listGaleriaGrupo = FindViewById<ListView>(Resource.Id.listGaleriaGrupo);
            _listGaleriaGrupo.ChoiceMode = ChoiceMode.Single;
            _listGaleriaGrupo.FastScrollEnabled = true;

            _listGaleriaGrupo.ItemClick += _listGaleriaGrupo_Click;

            Display display = WindowManager.DefaultDisplay;
            Point size = new Point();
            display.GetSize(size);
            fWidth = size.X;
            fHeight = size.Y;

            fCodigo = Intent.GetStringExtra("ID") ?? "ID do Pokemon não disponível";

            fNome = Intent.GetStringExtra("Nome") ?? "Nome do Pokemon não disponível";

            fPosition = -1;

            await RequestPokemons();

            TextView txtViewGrupo = FindViewById<TextView>(Resource.Id.txtViewGrupo);
            txtViewGrupo.SetText(fNome, TextView.BufferType.Normal);

            _adapter = new GenericAdapter<GaleriaGrupo>(fGaleriaGrupo,
            (convertView) =>
            {
                LayoutInflater mInflater = (LayoutInflater)GetSystemService(Context.LayoutInflaterService);
                return mInflater.Inflate(Resource.Layout.layGaleriaGrupo, null);
            },

            (convertView, parent, _item) =>
            {
                Android.Net.Uri mUri;
                ImageView imgViewGrupo = convertView.FindViewById<ImageView>(Resource.Id.imgViewGrupo);

                imgViewGrupo.SetMinimumWidth((int)(fWidth / 1.5));
                imgViewGrupo.SetMinimumHeight((int)(fHeight / 1.5));

                mUri = Android.Net.Uri.Parse(_item.Imagem);

                Picasso.With(this)
                        .Load(mUri)
                        .Placeholder(Resource.Drawable.placeholder)
                        .Error(Resource.Drawable.ic_error_white_24dp)
                        .Fit()
                        .CenterInside()
                        .Tag(this)
                        .Into(imgViewGrupo);

                return convertView;

            });
            _listGaleriaGrupo.Adapter = _adapter;

            SetSupportActionBar(toolbar);

            SupportActionBar.Title = "Galeria";
            SupportActionBar.Subtitle = "Grupo de Imagens";

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
        }

        private async Task RequestPokemons()
        {
            // envia a requisição GET
            var client = new HttpClient();

            // processa a resposta

            fGaleriaGrupo = new List<GaleriaGrupo>();

            var getPokemonItem = JsonConvert.DeserializeObject<GetPokemonItem>(await client.GetStringAsync("http://pokeapi.co/api/v1/pokemon/" + fNome + "/"));
            if (getPokemonItem != null)
            {
                if (!string.IsNullOrEmpty(getPokemonItem.Sprites.front_default))
                {
                    fGaleriaGrupo.Add(new GaleriaGrupo { NomeImg = "front_default", Imagem = getPokemonItem.Sprites.front_default });
                }

                if (!string.IsNullOrEmpty(getPokemonItem.Sprites.front_female))
                {
                    fGaleriaGrupo.Add(new GaleriaGrupo { NomeImg = "front_female", Imagem = getPokemonItem.Sprites.front_female });
                }

                if (!string.IsNullOrEmpty(getPokemonItem.Sprites.front_shiny))
                {
                    fGaleriaGrupo.Add(new GaleriaGrupo { NomeImg = "front_shiny", Imagem = getPokemonItem.Sprites.front_shiny });
                }

                if (!string.IsNullOrEmpty(getPokemonItem.Sprites.front_shiny_female))
                {
                    fGaleriaGrupo.Add(new GaleriaGrupo { NomeImg = "front_shiny_female", Imagem = getPokemonItem.Sprites.front_shiny_female });
                }
            }

        }

        private void _listGaleriaGrupo_Click(object sender, AdapterView.ItemClickEventArgs e)
        {
            fPosition = e.Position;
            string mNomeImg = fGaleriaGrupo[fPosition].NomeImg;
            Android.Net.Uri mUri;

            mUri = Android.Net.Uri.Parse(fGaleriaGrupo[fPosition].Imagem);

            Intent intent = new Intent();
            intent.SetAction(Android.Content.Intent.ActionView);

            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            intent.SetDataAndType(mUri, "image/jpeg");
            StartActivity(intent);
        }


        public class GaleriaGrupo
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }
            public string Imagem { get; set; }
            public string NomeImg { get; set; }
        }

        public override void OnBackPressed()
        {
            System.GC.Collect();
            base.OnBackPressed();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();

            return base.OnOptionsItemSelected(item);
        }

    }
}