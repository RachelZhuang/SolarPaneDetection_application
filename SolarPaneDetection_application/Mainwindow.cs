using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using System;
using ESRI.ArcGIS.SystemUI;
using System.Collections.Generic;
using System.Diagnostics;

namespace SolarPaneDetection_application
{

    public partial class Mainwindow : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private IList<string> RSFilelist = new List<string>();
        private IList<string> UAVFilelist = new List<string>();

        private ITOCControl2 pTocControl;

        private IMapControl3 pMapControl;

        private IToolbarMenu pToolMenuMap;

        private IToolbarMenu pToolMenuLayer;

        private IBasicMap pBasicMap = new MapClass();
        private ILayer pLayer = new FeatureLayerClass();
        object oLegendGroup = new object();
        object oIndex = new object();
        esriTOCControlItem pTocItem = new esriTOCControlItem();

        //设置地理坐标系统
        private string sMapUnits = string.Empty;

        private IGeographicCoordinateSystem gcs = null;

        //设置显示日期的天数（days<13）
        private string[] recent_date = new string[12];
                
        private IPointCollection pPc;

        //private DrawinGraphic mydraw = new DrawinGraphic();
        public Mainwindow()
        {
            InitializeComponent();          
        }

        private void Mainwindow_Load(object sender, System.EventArgs e)
        {
            // 取得 MapControl 和 PageLayoutControl 的引用
            pTocControl = (ITOCControl2)axTOCControl1.Object;
            pMapControl = (IMapControl3)axMapControl1.Object;

            // 创建菜单
            pToolMenuMap = new ToolbarMenuClass();
            pToolMenuLayer = new ToolbarMenuClass();
            pToolMenuLayer.AddItem(new ZoomToLayer(), -1, 0, true, esriCommandStyles.esriCommandStyleTextOnly);
            pToolMenuLayer.AddItem(new RemoveLayer(), -1, 1, true, esriCommandStyles.esriCommandStyleTextOnly);
            pToolMenuLayer.SetHook(pMapControl);

            ISpatialReferenceFactory srFact = new SpatialReferenceEnvironmentClass();///
            gcs = srFact.CreateGeographicCoordinateSystem(4214);
            axMapControl1.MapScale = 4000000;
            axMapControl2.MapScale = 125000000;
            // axMapControl2.
            IDisplayTransformationScales DTS = axMapControl2.ActiveView.ScreenDisplay.DisplayTransformation as IDisplayTransformationScales;
            DTS.RemoveAllUserScales();
            DTS.AddUserScale(125000000);
            DTS.ScaleSnapping = esriScaleSnapping.esriUserScaleSnapping;
        }


        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            // 得到新范围
            IEnvelope pEnvelope = (IEnvelope)e.newEnvelope;

            IGraphicsContainer pGraphicsContainer = axMapControl2.Map as IGraphicsContainer;

            IActiveView pActiveView = pGraphicsContainer as IActiveView;

            //在绘制前,清除axMapControl2中的任何图形元素 
            pGraphicsContainer.DeleteAllElements();

            IRectangleElement pRectangleEle = new RectangleElementClass();
            IElement pElement = pRectangleEle as IElement;
            pElement.Geometry = pEnvelope;

            //设置鹰眼图中的红线框

            IRgbColor pColor = new RgbColorClass();
            pColor.Red = 255;
            pColor.Green = 0;
            pColor.Blue = 0;
            pColor.Transparency = 255;

            //产生一个线符号对象

            ILineSymbol pOutline = new SimpleLineSymbolClass();
            pOutline.Width = 3;
            pOutline.Color = pColor;

            //设置颜色属性

            pColor = new RgbColorClass();
            pColor.Red = 255;
            pColor.Green = 0;
            pColor.Blue = 0;
            pColor.Transparency = 0;

