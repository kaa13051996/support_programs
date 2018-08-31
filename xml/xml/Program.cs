using System;
using System.Linq;
using System.Xml.Linq;

namespace xml
{
    class Program
    {
        private static string fileName = "settings.xml";
        private static XDocument doc = XDocument.Load(fileName);

        static void Main(string[] args)
        {
            Console.WriteLine(@"Выберите операцию:
                                1-создать;
                                2-читать;
                                3-добавить;
                                4-удалить;
                                5-изменить.");
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1)
            {
                //Создание вложенными конструкторами нового документа.
                CreateXML();
            }
            else if (choice == 2)
            {
                //Читать документ.
                ReadXML();
            }
            else if (choice == 3)
            {
                //Добавить сайт в документ.
                AddXML();
            }
            else if (choice == 4)
            {
                //Удалить запись из документа.
                RemoveXML();
            }
            else
            {
                //Изменить параметр частоты.
                UpdateXML();
            }
        }

        static void CreateXML()
        {
            XDocument xdoc = new XDocument(
                new XElement("settings",
                    new XElement("parameters",
                        new XElement("parametr",
                            new XAttribute("name", "update_frequency"),
                            new XElement("value", "12"))),
                    new XElement("channels",
                        new XElement("channel",
                            new XAttribute("name", "Habbr"),
                            new XElement("link", "https://habr.com/rss/interesting/")),
                        new XElement("channel",
                            new XAttribute("name", "Keddrcom"),
                            new XElement("link", "https://keddr.com/feed/")))));
            //Cохраняем документ.
            xdoc.Save("settings.xml");
        }

        static void ReadXML()
        {            
            //Разбивает на parametrs и channels.                      
            foreach (XElement el in doc.Root.Elements())
            {
                Console.WriteLine("{0}", el.Name);
                //Разбивает на parametr и channel.
                foreach (XElement element in el.Elements())
                {
                    Console.WriteLine("\r\t{0}", element.Name);
                    foreach (XAttribute cell in element.Attributes())
                    {
                        Console.WriteLine("\r\t\t{0}: {1}", cell.Name, cell.Value);
                    };
                    foreach (XElement cell in element.Elements())
                    {
                        Console.WriteLine("\r\t\t{0}: {1}", cell.Name, cell.Value);
                    };
                };
            }
        }

        static void AddXML()
        {
            Console.Write("Введите имя RSS-канала: ");            
            string channel = Console.ReadLine();
            Console.Write("\r\nВведите ссылку: ");
            string link = Console.ReadLine();
            doc.Element("settings").Element("channels").AddFirst(
                new XElement("channel",
                    new XAttribute("name", channel),
                    new XElement("link", link)));
                               
            doc.Save("settings.xml");
            ReadXML();
        }

        static void RemoveXML()
        {            
            Console.Write("Введите имя RSS-канала для удаления: ");
            string channel = Console.ReadLine();
            foreach (XElement ell in doc.Element("settings").Element("channels").Elements("channel").ToList())
            {
                if (ell.Attribute("name").Value == channel)
                {
                    ell.Remove();
                }
            }
            doc.Save("settings.xml");
            ReadXML();
        }

        static void UpdateXML()
        {
            Console.Write("Введите новое число:");
            int number = Convert.ToInt32(Console.ReadLine());
            foreach (XElement el in doc.Root.Elements("parameters").Elements("parametr"))
            {
                if (el.FirstAttribute.Value == "update_frequency") el.Value = number.ToString();
            }
            doc.Save(fileName);
            ReadXML();
        }
    }
}
