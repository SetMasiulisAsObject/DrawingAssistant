using SolidWorks.Interop.swconst;
using Xarial.XCad.SolidWorks;
using Xarial.XCad.Base.Enums;
using System.Diagnostics;
using SolidWorks.Interop.sldworks;
using static MMDevelop.DrawingAssistant.Globals;
using System;

namespace MMDevelop.DrawingAssistant
{
    public class GetBoundingBox
    {
        public double Width { get; set; }
        public double Depth { get; set; }
        public double Height { get; set; }
        //public ISwApplication swApplication = Globals.SwApp;
        //var AppTest = SwApplicationFactory.FromPointer(swApplication);

        public GetBoundingBox(ModelDoc2 swModel)

        {

            ///Reference: https://www.codestack.net/solidworks-api/geometry/precise-bounding-box/
            ///  https://www.codestack.net/license/ 
            //PartDoc swPart;
            //swPart = (PartDoc)Application.ActiveModel;
            //var boundingBoxIncreaseCoef = 0.4;
            // double Width = 0;
            //double Length = 0;
            // double Height = 0;

            if (swModel.GetType() == 1 && swModel != null)// 1 - Part
            {
                var swPart = (PartDoc)swModel;
                var vBBox = (double[])swPart.GetPartBox(true);
                Debug.Print("Width: " + (vBBox[3] - vBBox[0]));
                Debug.Print("Depth: " + (vBBox[5] - vBBox[2]));
                Debug.Print("Height: " + (vBBox[4] - vBBox[1]));
                Width = vBBox[3] - vBBox[0]; //Width
                Depth = vBBox[5] - vBBox[2]; // Depth
                Height = vBBox[4] - vBBox[1]; // Height
            }
            else if (swModel.GetType() == 2 && swModel != null) // 2 -Assembly
            {
                var swAss = (AssemblyDoc)swModel;
                var vBBox = (double[])swAss.GetBox((int)swBoundingBoxOptions_e.swBoundingBoxIncludeRefPlanes);
                Debug.Print("Width: " + (vBBox[3] - vBBox[0]));
                Debug.Print("Depth: " + (vBBox[5] - vBBox[2]));
                Debug.Print("Height: " + (vBBox[4] - vBBox[1]));
                Width = vBBox[3] - vBBox[0]; //Width
                Depth = vBBox[5] - vBBox[2]; // Depth
                Height = vBBox[4] - vBBox[1]; // Height

            }
            else
            {
                SwApp.ShowMessageBox("Please open a part document");
                //return Width,Length,Height;
                Debug.Print("No part and no assembly ");

            }
        }
    }
    public class GetViewCoordinates
    {

        public double TopX { get; set; }
        public double TopY { get; set; }
        public double Back1X { get; set; }
        public double Back1Y { get; set; }
        public double LeftX { get; set; }
        public double LeftY { get; set; }
        public double Back2X { get; set; }
        public double Back2Y { get; set; }
        public double RightX { get; set; }
        public double RightY { get; set; }
        public double Back3X { get; set; }
        public double Back3Y { get; set; }
        public double BottomX { get; set; }
        public double BottomY { get; set; }
        public double Back4X { get; set; }
        public double Back4Y { get; set; }
        public double MainX { get; set; }
        public double MainY { get; set; }
        public double Scale { get; set; }

        private readonly SettingsPage Settings = new SettingsPage();

