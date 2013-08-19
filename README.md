XamDroid.StickyListHeaders
================

Ported and Maintained by:
James Montemagno ([@JamesMontemagno](http://www.twitter.com/jamesmontemagno))

Original StickyListHeaders by:
([Emil Sjölander on GitHub](https://github.com/emilsjolander/StickyListHeaders))

StickyListHeaders is an Android library that makes it easy to integrate section headers in your `ListView`. These section headers stick to the top like in the new People app of Android 4.0 Ice Cream Sandwich. This behavior is also found in lists with sections on iOS devices. This library can also be used for without the sticky functionality if you just want section headers.

StickyListHeaders actively supports android versions 2.3 (gingerbread) and above
It should be compatible with much older versions of android as well but these are not actively tested.




## Getting started

###Installing the library
Install from Component Store or Clone and add project or dll to your solution.


###Code
Adding a StickyListHeadersList view is as easy as adding this code into your axml layout
```xml
<?xml version="1.0" encoding="utf-8"?>
<com.refractored.components.stickylistheaders.StickyListHeadersListView
    android:id="@+id/list"
    android:layout_width="match_parent"
    android:layout_height="match_parent"/>
```

Now in your activities `onCreate()` or your fragments `onCreateView()` you would want to do something like this
```
var stickyList =  FindViewById<StickyListHeadersListView>(R.id.list);
var adapter = new MyAdapter(this);
stickyList.Adapter = adapter;
```

`MyAdapter` in the above example would look something like this if your list was a list of countries where each header was for a letter in the alphabet.
```
public class HeaderViewHolder : Java.Lang.Object 
 {
        public TextView Text1 { get; set; }
    }

    public class ViewHolder : Java.Lang.Object
    {
        public TextView Text { get; set; }
    }


    public class MyAdapter : BaseAdapter, IStickyListHeadersAdapter, ISectionIndexer
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
```

Thats it! look through the api docs below to get know about things to customize and if you have any problems getting started please open an issue as it probably means the getting started guide need some improvement!


## Api

###IStickyListHeadersAdapter
```
public interface IStickyListHeadersAdapter : IListAdapter 
{
    View GetHeaderView(int position, View convertView, ViewGroup parent);
    long GetHeaderId(int position);
}
```
Your adapter must implement this interface to function with `IStickyListHeadersListView`.
`GetHeaderId()` must return a unique integer for every section. A valid implementation for a list with alphabetical sections is the return the char value of the section that `position` is a part of.

`GetHeaderView()` works exactly like `GetView()` in a regular `ListAdapter`.


###StickyListHeadersListView
Headers are sticky by default but that can easily be changed with this setter. There is of course also a matching getter for the sticky property.
```
public bool AreHeadersSticky{get;set;}
```

A IOnHeaderClickListener is the header version of IOnItemClickListener. This is the setter for it and the interface of the listener. The currentlySticky boolean flag indicated if the header that was clicked was sticking to the top at the time it was clicked.
```
public IOnHeaderListClickListener {get;set;}

public interface OnHeaderListClickListener 
{
    public void OnHeaderClick(StickyListHeadersListView l, View header, int itemPosition, long headerId, boolean currentlySticky);
}
```

StickyListHeaders wraps the adapter passed to `Adapter{set;}` is it's own adapter, so `Adapter{get;}` will not return the adapter that `Adapter{set;}` was passed. It is often recomended that you keep a reference to the adapter in your activity/fragment but if this does not fit you there is a method to retrieve the original adapter.
```
public IStickyListHeadersAdapter WrappedAdapter{get;}
```

This is a setter and getter for an internal attribute that controlls if the list should be drawn under the stuck header. The default value is false. If you want to see the list scroll under your header(the header should have a semi-transparent background) you will want to set this attribute to `true`.
```
public bool IsDrawingListUnderStickyHeader {get;set;}
```


## Limitations

There is currently two limitations with this library, they both have to do with what kind of views the header can contain and the both only apply for when sticky header are activated.

The first limitation is that the header can as of now not contain anything that animates, the list will not crash but the animation will just not run as expected while the header is stuck. The other limitation is that it is currently not possible to have interactive elements in the header, Buttons, switches, etc. will only work when the header is not stuck.


## Contributing

Contributions are very welcome and I will contstantly bring in new features from Emil Sjölander Branch. So if you find a big in the library or want a feature and think you can fix it yourself, fork + pull request and i will greatly appreciate it!


## Original License

    Copyright 2013 Emil Sjölander

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.


## My License

   Copyright 2013 James Montemagno

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
