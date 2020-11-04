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
using System.Globalization;
using static Pokedex.Geral;
using Android.Bluetooth;
using Android.Content.Res;
using Android.Graphics;
//using System.Xml.Linq;
using Android.Support.V4.Content;
using System.Xml.Linq;
using Android.Net;

namespace Pokedex

{
   

    public class Codigo_Nome
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
    }

    public class Cor
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
    }


    public static class Geral
    {
        public const string CrLf = "\r\n";
        public const string Lf = "\n";

        public const string Enter = "[Enter]";

        public static SQLiteConnection DBGeral { get; set; }

        public static decimal glTotalRam = 0;
        public static String glTipoItem = "";

        public static object obj;

        public static int glThema = 0;
        public static string glMensagem = "";
        public static string glPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

      


        public static Android.Graphics.Color GenerateRandomColor(Android.Graphics.Color mix)
        {
            Random random = new Random();
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);

            // mix the color
            if (mix != null)
            {
                red = (red + mix.R) / 2;
                green = (green + mix.G) / 2;
                blue = (blue + mix.B) / 2;
            }

            Android.Graphics.Color color = new Android.Graphics.Color(red, green, blue);
            return color;
        }

        public static Android.Graphics.Color LongToRGB(double byLongColorVB6)
        {
            double B, G, R;

            R = byLongColorVB6 % 256;
            G = (byLongColorVB6 / 256) % 256;
            B = (byLongColorVB6 / 256 / 256) % 256;

            return Android.Graphics.Color.Rgb(Convert.ToInt32(Math.Floor(R)), Convert.ToInt32(Math.Floor(G)), Convert.ToInt32(Math.Floor(B)));
        }

      


        public static int Len(string byVal)
        {
            if (byVal != null)
                return byVal.Length;
            else
                return 0;
        }

        public static int Len(double byValue)
        {
            string byVal = Convert.ToString(byValue);

            if (byVal != null)
                return byVal.Length;
            else
                return 0;
        }


        public static string Trim(string byString)
        {
            return byString.Trim();
        }



   
        public static string Mid(string byString, int byInicio, int byTamanho = 0)
        {
            if (byString.Length < byInicio)
            {
                return "";
            }

            if (byTamanho == 0)
                byTamanho = byString.Length;

            if (byTamanho > byString.Length - byInicio + 1)
            {
                byTamanho = byString.Length - byInicio + 1;
            }
            return byString.Substring(byInicio - 1, byTamanho);
        }

      

        public static void CarregarThema(ref List<Cor> fCor)
        {
            fCor = new List<Cor>
            {
                new Cor
                {
                    Id = Resource.Style.MyTheme_Azul,
                    Descricao = "AZUL"
                },

                new Cor
                {
                    Id = Resource.Style.MyTheme_Vermelho,
                    Descricao = "VERMELHO"
                },

                new Cor
                {
                    Id = Resource.Style.MyTheme_Purpura,
                    Descricao = "PÚRPURA"
                },

                new Cor
                {
                    Id = Resource.Style.MyTheme_Pink,
                    Descricao = "PINK"
                },

                new Cor
                {
                    Id = Resource.Style.MyTheme_Verde,
                    Descricao = "VERDE"
                },
                new Cor
                {
                    Id = Resource.Style.MyTheme_VerdeEscuro,
                    Descricao = "VERDE ESCURO"
                },

                new Cor
                {
                    Id = Resource.Style.MyTheme_Laranja,
                    Descricao = "LARANJA"
                },

                new Cor
                {
                    Id = Resource.Style.MyTheme_LaranjaEscuro,
                    Descricao = "LARANJA ESCURO"
                },

                new Cor
                {
                    Id = Resource.Style.MyTheme_Marrom,
                    Descricao = "MARROM"
                },

                new Cor
                {
                    Id = Resource.Style.MyTheme_Cinza,
                    Descricao = "CINZA"
                },

                new Cor
                {
                    Id = Resource.Style.MyTheme_CinzaAzulado,
                    Descricao = "CINZA AZULADO"
                },


                new Cor
                {
                    Id = Resource.Style.MyTheme_AzulPink,
                    Descricao = "AZUL COM PINK"
                },

                new Cor
                {
                    Id = Resource.Style.MyTheme_AzulLaranja,
                    Descricao = "AZUL COM LARANJA"
                }
            };


        }

     
        public static Color RetornaCorCaracteristica(string byCaracteristica, string byTipoCor)
        {
            string mCaracteristica = byCaracteristica;
            int mPosIni = mCaracteristica.IndexOf(byTipoCor);

            mPosIni += 10; // POIS COMEÇA NA POSICAO ZERO, VB6 COMEÇA EM 1; NO PERSONAL TA +9
            int mPosFim = mCaracteristica.IndexOf(")", mPosIni) + 1; // POIS COMEÇA NA POSICAO ZERO, VB6 COMEÇA EM 1

            string mCor = "";
            Color mColor = Color.Black;

            mCor = Mid(mCaracteristica, mPosIni, mPosFim - mPosIni);

            if (mCor.ToUpper() == "VERMELHO")
                mColor = Color.Red;
            else if (mCor.ToUpper() == "AZUL")
                mColor = Color.Blue;
            else if (mCor.ToUpper() == "VERDE")
                mColor = Color.Green;
            else if (mCor.ToUpper() == "AMARELO")
                mColor = Color.Yellow;
            else if (mCor.ToUpper() == "BRANCO")
                mColor = Color.White;
            else if (mCor.ToUpper() == "PRETO")
                mColor = Color.Black;
            else if (mCor.ToUpper() == "CINZA")
                mColor = Color.Gray;
            else
                mColor = Color.ParseColor(mCor);

            return mColor;
        }

    }
}

