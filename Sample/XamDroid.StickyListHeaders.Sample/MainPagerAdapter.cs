using Android.Support.V4.App;

namespace com.refractored.xamdroid.stickylistheaders.sample
{
    public class MainPagerAdapter : FragmentPagerAdapter 
    {
        public MainPagerAdapter(FragmentManager fm) : base(fm)
        {
            
        }



        public override Fragment GetItem(int p0)
        {
            return new TestFragment();
        }

        public override int Count
        {
            get { return 4; }
        }
    }
}