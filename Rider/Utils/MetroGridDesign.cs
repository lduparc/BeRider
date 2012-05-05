using GalaSoft.MvvmLight;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace Rider.Utils
{
    public class MetroGridDesign : Grid
    {
        protected Brush oldBrush;

        [Category("Metro Guides")]
        [Description("Show/Hides grid Metro guides")]
        public bool MetroVisible
        {
            get { return (bool)GetValue(MetroVisibleProperty); }
            set { SetValue(MetroVisibleProperty, value); }
        }

        [Category("Metro Guides")]
        [Description("Padding")]
        public Thickness MetroPadding
        {
            get { return (Thickness)GetValue(MetroPaddingProperty); }
            set { SetValue(MetroPaddingProperty, value); }
        }

        [Category("Metro Guides")]
        [Description("Horizontal spacing")]
        public int HSpacing
        {
            get { return (int)GetValue(HSpacingProperty); }
            set { SetValue(HSpacingProperty, value); }
        }

        [Category("Metro Guides")]
        [Description("Vertical spacing")]
        public int VSpacing
        {
            get { return (int)GetValue(VSpacingProperty); }
            set { SetValue(VSpacingProperty, value); }
        }

        [Category("Metro Guides")]
        [Description("Rectangle width")]
        public int RectWidth
        {
            get { return (int)GetValue(RectWidthProperty); }
            set { SetValue(RectWidthProperty, value); }
        }

        [Category("Metro Guides")]
        [Description("Rectangle height")]
        public int RectHeight
        {
            get { return (int)GetValue(RectHeightProperty); }
            set { SetValue(RectHeightProperty, value); }
        }

        [Category("Metro Guides")]
        [Description("Rectangle color")]
        public Color RectColor
        {
            get { return (Color)GetValue(RectColorProperty); }
            set { SetValue(RectColorProperty, value); }
        }

        public static readonly DependencyProperty MetroVisibleProperty = DependencyProperty.Register("MetroVisible", typeof(bool), typeof(MetroGridDesign), new PropertyMetadata(false, OnMetroGuidesVisiblePropertyChanged));
        public static readonly DependencyProperty MetroPaddingProperty = DependencyProperty.Register("MetroPadding", typeof(Thickness), typeof(MetroGridDesign), new PropertyMetadata(new Thickness(24, 24, 0, 0), OnMetroGuidesPropertyChanged));

        public static readonly DependencyProperty HSpacingProperty = DependencyProperty.Register("HSpacing", typeof(int), typeof(MetroGridDesign), new PropertyMetadata(12, OnMetroGuidesPropertyChanged));
        public static readonly DependencyProperty VSpacingProperty = DependencyProperty.Register("VSpacing", typeof(int), typeof(MetroGridDesign), new PropertyMetadata(12, OnMetroGuidesPropertyChanged));

        public static readonly DependencyProperty RectWidthProperty = DependencyProperty.Register("RectWidth", typeof(int), typeof(MetroGridDesign), new PropertyMetadata(25, OnMetroGuidesPropertyChanged));
        public static readonly DependencyProperty RectHeightProperty = DependencyProperty.Register("RectHeight", typeof(int), typeof(MetroGridDesign), new PropertyMetadata(25, OnMetroGuidesPropertyChanged));

        public static readonly DependencyProperty RectColorProperty = DependencyProperty.Register("RectColor", typeof(Color), typeof(MetroGridDesign), new PropertyMetadata(Color.FromArgb(255, 255, 0, 0), OnMetroGuidesPropertyChanged));

        public MetroGridDesign()
        {

        }

        private static void OnMetroGuidesVisiblePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            MetroGridDesign grid = sender as MetroGridDesign;

            if (grid.MetroVisible)
                grid.oldBrush = grid.Background;
            else
            {
                grid.Background = grid.oldBrush;
            }

            grid.ShowMetroGuides();
        }

        private static void OnMetroGuidesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            MetroGridDesign grid = sender as MetroGridDesign;
            grid.ShowMetroGuides();
        }

        private void ShowMetroGuides()
        {
            if (DesignerProperties.IsInDesignTool && this.MetroVisible)
                this.Background = CreateMetroBackground();
        }

        private Brush CreateMetroBackground()
        {
            Canvas canvas = new Canvas();
            canvas.Width = this.ActualWidth;
            canvas.Height = this.ActualHeight;

            for (double top = this.MetroPadding.Top; top < canvas.Height; top += this.HSpacing + RectHeight)
                for (double left = this.MetroPadding.Left; left < canvas.Width; left += this.VSpacing + RectWidth)
                {
                    Rectangle rect = new Rectangle();
                    rect.Width = RectWidth;
                    rect.Height = RectHeight;
                    rect.Fill = new SolidColorBrush(RectColor);
                    rect.SetValue(Canvas.LeftProperty, left);
                    rect.SetValue(Canvas.TopProperty, top);
                    rect.IsHitTestVisible = false;

                    canvas.Children.Add(rect);
                }

            var wb = new WriteableBitmap(canvas, null);
            var brush = new ImageBrush();
            brush.ImageSource = wb;

            return brush;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ShowMetroGuides();
        }
    }
}