            //设置填充符号的属性

            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = pColor;
            pFillSymbol.Outline = pOutline;
            IFillShapeElement pFillShapeEle = pElement as IFillShapeElement;
            pFillShapeEle.Symbol = pFillSymbol;
            pGraphicsContainer.AddElement((IElement)pFillShapeEle, 0);

            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

        }
        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            if (axMapControl1.LayerCount > 0)
            {
                axMapControl2.Map = new MapClass();
                for (int i = 0; i <= axMapControl1.Map.LayerCount - 1; i++)
                {
                    axMapControl2.AddLayer(axMapControl1.get_Layer(i));
                }
                axMapControl2.Extent = axMapControl1.Extent;
                axMapControl2.Refresh();
            }
        }
      
        private string ToDegreeMiniteSecond(double coordinate)
        {
            string result = "";
            if (coordinate < 0)
            {
                coordinate = Math.Abs(coordinate);//取绝对值
            }
            double pDegree = Math.Truncate(coordinate);
            double sy = coordinate - pDegree;
            double temp = sy * 60;
            double pMinite = Math.Truncate(temp);
            sy = temp - pMinite;
            double pSecond = sy * 60;
            return result = pDegree.ToString() + "°" + pMinite.ToString("00") + "' " + pSecond.ToString("00.0") + " \" ";//坐标显示的格式，度分秒
        }


        private void axMapControl2_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (e.button == 1)
            {
                IPoint pPoint = new PointClass();
                pPoint.PutCoords(e.mapX, e.mapY);
                axMapControl1.CenterAt(pPoint);
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }
        }
        private void axMapControl2_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (axMapControl2.Map.LayerCount > 0)
            {
                if (e.button == 1)
                {
                    IPoint pPoint = new PointClass();
                    pPoint.PutCoords(e.mapX, e.mapY);
                    axMapControl1.CenterAt(pPoint);
                    axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                }
                else if (e.button == 2)
                {
                    IEnvelope pEnv = axMapControl2.TrackRectangle();
                    axMapControl1.Extent = pEnv;
                    axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                }
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog OpenMXD = new OpenFileDialog();
            OpenMXD.Title = "打开地图";
            OpenMXD.InitialDirectory = "E:";
            OpenMXD.Filter = "Map Documents (*.mxd)|*.mxd";
            if (OpenMXD.ShowDialog() == DialogResult.OK)
            {
                string MxdPath = OpenMXD.FileName;
                axMapControl1.LoadMxFile(MxdPath);
            }
        }

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            //获取鼠标点击信息

            axTOCControl1.HitTest(e.x, e.y, ref pTocItem, ref pBasicMap, ref pLayer, ref oLegendGroup, ref oIndex);

            if (e.button == 2)

            {

                if (pTocItem == esriTOCControlItem.esriTOCControlItemMap)

                {

                    pTocControl.SelectItem(pBasicMap, null);

                }

                else

                {

                    pTocControl.SelectItem(pLayer, null);

                }

                //设置CustomProperty为layer (用于自定义的Layer命令)

                pMapControl.CustomProperty = pLayer;

                //弹出右键菜单

                if (pTocItem == esriTOCControlItem.esriTOCControlItemMap)

                {

                    pToolMenuMap.PopupMenu(e.x, e.y, pTocControl.hWnd);

                }

                else

                {

                    pToolMenuLayer.PopupMenu(e.x, e.y, pTocControl.hWnd);

                }

            }
        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (axMapControl1.LayerCount == 0)
            {
                ScaleLabel.Text = "";
                CoordinateLabel.Text = "";

                return;
            }
            else if (this.axMapControl1.LayerCount > 0)
            {

                //显示当前比例尺
                ScaleLabel.Text = "比例尺    1: " + ((long)this.axMapControl1.MapScale).ToString() + "  " + this.axMapControl1.MapUnits.ToString().Substring(4);
                //显示当前坐标
                IPoint position = this.axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
                position.Project(gcs);


                if (!position.IsEmpty)// 当前axmapcontrol上有地图（点）的地方才显示坐标
                {
                    string longtitude = "";
                    string latitude = "";

                    if (position.X < 0)
                    {
                        longtitude = "W  ";
                    }
                    else if (position.X >= 0.0001)
                    {
                        longtitude = "E  ";
                    }
                    if (position.Y < 0)
                    {
                        latitude = "S  ";
                    }
                    else if (position.Y >= 0.0001)
                    {
                        latitude = "N  ";
                    }
                    CoordinateLabel.Text = " 当前坐标    " + ToDegreeMiniteSecond(position.X) + longtitude + "  " +
                        ToDegreeMiniteSecond(position.Y) + latitude + " ";
                }

            }

        }



        private void 卫星_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RSdataQuery f2 = new RSdataQuery();
            f2.clickAddTiff = (str) => addtiffile(str,1);//用lambda表达式实现，这句话必须放在f2.ShowDialog();前面
            f2.ShowDialog();
        }

        private void addtiffile(string tifFilePath,int whichLayer)
        {
            switch (whichLayer)
            {
                case 0:
                    UAVFilelist.Add(tifFilePath);
                    break;
                case 1:
                    RSFilelist.Add(tifFilePath);
                    break;
            }
            IRasterLayer rasterLayer = new RasterLayerClass();
            rasterLayer.CreateFromFilePath(tifFilePath); // fileName指存本地的栅格文件路径
          //  RasterBackgroundDisplay(rasterLayer);
            IGroupLayer groupLayer = axMapControl1.get_Layer(whichLayer) as IGroupLayer;
            groupLayer.Add(rasterLayer);
            axTOCControl1.Update();
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

        }


        private void Mainwindow_ResizeBegin(object sender, EventArgs e)
        {
            //Suppress data redraw and draw bitmap instead.
            axMapControl1.SuppressResizeDrawing(true, 0);
            axMapControl2.SuppressResizeDrawing(true, 0);
        }

        private void Mainwindow_ResizeEnd(object sender, EventArgs e)
        {
            //Stop bitmap draw and draw data.
            axMapControl1.SuppressResizeDrawing(false, 0);
            axMapControl2.SuppressResizeDrawing(false, 0);
        }
        

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DetectImgEdge di = new DetectImgEdge(UAVFilelist);

            di.Show();
        } 

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
               
            DowloadProgressBar pb = new DowloadProgressBar();
            pb.Show();
            
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PreProcess pp = new PreProcess();
            pp.Show();
        }
    }
}
