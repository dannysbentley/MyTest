using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace MyTest
{
    class SampleDeleteElements
    {
        public void DeleteElement(Document doc)
        {
            Element element = FindElementByName(doc, typeof(Floor), "S24");

            using (Transaction t = new Transaction(doc, "Delete element"))
            {
                t.Start();
                doc.Delete(element.Id);
                t.Commit();
            }
        }

        public void DeleteElements(Document doc)
        {
            List<Wall> walls = GetWalls(doc);
            List<ElementId> idSelection = new List<ElementId>();

            foreach (Wall w in walls)
            {
                Element e = w as Element;

                idSelection.Add(e.Id);
            }

            using (Transaction t = new Transaction(doc, "Delete element"))
            {
                t.Start();
                doc.Delete(idSelection);
                t.Commit();
            }
        }


        public List<Wall> GetWalls(Document doc)
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

        public Element FindElementByName(Document doc, Type targetType, string targetName)
        {
            return new FilteredElementCollector(doc)
                .OfClass(targetType)
                .FirstOrDefault<Element>(e => e.Name.Equals(targetName));
        }
    }
}