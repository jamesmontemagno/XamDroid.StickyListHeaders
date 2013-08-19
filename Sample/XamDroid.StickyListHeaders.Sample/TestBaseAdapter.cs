using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;
using com.refractored.components.stickylistheaders;

namespace com.refractored.xamdroid.stickylistheaders.sample
{
    public class HeaderViewHolder : Java.Lang.Object 
    {
        public TextView Text1 { get; set; }
    }

    public class ViewHolder : Java.Lang.Object
    {
        public TextView Text { get; set; }
    }


    public class TestBaseAdapter : BaseAdapter, IStickyListHeadersAdapter, ISectionIndexer
    {
        private string[] m_Countries;
        private int[] m_SectionIndices;
        private char[] m_SectionLetters;
        private LayoutInflater m_Inflater;
        private Context m_Context;
        private Java.Lang.Object[] m_SectionLettersLang;

        public TestBaseAdapter(Context context)
        {
            m_Context = context;
            m_Inflater = LayoutInflater.From(context);
            m_Countries = context.Resources.GetStringArray(Resource.Array.countries);
            m_SectionIndices = GetSectionIndices();
            m_SectionLetters = GetStartingLetters();

        }

        private char[] GetStartingLetters()
        {
            var letters = new char[m_SectionIndices.Length];
            m_SectionLettersLang = new Object[m_SectionIndices.Length];
            for (int i = 0; i < m_SectionIndices.Length; i++)
            {
                letters[i] = m_Countries[m_SectionIndices[i]][0];
                m_SectionLettersLang[i] = letters[i];
            }
            return letters;
        }

        private int[] GetSectionIndices()
        {
            var sectionIndices = new List<int>();
            char lastFirstChar = m_Countries[0][0];
            sectionIndices.Add(0);
            for (int i = 1; i < m_Countries.Length; i++)
            {
                if (m_Countries[i][0] != lastFirstChar)
                {
                    lastFirstChar = m_Countries[i][0];
                    sectionIndices.Add(i);
                }
            }
            int[] sections = new int[sectionIndices.Count];
            for (int i = 0; i < sectionIndices.Count; i++)
            {
                sections[i] = sectionIndices[i];
            }
            return sections;
        }

        public override int Count
        {
            get { return m_Countries.Length; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return m_Countries[position];
        }

        public override long GetItemId(int position)
        {
            return position;//unique
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            if (convertView == null)
            {
                holder = new ViewHolder();
                convertView = m_Inflater.Inflate(Resource.Layout.test_list_item_layout, parent, false);
                holder.Text = convertView.FindViewById<TextView>(Resource.Id.text);
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }
            holder.Text.Text = m_Countries[position];

            return convertView;

        }




        public View GetHeaderView(int position, View convertView, ViewGroup parent)
        {
            HeaderViewHolder holder = null;
            if (convertView == null)
            {
                holder = new HeaderViewHolder();
                convertView = m_Inflater.Inflate(Resource.Layout.header, parent, false);
                holder.Text1 = convertView.FindViewById<TextView>(Resource.Id.text1);
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as HeaderViewHolder;
            }

            var headerChar = m_Countries[position].Substring(0, 1)[0];
            string headerText = headerChar.ToString();
            //Enable if you want to see 2 or 3 lines deep
            /*
            if (headerChar % 2 == 0)
            {
                headerText = headerChar + "\n" + headerChar + "\n" + headerChar;
            }
            else
            {
                headerText = headerChar + "\n" + headerChar;
            }*/
            holder.Text1.Text = headerText;
            return convertView;
        }

        public long GetHeaderId(int position)
        {
            return m_Countries[position].Substring(0, 1)[0];
        }

        public int GetPositionForSection(int section)
        {
            if (section >= m_SectionIndices.Length)
                section = m_SectionIndices.Length - 1;
            else if (section < 0)
                section = 0;

            return m_SectionIndices[section];
        }

        public int GetSectionForPosition(int position)
        {
            for (int i = 0; i < m_SectionIndices.Length; i++)
            {
                if (position < m_SectionIndices[i])
                    return i - 1;
            }
            return m_SectionIndices.Length - 1;
        }

        public Java.Lang.Object[] GetSections()
        {
            return m_SectionLettersLang;
        }

        public void Clear()
        {
            m_SectionIndices = new int[0];
            m_SectionLetters = new char[0];
            m_SectionLettersLang = new Object[0];
            m_Countries = new string[0];
            NotifyDataSetChanged();
        }

        public void Restore()
        {
            m_Countries = m_Context.Resources.GetStringArray(Resource.Array.countries);
            m_SectionIndices = GetSectionIndices();
            m_SectionLetters = GetStartingLetters();
            NotifyDataSetChanged();
        }

        public int GetSectionStart(int itemPosition)
        {
            return GetPositionForSection(GetSectionForPosition(itemPosition));
        }
    }
}