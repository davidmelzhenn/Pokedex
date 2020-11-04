using System;
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
using static Pokedex.Geral;
using Android.Graphics;
using Android.Content.Res;
using System.Threading;
using Square.Picasso;
using Java.IO;
using Android.Support.Design.Widget;
using System.Threading.Tasks;
using Java.Net;
using Android.Support.V4.Widget;
using Android.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Pokedex.Model;

namespace Pokedex
{
    [Activity(Theme = "@style/MyTheme.Azul", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class FrmListaPokedexAC : AppCompatActivity
    {
        Android.Support.V7.Widget.Toolbar toolbar;

        private GenericAdapter<ListaLocal> _adapter;
        private ListView _listView;
        private List<ListaLocal> fLista = new List<ListaLocal>();
        private TextView empty;
        private FragDetalhesAC fragment;
        private BottomNavigationView navigationView;
        private string fAnt = "", fNext = "";

        protected override async void OnCreate(Bundle bundle)
        {
            if (glThema > 0) SetTheme(glThema);

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.frmListaPokedex);

            _listView = FindViewById<ListView>(Resource.Id.listView);
            _listView.ItemClick += _listView_ItemClick;
            _listView.ChoiceMode = ChoiceMode.Single;

            empty = FindViewById<TextView>(Resource.Id.empty);
            empty.Text = "Nenhum pokemon encontrado";
            empty.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.ic_magnify_close_grey600_24dp, 0, 0);

            navigationView = FindViewById<BottomNavigationView>(Resource.Id.navigation_view);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            navigationView.InflateMenu(Resource.Menu.BottomListaPokedex);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Lista de Pokemons";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            _adapter = new GenericAdapter<ListaLocal>(fLista,
            (convertView) =>
            {
                LayoutInflater mInflater = (LayoutInflater)GetSystemService(Context.LayoutInflaterService);
                return mInflater.Inflate(Resource.Layout.layListaPokedex, null);
            },

            (convertView, parent, _item) =>
            {

                if (_item.Image != null)
                {
                    ImageView img = convertView.FindViewById<ImageView>(Resource.Id.img);
                    img.Click -= Img_Click;
                    img.Click += Img_Click;
                    img.Tag = parent.Tag;
                    Android.Net.Uri mUri;

                    mUri = Android.Net.Uri.Parse(_item.Image);

                    Picasso.With(this)
                            .Load(mUri)
                            .NetworkPolicy(NetworkPolicy.NoCache)
                            .Error(Resource.Drawable.ic_block_white_24dp)
                            .Fit()
                            .CenterInside()
                            .Into(img);
                }

                TextView txtNome = convertView.FindViewById<TextView>(Resource.Id.txtNome);
                txtNome.SetText(_item.Name, TextView.BufferType.Normal);

                return convertView;

            });

            _listView.Adapter = _adapter;

            await RequestPokemons();
            //Filtrar();
        }

