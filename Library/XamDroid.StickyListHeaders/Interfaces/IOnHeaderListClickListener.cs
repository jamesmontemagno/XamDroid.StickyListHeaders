using Android.Views;

namespace com.refractored.components.stickylistheaders.Interfaces
{
    public interface IOnHeaderListClickListener
    {
        void OnHeaderClick(StickyListHeadersListView listView, View header, int itemPosition, long headerId,
                           bool currentlySticky);
    }
}