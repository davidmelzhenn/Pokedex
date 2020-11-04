using System;
using Android.Views;
using Android.Widget;

using System.Collections.Generic;

namespace Pokedex
{

    public class GenericAdapter<T> : BaseAdapter<T>
    {
        public List<T> _Items;
        private Func<View, ViewGroup, T, View> _GetView;
        private Func<View, View> _Inflate;
        public List<T> _originalData;
        public bool Stop { get; set; }
        public GenericAdapter(List<T> items, Func<View, View> inflate, Func<View, ViewGroup, T, View> getView)
        {
            _Items = items;
            _GetView = getView;
            _Inflate = inflate;
            Stop = false;
            //Filter = new ConexaoFilter<T>(this);
        }
        public void FiltrarAdapter(List<T> items)
        {
            _Items = items;
            Stop = false;
            this.NotifyDataSetChanged();
        }


        public override int Count { get { return _Items.Count; } }

        //public Filter Filter { get; private set; }

        public override void NotifyDataSetChanged()
        {
            // If you are using cool stuff like sections
            // remember to update the indices here!
            base.NotifyDataSetChanged();
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override T this[int position]
        {
            get { return _Items[position]; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (Stop)
                return convertView;
            if (convertView == null)
                convertView = _Inflate.Invoke(convertView);
            //convertView.Tag = position;
            parent.Tag = position;

            return _GetView.Invoke(convertView, parent, this[position]);
        }


    }

}