using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyTest
{
    class SampleWallChangeSize
    {
        public void ChangeWallType(Document doc)
        {
            List<Wall> ListWalls = GetWalls(doc);
            WallType wallType = null;

            foreach (Wall w in ListWalls)
            {
                try
                {
                    wallType = new FilteredElementCollector(doc)
                        .OfClass(typeof(WallType)).First<Element>(x => x.Name.Equals("SW48")) as WallType;
                }
                catch { }
                if (wallType != null)
                {
                    Transaction t = new Transaction(doc, "Edit Type");
                    t.Start();
                    try
                    {
                        w.WallType = wallType;
                    }
                    catch { }
                    t.Commit();
                }
                if (wallType == null)
                {
                    WallType NewWallType = CreateNewWallType(doc, w);
                    Transaction t = new Transaction(doc, "Set New Type");
                    t.Start();
                    try
                    {
                        w.WallType = NewWallType;
                    }
                    catch { }
                    t.Commit();
                }
            }
        }

        public List<Wall> GetWalls(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> Walls = collector.OfClass(typeof(Wall)).ToElements();
            List<Wall> ListWalls = new List<Wall>();

            foreach (Wall w in Walls)
            {
                ListWalls.Add(w);
            }
            return ListWalls;
        }

        public WallType CreateNewWallType(Document doc, Wall wall)
        {
            WallType wallType = wall.WallType;
            WallType NewWallType = null;

            Transaction t = new Transaction(doc, "Duplicate wall");
            t.Start();
            try 
            {
                NewWallType = wallType.Duplicate("SW48") as WallType;
                CompoundStructure compoundStructure = NewWallType.GetCompoundStructure();
                int layerIndex = compoundStructure.GetFirstCoreLayerIndex();
                IList<CompoundStructureLayer> csLayers = compoundStructure.GetLayers();
                foreach(CompoundStructureLayer csl in csLayers)
                {
                    if(csl.Function.ToString() == "Structure")
                    {
                        compoundStructure.SetLayerWidth(layerIndex, 48/12);
                    }
                    layerIndex++;
                }
                NewWallType.SetCompoundStructure(compoundStructure);
            }
            catch{}
            t.Commit();
            return NewWallType;
        }
    }
}
