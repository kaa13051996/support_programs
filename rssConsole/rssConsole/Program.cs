using System;
using System.ServiceModel.Syndication;
using System.Xml;

namespace rssConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Rss20FeedFormatter feedFormatter = new Rss20FeedFormatter();
            XmlReader rssReader = XmlReader.Create("https://habr.com/rss/interesting/");
            if (feedFormatter.CanRead(rssReader))
            {
                feedFormatter.ReadFrom(rssReader);
                var DataContext = feedFormatter.Feed;
                var feedContent = feedFormatter.Feed.Items;                
                foreach(var el in feedContent)
                {
                    //Дата публикации.
                    Console.WriteLine("Дата: {0}", el.PublishDate);
                    //Название статьи.
                    Console.WriteLine("Название: {0}", el.Title.Text);
                    //Описание.
                    Console.WriteLine("Описание: {0}", el.Summary.Text);
                    //Ссылка.
                    Console.WriteLine("Ссылка: {0}\r\n", el.Id);
                }
                rssReader.Close();
            }            
        }
    }
}
