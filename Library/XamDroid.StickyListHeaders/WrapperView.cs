/*
 * Copyright (C) 2013 @JamesMontemagno http://www.montemagno.com http://www.refractored.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * Converted from: https://github.com/emilsjolander/StickyListHeaders
 */

using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Java.Lang;

namespace com.refractored.components.stickylistheaders
{
    /// <summary>
    /// View that wraps a divider header and a normal list item. THe list view sees this as 1 item
    /// </summary>
    public class WrapperView : ViewGroup
    {
        private View m_Item;
        private Drawable m_Divider;
        private int m_DividerHeight;
        private View m_Header;
        private int m_ItemTop;

        public WrapperView(Context context)
            : base(context)
        {

        }

        public void Update(View item, View header, Drawable divider, int dividerHeight)
        {
            if (item == null)
                throw new NullPointerException("List view item must not be null.");

            //Remove the current item if it isn't the same
            //Incase there is recycling of views
            if (m_Item != item)
            {
                RemoveView(m_Item);
                m_Item = item;
                var parent = item.Parent as ViewGroup;
                if (parent != null && parent != this)
                {
                    parent.RemoveView(item);
                }

                AddView(item);
            }

            //Also try this for the header
            if (m_Header != header)
            {
                if (m_Header != null)
                    RemoveView(m_Header);

                m_Header = header;
                if (header != null)
                    AddView(header);
            }

            if (m_Divider != divider)
            {
                m_Divider = divider;
                m_DividerHeight = dividerHeight;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets if it has a header
        /// </summary>
        public bool HasHeader { get { return m_Header != null; } }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var measuredWidth = MeasureSpec.GetSize(widthMeasureSpec);
            var childWidthMeasureSpec = MeasureSpec.MakeMeasureSpec(measuredWidth, MeasureSpecMode.Exactly);
            var measuredHeight = 0;

            var height = 0;
            //Measer the header or the deivider, when there is a header visible it will act as the divider
            if (m_Header != null)
            {

                if (m_Header.LayoutParameters != null && m_Header.LayoutParameters.Height > 0)
                {
                    height = m_Header.LayoutParameters.Height;
                }
                m_Header.Measure(childWidthMeasureSpec,
                                   MeasureSpec.MakeMeasureSpec(height, MeasureSpecMode.Exactly));

                measuredHeight += m_Header.MeasuredHeight;
            }
            else if (m_Divider != null)
            {
                measuredHeight += m_DividerHeight;
            }

            //Measure the item
            height = 0;
            if (m_Item.LayoutParameters != null && m_Item.LayoutParameters.Height > 0)
            {
                height = m_Item.LayoutParameters.Height;
            }
            m_Item.Measure(childWidthMeasureSpec,
                               MeasureSpec.MakeMeasureSpec(height, MeasureSpecMode.Exactly));

            measuredHeight += m_Item.MeasuredHeight;

            SetMeasuredDimension(measuredWidth, measuredHeight);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            l = 0;
            t = 0;
            r = Width;
            b = Height;

            if (m_Header != null)
            {
                var headerHeight = m_Header.MeasuredHeight;
                m_Header.Layout(l, t, r, headerHeight);
                m_ItemTop = headerHeight;
                m_Item.Layout(l, headerHeight, r, b);
            }
            else if (m_Divider != null)
            {
                m_Divider.SetBounds(l, t, r, m_DividerHeight);
                m_ItemTop = m_DividerHeight;
                m_Item.Layout(l, m_DividerHeight, r, b);
            }
            else
            {
                m_ItemTop = t;
                m_Item.Layout(l, t, r, b);
            }
        }

        protected override void DispatchDraw(Android.Graphics.Canvas canvas)
        {
            base.DispatchDraw(canvas);

            if (m_Header == null && m_Divider != null)
            {
                //Drawable.setbounds does not work on pre honeycomb, so you have to do a little work around
                //for anything pre-HC.
                if ((int) Build.VERSION.SdkInt < 11)
                {
                    canvas.ClipRect(0, 0, Width, m_DividerHeight);
                }
                m_Divider.Draw(canvas);
            }
        }
    }
}