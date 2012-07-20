using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using COB.SharePoint.Utilities.DeploymentWizard.UI;
using WizardBase.Designers;

namespace WizardBase
{
    [ToolboxItem(false), Designer(typeof (IntermediateStepDesigner)), DefaultEvent("Click")]
    public class IntermediateStep : WizardStep
    {
        #region Private Fields

        private Color headerBackColor = SystemColors.ControlLightLight;
        private Image bindingImage;
        private Image iconImage;
        private string subtitle = "Description for the new step.";
        private Font subtitleFont = new Font("Microsoft Sans", 8.25f, GraphicsUnit.Point);
        private string title = "New WizardControl step.";
        private Font titleFont = new Font("Microsoft Sans", 8.25f, FontStyle.Bold, GraphicsUnit.Point);

        #endregion

        #region Constructor

        public IntermediateStep()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            bindingImage = Resources.Top;
        }

        #endregion

        #region Virtual Methods

        protected virtual void GetTextRects(out RectangleF titleRect, out RectangleF subtitleRect, Graphics graphics)
        {
            StringFormat format = new StringFormat(StringFormatFlags.FitBlackBox);
            format.Trimming = StringTrimming.EllipsisCharacter;
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            format.Trimming = StringTrimming.None;
            SizeF sz = graphics.MeasureString(Title, titleFont, Width, format);
            titleRect = new RectangleF(subtitleFont.SizeInPoints, subtitleFont.SizeInPoints, sz.Width, sz.Height);
            SizeF sz1 = graphics.MeasureString(Subtitle, subtitleFont, Width, format);
            subtitleRect = new RectangleF(2*subtitleFont.SizeInPoints, titleRect.Height + subtitleFont.SizeInPoints, sz1.Width, sz1.Height);
        }

        #endregion

        #region Private Methods

