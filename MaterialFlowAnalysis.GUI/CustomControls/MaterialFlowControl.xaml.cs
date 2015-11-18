using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;


namespace MaterialFlowAnalysis.GUI.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для MaterialFlowControl.xaml
    /// </summary>
    public partial class MaterialFlowControl : UserControl
    {
        public MaterialFlowControl()
        {
            InitializeComponent();
        }

        // DEFINING PROPERTIES ==================================================================================================

        public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register(
            "StartPoint", typeof (Point), typeof (MaterialFlowControl), new PropertyMetadata(default(Point)));

        public Point StartPoint
        {
            get { return (Point) GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); UpdateControl(); }
        }



        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(
            "EndPoint", typeof (Point), typeof (MaterialFlowControl), new PropertyMetadata(default(Point)));

        public Point EndPoint
        {
            get { return (Point) GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); UpdateControl(); }
        }



        public static readonly DependencyProperty TextContentProperty = DependencyProperty.Register(
            "TextContent", typeof(string), typeof(MaterialFlowControl), new PropertyMetadata(default(string)));

        public string TextContent
        {
            get { return (string)GetValue(TextContentProperty); }
            set { SetValue(TextContentProperty, value); UpdateTextPosition(); }
        }

        // DEFINED PROPERTIES ===================================================================================================

        public static readonly DependencyProperty ArrowPointsProperty = DependencyProperty.Register(
            "ArrowPoints", typeof (PointCollection), typeof (MaterialFlowControl), new PropertyMetadata(default(PointCollection)));

        public PointCollection ArrowPoints
        {
            get { return (PointCollection) GetValue(ArrowPointsProperty); }
            set { SetValue(ArrowPointsProperty, value); }
        }



        public static readonly DependencyProperty TextPositionProperty = DependencyProperty.Register(
            "TextPosition", typeof (Point), typeof (MaterialFlowControl), new PropertyMetadata(default(Point)));

        public Point TextPosition
        {
            get { return (Point) GetValue(TextPositionProperty); }
            set { SetValue(TextPositionProperty, value); }
        }

        // DEFINITION LOGIC =====================================================================================================

        private double arrowLength = 20;
        private double arrowWidth = 20;

        private void UpdateControl()
        {
            var X1 = StartPoint.X;
            var Y1 = StartPoint.Y;
            var X2 = EndPoint.X;
            var Y2 = EndPoint.Y;

            var o = new Vector(X1, Y1);
            var v = new Vector(X2 - X1, Y2 - Y1);
            var d = v;
            v.Normalize();
            var n = new Vector(v.Y, -v.X);

            var p1 = o + d/2 + v*(arrowLength/2);
            var p2 = o + d/2 - v*(arrowLength/2) + n*(arrowWidth/2);
            var p3 = o + d/2 - v*(arrowLength/2) - n*(arrowWidth/2);

            ArrowPoints = new PointCollection(new[]
            {
                new Point(p1.X, p1.Y),
                new Point(p2.X, p2.Y),
                new Point(p3.X, p3.Y),
            });

            UpdateTextPosition();
        }



        private double tan15 = 0.268;
        private double tan75 = 3.732;

        private void UpdateTextPosition()
        {
            var X1 = StartPoint.X;
            var Y1 = StartPoint.Y;
            var X2 = EndPoint.X;
            var Y2 = EndPoint.Y;

            var tan = (X2 - X1)/(Y2 - Y1);

            TextL.Measure(new Size(200, 200));
            var H = TextL.DesiredSize.Height;
            var W = TextL.DesiredSize.Width;

            if (Y2 == Y1 || (tan < tan15 && tan > -tan15 ))
                TextPosition = new Point(X1 > X2 ? X1 : X2, Y1 + (Y2 - Y1) / 2 - H / 2);
            else if (tan > tan75 || tan < -tan75)
                TextPosition = new Point(X1 + (X2 - X1) / 2 - W / 2, Y1 > Y2 ? Y1 : Y2);
            else if (tan > 0)
                TextPosition = new Point(X1 + (X2 - X1)/2, Y1 + (Y2 - Y1)/2 - H);
            else
                TextPosition = new Point(X1 + (X2 - X1)/2, Y1 + (Y2 - Y1)/2);
        }

    }

}
