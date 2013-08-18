using System;
using Android.App;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.OS;

namespace com.refractored.xamdroid.stickylistheaders.sample
{
    [Activity(Label = "XamDroid.StickyListHeaders.Sample", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : FragmentActivity, ViewPager.IOnPageChangeListener 
    {
        private ViewPager m_Pager;
#if __ANDROID_11__
        private ActionBar.ITabListener tabChangeListener;
        public class MyListener : ActionBar.ITabListener
        {
            private ViewPager m_Pager;

            private IntPtr m_IntPtr = new IntPtr();
            public MyListener(ViewPager viewPager)
            {
                m_Pager = viewPager;
            }
            public void OnTabReselected(ActionBar.Tab tab, Android.App.FragmentTransaction ft)
            {
            }

            public void OnTabSelected(ActionBar.Tab tab, Android.App.FragmentTransaction ft)
            {
               m_Pager.SetCurrentItem(tab.Position, true);
            }

            public void OnTabUnselected(ActionBar.Tab tab, Android.App.FragmentTransaction ft)
            {
            }

            public System.IntPtr Handle
            {
                get { return m_IntPtr; }
            }

            public void Dispose()
            {
            }
        }
#endif

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            m_Pager = FindViewById<ViewPager>(Resource.Id.pager);
            m_Pager.Adapter = new MainPagerAdapter(SupportFragmentManager);


#if __ANDROID_11__
            tabChangeListener = new MyListener(m_Pager);
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            ActionBar.AddTab(ActionBar.NewTab().SetText("1").SetTabListener(tabChangeListener));
            ActionBar.AddTab(ActionBar.NewTab().SetText("2").SetTabListener(tabChangeListener));
            ActionBar.AddTab(ActionBar.NewTab().SetText("3").SetTabListener(tabChangeListener));
            ActionBar.AddTab(ActionBar.NewTab().SetText("4").SetTabListener(tabChangeListener));
#endif

        }

        public void OnPageScrollStateChanged(int state)
        {
            
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            
        }

        public void OnPageSelected(int position)
        {

#if __ANDROID_11__
            ActionBar.SetSelectedNavigationItem(position);
#endif
        }
    }
}