        public GetViewCoordinates(GetBoundingBox boundingBox, double[] templateSize, string mainViewProjectionName, ProjectedViews projectedViews,
            int[] scaleList)
        {

            double width = 0;
            double depth = 0;
            double height = 0;
            double totalWidth = 0;
            //double totalDepth;
            double totalHeight = 0;
            var gapBetweenViews = 0.025; // May it be manual too?
            double sideMargins = (Settings.ViewMargins[0] + Settings.ViewMargins[1]); // Integer to double
            double topBottomMargins = (Settings.ViewMargins[2] + Settings.ViewMargins[3]); // Integer to double
            var placementAreaWidth = templateSize[1] - sideMargins / 1000; // 0 - left ; 1 - right
            var placementAreaHeight = templateSize[2] - topBottomMargins / 1000;// 2 - top ; 3 - bottom
            foreach (var scale in scaleList)
            {
                Scale = scale;
                switch (mainViewProjectionName)
                {
                    case "*Front":
                    case "*Back":
                        width = boundingBox.Width;
                        depth = boundingBox.Depth;
                        height = boundingBox.Height;
                        totalWidth = width;
                        totalHeight = height;
                        break;
                    case "*Left":
                    case "*Right":
                        width = boundingBox.Depth;
                        depth = boundingBox.Width;
                        height = boundingBox.Height;
                        totalWidth = width;
                        totalHeight = height;
                        break;
                    case "*Top":
                    case "*Bottom":
                        width = boundingBox.Width;
                        depth = boundingBox.Height;
                        height = boundingBox.Depth;
                        totalWidth = width;
                        totalHeight = height;
                        break;
                    default:
                        break;
                }
                width = (width / Scale) + gapBetweenViews;
                depth = (depth / Scale) + gapBetweenViews;
                height = (height / Scale) + gapBetweenViews;
                totalWidth /= Scale;
                totalHeight /= Scale;
                MainX = 0; //0 - x coord, 1 - y coord.
                MainY = 0;
                if (projectedViews.top)
                {
                    totalHeight += depth;
                    MainY -= depth / 2;
                    if (projectedViews.back1)
                    {
                        totalHeight += height;
                        MainY -= height / 2;
                    }
                }
                if (projectedViews.left)
                {
                    totalWidth += depth;
                    MainX += depth / 2;
                    if (projectedViews.back2)
                    {
                        totalWidth += width;
                        MainX += width / 2;
                    }
                }
                if (projectedViews.right)
                {
                    totalWidth += depth;
                    MainX -= depth / 2;
                    if (projectedViews.back3)
                    {
                        totalWidth += width;
                        MainX -= width / 2;
                    }
                }
                if (projectedViews.bottom)
                {
                    totalHeight += depth;
                    MainY += depth / 2;
                    if (projectedViews.back4)
                    {
                        totalHeight += height;
                        MainY += height / 2;
                    }
                }
                if (projectedViews._3D1 && !projectedViews.left && !projectedViews.top)
                {
                    totalWidth += depth;
                    MainX += depth / 2;
                    totalHeight += depth;
                    MainY -= depth / 2;
                }
                if (projectedViews._3D2 && !projectedViews.right && !projectedViews.top)
                {
                    totalWidth += depth;
                    MainX -= depth / 2;
                    totalHeight += depth;
                    MainY -= depth / 2;
                }
                if (projectedViews._3D3 && !projectedViews.left && !projectedViews.bottom)
                {
                    totalWidth += depth;
                    MainX += depth / 2;
                    totalHeight += depth;
                    MainY += depth / 2;
                }
                if (projectedViews._3D4 && !projectedViews.right && !projectedViews.bottom)
                {
                    totalWidth += depth;
                    MainX -= depth / 2;
                    totalHeight += depth;
                    MainY += depth / 2;
                }
                //Scale = CalculateScale(totalWidth, totalHeight, templateSize, scaleList);
                ////Center of the taken space of the views on the drawing sheet. Calculated from the left-lower corner.
                //totalWidth /= Scale;
                //totalHeight /= Scale;
                //width /= Scale;
                //depth /= Scale;
                //height /= Scale;
                if (totalWidth < placementAreaWidth && totalHeight < placementAreaHeight)
                {

                    break;
                }
                //scaleCounter++;
            }
            ///// The middle of the drawing placement area 
            var placementAreaWidthCenter = (placementAreaWidth / 2) + ((double)Settings.ViewMargins[0] / 1000); // 0 - left ; 1 - right          
            var placementAreaCenter = (placementAreaHeight / 2) + ((double)Settings.ViewMargins[3] / 1000);  // 2 - top ; 3 - bottom
            MainX = MainX + placementAreaWidthCenter;
            MainY = MainY + placementAreaCenter;
            TopX = MainX;
            TopY = MainY + ((height + depth) / 2);
            Back1X = MainX;
            Back1Y = MainY + depth + height;
            LeftX = MainX - ((depth + width) / 2);
            LeftY = MainY;
            Back2X = MainX - (depth + width);
            Back2Y = MainY;
            RightX = MainX + ((depth + width) / 2);
            RightY = MainY;
            Back3X = MainX + (depth + width);
            Back3Y = MainY;
            BottomX = MainX;
            BottomY = MainY - ((height + depth) / 2);
            Back4X = MainX;
            Back4Y = MainY - (depth + height);
        }

        private static double CalculateScale(double totalWidth, double totalHeight, double[] templateSize, int[] scaleList)
        {
            var Width = totalWidth;
            var Height = totalHeight;
            var tempWidth = templateSize[1] - 0.06; // to have a margin from an edge of the drawing sheet
            var tempHeight = templateSize[2] - 0.06;
            var scaleCounter = 0;
            var found = false;
            while (!found && scaleCounter < scaleList.Length)
            {
                Width = totalWidth / scaleList[scaleCounter];
                Height = totalHeight / scaleList[scaleCounter];
                if (Width < tempWidth && Height < tempHeight)
                {
                    found = true;
                    break;
                }
                scaleCounter++;
            }
            return scaleList[scaleCounter];
        }
    }


