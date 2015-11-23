using MaterialFlowAnalysis.GUI.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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



        private void Control_MouseRightButtonDown(object sender,MouseButtonEventArgs e)
        {
            ContextMenu.DataContext = DataContext;
            ContextMenu.IsOpen = true;
            e.Handled = true;
        }



        public Point Start
        {
            get { return (Point)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }

        public static readonly DependencyProperty StartProperty =
            DependencyProperty.Register("Start", typeof(Point), typeof(MaterialFlowControl),
                new FrameworkPropertyMetadata(default(Point), FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback((d, e) => (d as MaterialFlowControl).UpdateControl())));



        public Point End
        {
            get { return (Point)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }

        public static readonly DependencyProperty EndProperty =
            DependencyProperty.Register("End", typeof(Point), typeof(MaterialFlowControl),
                new FrameworkPropertyMetadata(default(Point), FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback((d, e) => (d as MaterialFlowControl).UpdateControl())));



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MaterialFlowControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback((d, e) => (d as MaterialFlowControl).UpdateTextPosition())));


        
        private double arrowLength = 20;
        private double arrowWidth = 20;

        private void UpdateControl()
        {
            if (Start == null || End == null) return;

            var X1 = Axis.X1 = Start.X;
            var Y1 = Axis.Y1 = Start.Y;
            var X2 = Axis.X2 = End.X;
            var Y2 = Axis.Y2 = End.Y;

            var o = new Vector(X1, Y1);
            var v = new Vector(X2 - X1, Y2 - Y1);
            var d = v;
            v.Normalize();
            var n = new Vector(v.Y, -v.X);

            var p1 = o + d / 2 + v * (arrowLength / 2);
            var p2 = o + d / 2 - v * (arrowLength / 2) + n * (arrowWidth / 2);
            var p3 = o + d / 2 - v * (arrowLength / 2) - n * (arrowWidth / 2);

            Direction.Points = new PointCollection(new[] {
                new Point(p1.X, p1.Y),
                new Point(p2.X, p2.Y),
                new Point(p3.X, p3.Y)});

            UpdateTextPosition();
        }

        private double tan15 = 0.268;
        private double tan75 = 3.732;

        private void UpdateTextPosition()
        {
            var X1 = Axis.X1;
            var Y1 = Axis.Y1;
            var X2 = Axis.X2;
            var Y2 = Axis.Y2;

            Description.Content = Text;
            Description.Measure(new Size(200, 200));
            var H = Description.DesiredSize.Height;
            var W = Description.DesiredSize.Width;
            var tan = (X2 - X1) / (Y2 - Y1);

            Point position;
            if (Y2 == Y1 || (tan < tan15 && tan > -tan15))
                position = new Point(X1 > X2 ? X1 : X2, Y1 + (Y2 - Y1) / 2 - H / 2);
            else if (tan > tan75 || tan < -tan75)
                position = new Point(X1 + (X2 - X1) / 2 - W / 2, Y1 > Y2 ? Y1 : Y2);
            else if (tan > 0)
                position = new Point(X1 + (X2 - X1) / 2, Y1 + (Y2 - Y1) / 2 - H);
            else
                position = new Point(X1 + (X2 - X1) / 2, Y1 + (Y2 - Y1) / 2);
            Canvas.SetLeft(Description, position.X);
            Canvas.SetTop(Description, position.Y);
        }
    }

}
