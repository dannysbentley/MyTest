using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace MyTest
{
    class SampleCollector
    {
        //*****************************(GetWalls_Class)*****************************
        public List<Wall> GetWalls_Class(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> Walls = collector.OfClass(typeof(Wall)).ToElements();
            List<Wall> List_Walls = new List<Wall>();
            foreach (Wall w in Walls)
            {
                List_Walls.Add(w);
            }
            return List_Walls;
        }

        //*****************************(GetWalls_ActiveView)*****************************
        public List<Wall> GetWalls_ActiveView(Document doc)
        {
            ICollection<Element> Walls = new FilteredElementCollector(doc, doc.ActiveView.Id).OfClass(typeof(Wall)).ToElements();
            List<Wall> List_Walls = new List<Wall>();

            foreach (Wall w in Walls)
            {
                List_Walls.Add(w);
            }
            return List_Walls;
        }

        //*****************************(GetWalls_Category)*****************************
        public List<Wall> GetWalls_Category(Document doc)
        {
            IList<Element> collector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .WhereElementIsNotElementType().ToElements();

            List<Wall> List_Walls = new List<Wall>();
            foreach (Element e in collector)
            {
                Wall w = e as Wall;
                List_Walls.Add(w);
            }
            return List_Walls;
        }

        //*****************************(GetWallByNameLINQ)*****************************
        public Wall GetWallByNameLINQ(Document doc, string name)
        {
            Wall wall = (from v in new FilteredElementCollector(doc).OfClass(typeof(Wall)).Cast<Wall>()
                         where v.Name == name
                         select v).First();
            return wall;
        }

        //*****************************(GetWallByNameLambda)*****************************
        public Element GetWallByNameLambda(Document doc, string name)
        {
            return new FilteredElementCollector(doc)
              .OfClass(typeof(Wall))
              .FirstOrDefault<Element>(
                e => e.Name.Equals(name));
        }
    }
}
