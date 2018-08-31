using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace testThreads
{
    public class ClassInfo
    {
        private bool _cancelled = false;
        public List<ClassBla> list;
        

        public void Cancel()
        {
            _cancelled = true;
        }

        public void Work()
        {               
            list = new List<ClassBla>();
            int i = 0;
            List<string> sites = new List<string> { "https://habr.com/rss/interesting/", "https://keddr.com/feed/"};
            foreach(string site in sites)
            {
                Rss20FeedFormatter feedFormatter = new Rss20FeedFormatter();
                XmlReader rssReader = XmlReader.Create(site);
                if (feedFormatter.CanRead(rssReader))
                {
                    feedFormatter.ReadFrom(rssReader);
                    var DataContext = feedFormatter.Feed;
                    var feedContent = feedFormatter.Feed.Items;
                    foreach (var el in feedContent)
                    {
                        ClassBla obj = new ClassBla(
                            el.PublishDate.ToString(),
                            el.Id
                        );
                        list.Add(obj);
                    }
                    rssReader.Close();
                }
                ProcessChanged(i+=50);
            }                
            
            WorkCompleted(_cancelled);
            DataSave(true);
        }
        
        public event Action<int> ProcessChanged;
        public event Action<bool> WorkCompleted;
        public event Action<bool> DataSave;
    }
}
