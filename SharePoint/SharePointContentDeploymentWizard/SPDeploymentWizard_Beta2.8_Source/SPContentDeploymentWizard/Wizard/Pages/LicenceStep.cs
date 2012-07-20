using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using COB.SharePoint.Utilities.DeploymentWizard.UI;

namespace WizardBase
{
    public class LicenceStep : WizardStep
    {
        #region Private Fields

        private Color headerBackColor = SystemColors.ControlLightLight;
        private Image bindingImage;
        private Image iconImage;
        private string subtitle = "Please read the following important information before continuing.";
        private Font subtitleFont = new Font("Microsoft Sans", 8.25f, GraphicsUnit.Point);
        private string title = "License Agreement.";
        private Font titleFont = new Font("Microsoft Sans", 8.25f, FontStyle.Bold, GraphicsUnit.Point);
        private string warning = "Please read the following license agreement. You must accept the terms  of this agreement before continuing.";
        private RichTextBox rtbLicense = new RichTextBox();
        private RadioButton rbtnAccept = new RadioButton();
        private RadioButton rbtnDecline = new RadioButton();

        #endregion

        #region Constructor

        public LicenceStep()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            bindingImage = Resources.Top;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            RectangleF titleRect;
            RectangleF subtitleRect;
            RectangleF descRect;
            GetTextRects(out titleRect, out subtitleRect, out descRect);

            rtbLicense.Location = new Point(50, 99);
            rtbLicense.Name = "rtbLicense";
            rtbLicense.Size = new Size(Size.Width - 100, Size.Height -200);
            rtbLicense.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            rtbLicense.TabIndex = 0;
            rtbLicense.Text = "Please select the licence file.";

            rbtnAccept.AutoSize = true;
            rbtnAccept.Location = new Point(50, 314);
            rbtnAccept.Name = "rbtnAccept";
            rbtnAccept.TabIndex = 1;
            rbtnAccept.TabStop = true;
            rbtnAccept.Text = "I &accept the agreement";
            rbtnAccept.UseVisualStyleBackColor = true;

            rbtnDecline.AutoSize = true;
            rbtnDecline.Location = new Point(50, 337);
            rbtnDecline.Name = "rbtnDecline";
            rbtnDecline.TabIndex = 2;
            rbtnDecline.TabStop = true;
            rbtnDecline.Text = "I do &not accept the agreement";
            rbtnDecline.UseVisualStyleBackColor = true;
            Controls.AddRange(new Control[] {rtbLicense, rbtnAccept, rbtnDecline});
            ResumeLayout();
        }

        #endregion

        #region Virtual Methods

        protected virtual void GetTextRects(out RectangleF titleRect, out RectangleF subtitleRect, out RectangleF descriptionRect, Graphics graphics)
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
            SizeF sz2 = graphics.MeasureString(warning, subtitleFont, Width - 2*48, format);
            descriptionRect = new RectangleF(48, HeaderRect.Height + subtitleFont.SizeInPoints, sz2.Width, sz2.Height);
        }

        #endregion

        #region Private Methods

        protected void GetTextRects(out RectangleF titleRect, out RectangleF subtitleRect, out RectangleF descriptionRect)
        {
            Graphics graphics = CreateGraphics();
            try
            {
                GetTextRects(out titleRect, out subtitleRect, out descriptionRect, graphics);
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
            RectangleF descriptionRect;
            GetTextRects(out titleRect, out subtitleRect, out descriptionRect);
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

        private void DrawText(RectangleF empty, Graphics graphics, RectangleF layoutRectangle, RectangleF warningRect)
        {
            if (!layoutRectangle.IsEmpty)
            {
                graphics.DrawString(title, titleFont, new SolidBrush(ForeColor), layoutRectangle);
            }
            if (!empty.IsEmpty)
            {
                graphics.DrawString(subtitle, subtitleFont, new SolidBrush(ForeColor), empty);
            }
            if (!warningRect.IsEmpty)
            {
                graphics.DrawString(warning, subtitleFont, new SolidBrush(ForeColor), warningRect);
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
            RectangleF warningRect;
            GetTextRects(out layoutRectangle, out empty, out warningRect);
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
            DrawText(empty, graphics, layoutRectangle, warningRect);
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

        [Category("Appearance"), DefaultValue("Please read the following important information before continuing."), Description("The subtitle of the step."), Editor(typeof (MultilineStringEditor), typeof (UITypeEditor))]
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

        [Description("License Agreement."), DefaultValue("New Wizard step."), Editor(typeof (MultilineStringEditor), typeof (UITypeEditor)), Category("Appearance")]
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