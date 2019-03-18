using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.x4ever.Pages;
using Xamarin.Forms;

namespace com.organo.x4ever.Models
{
    public interface IVisitedPage
    {
        short Id { get; set; }
        Page Page { get; set; }
        MenuType MenuType { get; set; }
        bool IsCurrent { get; set; }
    }

    public class VisitedPage : IVisitedPage
    {
        public VisitedPage()
        {
            Id = 0;
            Page = new Page();
            IsCurrent = false;
            MenuType = MenuType.MyProfile;
        }

        public short Id { get; set; }
        public Page Page { get; set; }
        public bool IsCurrent { get; set; }
        public MenuType MenuType { get; set; }
    }

    public class VisitedPages
    {
        private List<VisitedPage> _visitedPageList { get; set; }

        private List<VisitedPage> VisitedPageList
        {
            get { return _visitedPageList; }
            set { _visitedPageList = value; }
        }

        public VisitedPages()
        {
            VisitedPageList = new List<VisitedPage>();
        }

        public void Add(VisitedPage visitedPage)
        {
            var lastPage = GetLast();
            if (lastPage != null && lastPage == visitedPage)
                return;
            UpdateCurrent(false);
            if (visitedPage != null)
            {
                visitedPage.Id = GetIndex();
                VisitedPageList.Add(visitedPage);
            }
        }

        public void Add(Page page, bool current = false)
        {
            var lastPage = GetLast();
            if (lastPage != null && lastPage.Page == page)
                return;
            Add(new VisitedPage()
            {
                Page = page,
                IsCurrent = current
            });
        }

        public void Add(MenuType menuType, bool current = false)
        {
            var lastPage = GetLast();
            if (lastPage != null && lastPage.MenuType == menuType)
                return;
            Add(new VisitedPage()
            {
                MenuType = menuType,
                IsCurrent = current
            });
        }

        private short GetIndex()
        {
            var id = GetLast()?.Id;
            if (id == null)
                id = 0;
            return ((short) (id + 1));
        }

        public List<VisitedPage> Get()
        {
            return VisitedPageList;
        }

        public VisitedPage Get(bool current)
        {
            return VisitedPageList.Find(p => p.IsCurrent == current);
        }

        public VisitedPage Get(Page page)
        {
            return VisitedPageList.Find(p => p.Page == page);
        }

        public VisitedPage Get(MenuType menuType)
        {
            return VisitedPageList.Find(p => p.MenuType == menuType);
        }

        public VisitedPage GetLast()
        {
            return VisitedPageList.LastOrDefault();
        }

        public VisitedPage GetFirst()
        {
            return VisitedPageList.FirstOrDefault();
        }

        public VisitedPage GetPreviousPage()
        {
            var lastPage = GetLast();
            if (lastPage != null)
            {
                Remove(lastPage);
                lastPage = GetLast();
            }
            
            return lastPage;
        }

        public VisitedPage GetPreviousMenuType()
        {
            var lastPage = GetLast();
            if (lastPage != null)
            {
                Remove(lastPage);
                lastPage = GetLast();
            }
            else
                lastPage = new VisitedPage()
                {
                    MenuType = MenuType.Logout,
                    Id = GetIndex(),
                    IsCurrent = true
                };

            return lastPage;
        }

        public void Remove(VisitedPage visitedPage)
        {
            VisitedPageList.Remove(visitedPage);
            var lastPage = GetLast();
            UpdateCurrent(true, lastPage);
        }

        private void UpdateCurrent(bool current, VisitedPage visited = null)
        {
            foreach (var visitedPage in VisitedPageList)
            {
                if (visited == null)
                    visitedPage.IsCurrent = current;
                else if (visited == visitedPage)
                {
                    visitedPage.IsCurrent = current;
                }
            }
        }
    }
}