        private async void NavigationView_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            if (e.Item.ItemId == Resource.Id.btnAnterior)
            {
                if (fAnt != "")
                    await RequestPokemons(fAnt);
            }
            else if (e.Item.ItemId == Resource.Id.btnProximo)
            {
                if (fNext != "")
                    await RequestPokemons(fNext);
            }
        }

        private async Task RequestPokemons(string byUrl = "")
        {
            //await Task.Delay(5000);

            ProgressDialog pbar = new ProgressDialog(this);

            pbar.SetCancelable(false);
            pbar.SetMessage("Buscando Informações...");
            pbar.SetProgressStyle(ProgressDialogStyle.Horizontal);
            pbar.Indeterminate = true;
            pbar.SetProgressStyle(ProgressDialogStyle.Spinner);
            pbar.Show();

            //new Thread(new ThreadStart(async delegate
            //{

                try
                {
                    fLista.Clear();
                    Filtrar();

                    await Task.Delay(3000);

                    using (var client = new HttpClient())
                    {

                        // envia a requisição GET
                        //var response = await client.GetStringAsync("https://pokeapi.co/api/v2/pokemon/").ConfigureAwait(true);
                        string response;

                        if (byUrl == "")
                            response = await client.GetStringAsync("https://pokeapi.co/api/v2/pokemon/?limit=25").ConfigureAwait(true);
                        else
                            response = await client.GetStringAsync(byUrl).ConfigureAwait(true);

                        // processa a resposta
                        GetPokemon getPokemon = JsonConvert.DeserializeObject<GetPokemon>(response);

                        fAnt = "";
                        fNext = "";

                        if (!string.IsNullOrEmpty(getPokemon.Previous))
                            fAnt = getPokemon.Previous;

                        if (!string.IsNullOrEmpty(getPokemon.Next))
                            fNext = getPokemon.Next;


                        fLista = new List<ListaLocal>();
                        foreach (var result in getPokemon.Results)
                        {
                            ListaLocal lista = new ListaLocal();

                            lista.Name = result.name;

                            response = await client.GetStringAsync("http://pokeapi.co/api/v1/pokemon/" + result.name + "/").ConfigureAwait(true);
                            GetPokemonItem getPokemonItem = JsonConvert.DeserializeObject<GetPokemonItem>(response);
                            if (getPokemonItem != null)
                            {
                                lista.Image = getPokemonItem.Sprites.front_default;
                                lista.Id = getPokemonItem.Id;
                                lista.Height = getPokemonItem.Height;
                                lista.Weight = getPokemonItem.Weight;

                                foreach (var tipo in getPokemonItem.Types)
                                {
                                    List<PokemonType> pokemonTypes = new List<PokemonType>();

                                    pokemonTypes.Add(new PokemonType
                                    {
                                        slot = tipo.slot,
                                        type = new NameAPIResource { name = tipo.type.name, url = tipo.type.url }
                                    });

                                    lista.Types = pokemonTypes;
                                }
                            }


                            fLista.Add(lista);
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                    Thread.Sleep(1000);
                    this.RunOnUiThread(() =>
                    {
                        this.Window.AddFlags(WindowManagerFlags.NotTouchable | WindowManagerFlags.NotTouchable);
                    });
                }

                RunOnUiThread(() =>
                {
                    Filtrar();
                    pbar.Cancel();
                    return;
                });

            //})).Start();

        }
       
        private void Img_Click(object sender, EventArgs e)
        {
            int position = (int)((ImageView)sender).Tag;
            CarregarGaleria(fLista[position].Id, fLista[position].Name);
        }

        private void CarregarGaleria(int byId, string byName)
        {
            var _frmGaleriaGrupoAC = new Intent(this, typeof(FrmGaleriaGrupoAC));
            _frmGaleriaGrupoAC.PutExtra("Codigo", byId.ToString());
            _frmGaleriaGrupoAC.PutExtra("Nome", byName.ToString());
            StartActivity(_frmGaleriaGrupoAC);
        }

        private void Filtrar()
        {
            _adapter._Items = null;
            _listView.Adapter = null;

            _adapter.FiltrarAdapter(fLista);
            _listView.Adapter = _adapter;

            if (fLista.Count == 0)
            {
                empty.Visibility = ViewStates.Visible;
                _listView.Visibility = ViewStates.Gone;
            }
            else
            {
                empty.Visibility = ViewStates.Gone;
                _listView.Visibility = ViewStates.Visible;
            }
        }

        private void _listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            if (e.Position >= 0)
            {

                Bundle mybundle = new Bundle();

                mybundle.PutString("Imagem", fLista[e.Position].Image);
                mybundle.PutString("Nome", fLista[e.Position].Name);
                mybundle.PutInt("Id", fLista[e.Position].Id);
                mybundle.PutInt("Altura", fLista[e.Position].Height);
                mybundle.PutInt("Peso", fLista[e.Position].Weight);

                string mStr = "";
                if (fLista[e.Position].Types != null)
                {
                    foreach (var item in fLista[e.Position].Types)
                    {
                        mStr = item.type.name + ", ";
                    }
                    mStr = mStr.Substring(0, mStr.Length - 2);
                }

                mybundle.PutString("Tipo", mStr.ToString());

                fragment = new FragDetalhesAC
                {
                    Arguments = mybundle
                };

                fragment.Show(SupportFragmentManager, "Tag");
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //criar filtro aqui
            MenuInflater.Inflate(Resource.Menu.Filtrar, menu);
            //nao implementado
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                OnBackPressed();

            return base.OnOptionsItemSelected(item);
        }

        private class ListaLocal
        {
            public string Name { get; set; }
            public string Image { get; set; }
            public int Id { get; set; }
            public int Height { get; set; }
            public int Weight { get; set; }
            public List<PokemonType> Types { get; set; }
        }

    }


}