        protected void GetTextRects(out RectangleF titleRect, out RectangleF subtitleRect)
        {
            Graphics graphics = CreateGraphics();
            try
            {
                GetTextRects(out titleRect, out subtitleRect, graphics);
            }
            finally
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                }
            }
        }

        protected Region GetTextRegionToInvalidate()
        {
            RectangleF titleRect;
            RectangleF subtitleRect;
            GetTextRects(out titleRect, out subtitleRect);
            return GetTextRegionToInvalidate(titleRect, subtitleRect);
        }

        protected Region GetTextRegionToInvalidate(RectangleF titleRect, RectangleF subtitleRect)
        {
            if (!titleRect.IsEmpty)
            {
                if (!subtitleRect.IsEmpty)
                {
                    return new Region(new RectangleF(6f, Width - 12, (Width - 0x42), (6f + titleRect.Height) + subtitleRect.Height));
                }
                else
                {
                    return new Region(titleRect);
                }
            }
            else
            {
                if (!subtitleRect.IsEmpty)
                {
                    return new Region(subtitleRect);
                }
                return new Region(RectangleF.Empty);
            }
        }


        private void DrawIcon(Graphics graphics, Rectangle rectangle3)
        {
            IconRect.Inflate(-1, -1);
            if (iconImage == null)
            {
                using (Pen pen = new Pen(SystemColors.ControlDark))
                {
                    pen.DashStyle = DashStyle.Dash;
                    graphics.DrawRectangle(pen, rectangle3);
                }
            }
            else
            {
                graphics.DrawImage(iconImage, rectangle3);
            }
        }

        private void DrawText(RectangleF empty, Graphics graphics, RectangleF layoutRectangle)
        {
            if (!layoutRectangle.IsEmpty)
            {
                graphics.DrawString(title, titleFont, new SolidBrush(ForeColor), layoutRectangle);
            }
            if (!empty.IsEmpty)
            {
                graphics.DrawString(subtitle, subtitleFont, new SolidBrush(ForeColor), empty);
            }
        }

        #endregion

        #region Override

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            Rectangle rect = HeaderRect;
            Rectangle rectangle;
            Rectangle rectangle3 = new Rectangle();
            RectangleF layoutRectangle;
            RectangleF empty;
            GetTextRects(out layoutRectangle, out empty);
            if (bindingImage != null)
            {
                graphics.DrawImage(bindingImage, rect);
                rectangle = new Rectangle(rect.Left, rect.Bottom, rect.Width, 2);
                ControlPaint.DrawBorder3D(graphics, rectangle);
                DrawIcon(graphics, rectangle3);
            }
            else
            {
                using (Brush brush = new SolidBrush(headerBackColor))
                {
                    graphics.FillRectangle(brush, rect);
                    rectangle = new Rectangle(rect.Left, rect.Bottom, rect.Width, 2);
                    ControlPaint.DrawBorder3D(graphics, rectangle);
                    DrawIcon(graphics, rectangle3);
                }
            }
            DrawText(empty, graphics, layoutRectangle);
        }

        internal override void Reset()
        {
            HeaderBackColor = SystemColors.ControlLightLight;
            BindingImage = null;
            Icon = null;
            BackColor = SystemColors.Control;
            BackgroundImage = null;
            BackgroundImageLayout = ImageLayout.Tile;
            ForeColor = SystemColors.ControlText;
            Title = "New Wizard step.";
            Subtitle = "Description for the new step.";
        }

        #endregion

        #region Public Property

        [Description("The title font of step."), Category("Appearance")]
        public Font TitleFont
        {
            get { return titleFont; }
            set { titleFont = value; }
        }

        [Description("The sub title font of step."), Category("Appearance")]
        public Font SubtitleFont
        {
            get { return subtitleFont; }
            set { subtitleFont = value; }
        }
        [Description("The back color of the header."), DefaultValue(typeof (Color), "ControlLightLight"), Category("Appearance")]
        public Color HeaderBackColor
        {
            get { return headerBackColor; }
            set
            {
                if (value != headerBackColor)
                {
                    headerBackColor = value;
                    Invalidate(HeaderRect);
                }
            }
        }

        [Description("The background image of the panel."), DefaultValue((string) null), Category("Appearance")]
        public Image BindingImage
        {
            get { return bindingImage; }
            set
            {
                if (value != null && value != bindingImage)
                {
                    bindingImage = value;
                    Invalidate(HeaderRect);
                }
            }
        }

        protected virtual Rectangle HeaderRect
        {
            get { return new Rectangle(0, 0, Width, 60); }
        }

        [DefaultValue((string) null), Description("The icon image of the step."), Category("Appearance")]
        public Image Icon
        {
            get { return iconImage; }
            set
            {
                if (value != null && value != iconImage)
                {
                    iconImage = value;
                    Invalidate();
                }
            }
        }

        protected virtual Rectangle IconRect
        {
            get { return new Rectangle(Width - 0x36, 6, 0x30, 0x30); }
        }

        [Category("Appearance"), DefaultValue("Description for the new step."), Description("The subtitle of the step."), Editor(typeof (MultilineStringEditor), typeof (UITypeEditor))]
        public string Subtitle
        {
            get { return subtitle; }
            set
            {
                if (!string.IsNullOrEmpty(subtitle) && value != subtitle)
                {
                    Region textRegionToInvalidate = GetTextRegionToInvalidate();
                    subtitle = value;
                    textRegionToInvalidate.Union(GetTextRegionToInvalidate());
                    Invalidate(textRegionToInvalidate);
                }
            }
        }

        [Description("The title of the step."), DefaultValue("New Wizard step."), Editor(typeof (MultilineStringEditor), typeof (UITypeEditor)), Category("Appearance")]
        public string Title
        {
            get { return title; }
            set
            {
                if (!string.IsNullOrEmpty(title) && value != title)
                {
                    Region textRegionToInvalidate = GetTextRegionToInvalidate();
                    title = value;
                    textRegionToInvalidate.Union(GetTextRegionToInvalidate());
                    Invalidate(textRegionToInvalidate);
                }
            }
        }

        #endregion
    }
}