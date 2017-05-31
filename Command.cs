#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Linq;
using System.Text;
#endregion

namespace MyTest
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            // Revit application documents. 
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            SampleCollector sc = new SampleCollector();

            List<Wall> ListWalls_Class = sc.GetWalls_Class(doc);
            List<Wall> ListWalls_ClassActiveView = sc.GetWalls_ActiveView(doc);
            List<Wall> ListWalls_Category = sc.GetWalls_Category(doc);
            Element Wall_ByNameLINQ = sc.GetWallByNameLINQ(doc, "SW48");
            Element Wall_ByNameLambda = sc.GetWallByNameLambda(doc, "SW48");

            TaskDialog.Show("Values", "---Walls using Class \n" + SB(ListWalls_Class).ToString()
                + "---Walls using Class Active View \n" + SB(ListWalls_ClassActiveView).ToString()
                + "---Walls using Category \n" + SB(ListWalls_Category).ToString()
                + "\n ---Wall LINQ \n" + Wall_ByNameLINQ.Name + " " + Wall_ByNameLINQ.Id
                + "\n ---Wall lambda \n" + Wall_ByNameLambda.Name + " " + Wall_ByNameLambda.Id);

            return Result.Succeeded;
        }

        public StringBuilder SB(List<Wall> Walls)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Wall w in Walls)
                sb.Append(w.Name + " " + w.Id + "\n");

            return sb;
        }
    }
}
