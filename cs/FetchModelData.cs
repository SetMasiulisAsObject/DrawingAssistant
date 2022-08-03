
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace MMDevelop.DrawingAssistant
{
    internal class FetchModelData
    {

        public int[] FetchModelDataRun(ModelDoc2 swModel)
        {


            if (swModel.GetType() == 1) // 1 - Part model
            {
                var swPart = (PartDoc)swModel;
                var SheetMetalCounter = 0;
                //var WeldmentBodyCounter = 0;
                var OtherBodyCounter = 0;
                var swBodies = (object[])swPart.GetBodies2((int)swBodyType_e.swSolidBody, true); // 0 - "swSolidBody"
                                                                                                 //CustomPropertyManager swProperty;
                foreach (var item in swBodies)
                {
                    var swBody = (IBody2)item;
                    if (swBody.IsSheetMetal())
                    {
                        SheetMetalCounter++;
                    }
                    else
                    {
                        OtherBodyCounter++;
                    }
                }

                int[] data = { 0, SheetMetalCounter, OtherBodyCounter };
                return data;
            }
            else
            {
                int[] data = { 0, 0, 0 };
                return data;
            }
        }
    }

}



