using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MaterialFlowAnalysis.GUI.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для QuantificationCenterControl.xaml
    /// </summary>
    public partial class QuantificationCenterControl : UserControl
    {
        public QuantificationCenterControl()
        {
            InitializeComponent();

            Body.Width = Body.Height = 2 * radius;
            DescriptionBody.Width = DescriptionBody.Height = 2 * radius;
        }

        private void QC_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu.DataContext = DataContext;
            ContextMenu.IsOpen = true;
            e.Handled = true;
        }

        private void QC_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(this, DataContext, DragDropEffects.Move);
        }

        

        public Point Position
        {
            get { return (Point)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Point), typeof(QuantificationCenterControl), 
                new FrameworkPropertyMetadata(default(Point), FrameworkPropertyMetadataOptions.AffectsRender, 
                    new PropertyChangedCallback((d, e) => (d as QuantificationCenterControl).UpdateControl())));
        


        public string TopArrowDescription
        {
            get { return (string)GetValue(TopArrowDescriptionProperty); }
            set { SetValue(TopArrowDescriptionProperty, value); }
        }
        
        public static readonly DependencyProperty TopArrowDescriptionProperty =
            DependencyProperty.Register("TopArrowDescription", typeof(string), typeof(QuantificationCenterControl), 
                new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.AffectsRender, UpdateText));



        public string BodyDescription
        {
            get { return (string)GetValue(BodyDescriptionProperty); }
            set { SetValue(BodyDescriptionProperty, value); }
        }

        public static readonly DependencyProperty BodyDescriptionProperty =
            DependencyProperty.Register("BodyDescription", typeof(string), typeof(QuantificationCenterControl),
                new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.AffectsRender, UpdateText));



        public string BottomArrowDescription
        {
            get { return (string)GetValue(BottomArrowDescriptionProperty); }
            set { SetValue(BottomArrowDescriptionProperty, value); }
        }

        public static readonly DependencyProperty BottomArrowDescriptionProperty =
            DependencyProperty.Register("BottomArrowDescription", typeof(string), typeof(QuantificationCenterControl),
                new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.AffectsRender, UpdateText));
        


        public static void UpdateText(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = obj as QuantificationCenterControl;
            control.DescriptionTop.Content = control.TopArrowDescription;
            control.DescriptionBody.Content = control.BodyDescription;
            control.DescriptionBottom.Content = control.BottomArrowDescription;
        }



        private double radius = 32;
        private double arrowAxisLength = 64;
        private double arrowLength = 20;
        private double arrowWidth = 20;

        public void UpdateControl()
        {
            Canvas.SetLeft(Body, Position.X - radius);
            Canvas.SetTop(Body, Position.Y - radius);
            Canvas.SetLeft(DescriptionBody, Position.X - radius);
            Canvas.SetTop(DescriptionBody, Position.Y - radius);

            Axis.X1 = Axis.X2 = Position.X;
            Axis.Y1 = Position.Y - radius - arrowAxisLength;
            Axis.Y2 = Position.Y + radius + arrowAxisLength;

            var y = Position.Y - radius - arrowLength;
            var topArrowPoints = new PointCollection(new[] {
                new Point(Position.X, Position.Y - radius),
                new Point(Position.X - arrowWidth/2, y),
                new Point(Position.X + arrowWidth/2, y),
            });

            Canvas.SetLeft(DescriptionTop, Position.X);
            Canvas.SetTop(DescriptionTop, Position.Y - radius - arrowAxisLength);

            y = Position.Y + radius + arrowAxisLength - arrowLength;
            var bottomArrowPoints = new PointCollection(new[] {
                new Point(Position.X, y + arrowLength),
                new Point(Position.X - arrowWidth/2, y),
                new Point(Position.X + arrowWidth/2, y),
            });

            Canvas.SetLeft(DescriptionBottom, Position.X);
            Canvas.SetTop(DescriptionBottom, Position.Y + radius);

            ArrowTop.Points = topArrowPoints;
            ArrowBottom.Points = bottomArrowPoints;
        }
    }
}