    public class DrawingFunction //: SwAddInEx
    {
        //public ISwApplication Application = Globals.SwApp;
        // Other properties, methods, events...
        public int[] scaleList = { 1, 2, 4, 5, 8, 10, 20, 50, 100, 200, 500 }; // Can be selected according user needs 1:2...
                                                                               //public PartDoc SwPart { get; set; }

        //public DrawingFunction(PartDoc swPart)
        //{
        //    SwPart = swPart;
        //}

        //1.Must select drawing template or take pre-selected.+ The add-in must hold a last used template.+
        //2.Get template size+
        //3.Calculate part's an approx bouding box and get each view (front,right..) sizes on the drawing.+
        //4.Get the views that the user wants to place+
        //.Calculate the centers of the views. Recalculate if the user changes options.+
        //5.Place the vies on the drawing.+
        public void CreateDrawing(string drawingTemplatePath, string mainViewProjectionName, ModelDoc2 StaticModel,
            ProjectedViews projectedViews) // , object SelectedViews ///, object inputs
        {
            //ModelDoc2 swModel;
            DrawingDoc swDraw;
            var swActiveDoc = (ModelDoc2)SwApp.Sw.ActiveDoc;
            // var swActiveDoc = Application.ActiveModel;
            // Calculate each view bounding box
            var BoundingBox = new GetBoundingBox(StaticModel);
            //Calculate coordinates for each view
            var vTemplateSizes = (double[])SwApp.Sw.GetTemplateSizes(drawingTemplatePath); //.UnsafeObject.GetTemplateSizes(drawingTemplatePath);
            var Coord = new GetViewCoordinates(BoundingBox, vTemplateSizes, mainViewProjectionName, projectedViews, scaleList);
            //swModel = StaticModel as ModelDoc2;
            var modelPath = StaticModel.GetPathName();

            if (swActiveDoc.GetType() != 3) //swDocDRAWING	- 3
            {

                var swPaperSize = (int)vTemplateSizes[0];
                var swSheetWidth = vTemplateSizes[1];
                var swSheetHeight = vTemplateSizes[2];
                swDraw = (DrawingDoc)SwApp.Sw.NewDocument(drawingTemplatePath, swPaperSize, swSheetWidth, swSheetHeight);
                //swDraw = (DrawingDoc)Application.UnsafeObject.NewDocument(drawingTemplatePath, swPaperSize, swSheetWidth, swSheetHeight);
                CreateViews(swDraw, modelPath, mainViewProjectionName, Coord, projectedViews);
            }
            else if (swActiveDoc.GetType() == 3)
            {
                swDraw = (DrawingDoc)swActiveDoc;
                DeleteAllViews(swDraw);
                CreateViews(swDraw, modelPath, mainViewProjectionName, Coord, projectedViews);
            }

        }
        public void CreateViews(DrawingDoc swDraw, string modelPath, string mainViewProjectionName, GetViewCoordinates Coord, ProjectedViews projectedViews)
        {
            Sheet swSheet;
            ModelDoc2 swModel;
            IView swView;
            swModel = (ModelDoc2)swDraw;
            swSheet = (Sheet)swDraw.GetCurrentSheet();
            SwApp.Sw.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swAutomaticScaling3ViewDrawings, false);
            var success = swSheet.SetScale(1, Coord.Scale, true, true);
            var mainViewPointer = (IView)swDraw.CreateDrawViewFromModelView3(modelPath, mainViewProjectionName, Coord.MainX, Coord.MainY, 0);
            SwApp.Sw.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swAutomaticScaling3ViewDrawings, true);
            // The scale must be changed after the view has been inserted otherwise
            // SOLIDWORKS will choose scale for a perfect fit of the view
            var mainViewDrawingName = (string)mainViewPointer.GetName2();
            bool selected;
            string projectedViewName;
            if (projectedViews.top)
            {
                selected = swModel.Extension.SelectByID2(mainViewDrawingName, "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                var topProjectedPointer = (IView)swDraw.CreateUnfoldedViewAt3(Coord.TopX, Coord.TopY, 0, false);
                if (projectedViews.back1)
                {
                    projectedViewName = (string)topProjectedPointer.GetName2();
                    selected = swModel.Extension.SelectByID2(projectedViewName, "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                    var back1ProjectedPointer = (IView)swDraw.CreateUnfoldedViewAt3(Coord.Back1X, Coord.Back1Y, 0, false);
                }
            }
            if (projectedViews.left)
            {
                selected = swModel.Extension.SelectByID2(mainViewDrawingName, "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                var leftProjectedPointer = (IView)swDraw.CreateUnfoldedViewAt3(Coord.LeftX, Coord.LeftY, 0, false);
                if (projectedViews.back2)
                {
                    projectedViewName = (string)leftProjectedPointer.GetName2();
                    selected = swModel.Extension.SelectByID2(projectedViewName, "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                    var back2ProjectedPointer = (IView)swDraw.CreateUnfoldedViewAt3(Coord.Back2X, Coord.Back2Y, 0, false);
                }
            }
            if (projectedViews.right)
            {
                selected = swModel.Extension.SelectByID2(mainViewDrawingName, "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                var rightProjectedPointer = (IView)swDraw.CreateUnfoldedViewAt3(Coord.RightX, Coord.RightY, 0, false);
                if (projectedViews.back3)
                {
                    projectedViewName = (string)rightProjectedPointer.GetName2();
                    selected = swModel.Extension.SelectByID2(projectedViewName, "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                    var back3ProjectedPointer = (IView)swDraw.CreateUnfoldedViewAt3(Coord.Back3X, Coord.Back3Y, 0, false);
                }
            }
            if (projectedViews.bottom)
            {
                selected = swModel.Extension.SelectByID2(mainViewDrawingName, "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                var bottomProjectedPointer = (IView)swDraw.CreateUnfoldedViewAt3(Coord.BottomX, Coord.BottomY, 0, false);
                if (projectedViews.back4)
                {
                    projectedViewName = (string)bottomProjectedPointer.GetName2();
                    selected = swModel.Extension.SelectByID2(projectedViewName, "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                    var back4ProjectedPointer = (IView)swDraw.CreateUnfoldedViewAt3(Coord.Back4X, Coord.Back4Y, 0, false);
                }
            }
            if (projectedViews._3D1)
            {
                selected = swModel.Extension.SelectByID2(mainViewDrawingName, "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                swView = swDraw.CreateUnfoldedViewAt3(Coord.LeftX, Coord.TopY, 0, false);
            }
            if (projectedViews._3D2)
            {
                selected = swModel.Extension.SelectByID2(mainViewDrawingName, "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                swView = swDraw.CreateUnfoldedViewAt3(Coord.RightX, Coord.TopY, 0, false);
            }
            if (projectedViews._3D3)
            {
                selected = swModel.Extension.SelectByID2(mainViewDrawingName, "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                swView = swDraw.CreateUnfoldedViewAt3(Coord.LeftX, Coord.BottomY, 0, false);
            }
            if (projectedViews._3D4)
            {
                selected = swModel.Extension.SelectByID2(mainViewDrawingName, "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                swView = swDraw.CreateUnfoldedViewAt3(Coord.RightX, Coord.BottomY, 0, false);
            }
        }

        public string SetDrawingTemplate(string drawingTemplatePathNotSelected)
        {
            string filter;
            // The Filter string has three filters
            // associated with it; note the use of
            // the | character between filters
            filter = "Drawing template (*.drwdot)|*.drwdot|All Files (*.*)|*.*|";

            var drawingTemplatePath = SwApp.Sw.GetOpenFileName("Select drawing template", "", filter, out var fileOptions, out var fileConfig, out var fileDispName);
            var checkString = drawingTemplatePath.ToUpper();
            if (checkString.Replace(".DRWDOT", "") == drawingTemplatePath)
            {
                if (checkString != "")
                {
                    SwApp.ShowMessageBox("Please select drawing template \".drwdot\"", MessageBoxIcon_e.Info, MessageBoxButtons_e.Ok);
                    // Application.ShowMessageBox("Please select drawing template \".drwdot\"", AngelSix.SolidDna.SolidWorksMessageBoxIcon.Information, AngelSix.SolidDna.SolidWorksMessageBoxButtons.Ok);
                }
                return drawingTemplatePathNotSelected;
            }

            return drawingTemplatePath;

        }
        public void DeleteAllViews(DrawingDoc swDraw)
        {
            IView swView;
            IModelDoc2 swModel;
            ISheet swSheet;
            swModel = (ModelDoc2)swDraw;
            string viewName;
            swModel.ClearSelection2(true);
            swSheet = (ISheet)swDraw.GetCurrentSheet();
            swView = (IView)swDraw.GetFirstView();
            while (swView != null)
            {
                viewName = swView.GetName2();
                _ = swModel.Extension.SelectByID2(viewName, "DRAWINGVIEW", 0, 0, 0, true, 0, null, 0);
                swView = (IView)swView.GetNextView();
            }
            swModel.EditDelete();
        }
    }

    //internal class GetBoundingBox : DrawingFunction
    //{

    //}
}
