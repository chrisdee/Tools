using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using COB.SharePoint.Utilities.DeploymentWizard.UI;

namespace WizardBase
{
    [Designer(typeof (WizardStepDesigner)), ToolboxItem(false), DefaultEvent("Click")]
    public class StartStep : WizardStep
    {
        private Image iconImage;
        private Color leftPanelBackColor = SystemColors.Desktop;
        private Image bindingImage;
        private string subtitle = "Enter a brief description of the wizard here.";
        private Font subtitleFont = new Font("Microsoft Sans", 8.25f, GraphicsUnit.Point);
        private string title = "Welcome to the DemoWizard.";
        private Font titleFont = new Font("Verdana", 12f, FontStyle.Bold, GraphicsUnit.Point);
        public event EventHandler BindingImageChanged;

        public StartStep()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            BackColor = SystemColors.ControlLightLight;
            Icon = Resources.icon;
            BindingImage = Resources.left;
        }

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

        protected virtual void GetTextRects(out RectangleF titleRect, out RectangleF subtitleRect, Graphics graphics)
        {
            StringFormat format = new StringFormat(StringFormatFlags.FitBlackBox);
            format.Trimming = StringTrimming.EllipsisCharacter;
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            format.Trimming = StringTrimming.None;
            SizeF sz = graphics.MeasureString(Title, titleFont, Width - bindingImage.Width, format);
            titleRect = new RectangleF(bindingImage.Width + subtitleFont.SizeInPoints, subtitleFont.SizeInPoints, sz.Width, sz.Height);
            SizeF sz1 = graphics.MeasureString(Subtitle, subtitleFont, Width - bindingImage.Width, format);
            subtitleRect = new RectangleF(bindingImage.Width + subtitleFont.SizeInPoints, titleRect.Height + subtitleFont.SizeInPoints, sz1.Width, sz1.Height);
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
            if (titleRect.IsEmpty)
            {
                if (!subtitleRect.IsEmpty)
                {
                    return new Region(subtitleRect);
                }
                return new Region(RectangleF.Empty);
            }
            else
            {
                if (!subtitleRect.IsEmpty)
                {
                    return new Region(new RectangleF(172f, 8f, (Width - 180), (8f + titleRect.Height) + subtitleRect.Height));
                }
                return new Region(titleRect);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect;
            Rectangle iconRect;
            RectangleF layoutRectangle;
            RectangleF ef2;
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            rect = LeftRect;
            GetTextRects(out layoutRectangle, out ef2);
            if (bindingImage != null)
            {
                graphics.DrawImage(bindingImage, rect);
                iconRect = IconRect;
                iconRect.Inflate(-1, -1);
                if (iconImage != null)
                {
                    graphics.DrawImage(iconImage, iconRect);
                }
                DrawText(ef2, graphics, layoutRectangle);
            }
            else
            {
                using (Brush brush = new SolidBrush(leftPanelBackColor))
                {
                    graphics.FillRectangle(brush, rect);
                    iconRect = IconRect;
                    iconRect.Inflate(-1, -1);
                    if (iconImage != null)
                    {
                        graphics.DrawImage(iconImage, iconRect);
                    }
                    DrawText(ef2, graphics, layoutRectangle);
                }
            }
        }

        private void DrawText(RectangleF ef2, Graphics graphics, RectangleF layoutRectangle)
        {
            if (!layoutRectangle.IsEmpty)
            {
                graphics.DrawString(title, titleFont, new SolidBrush(ForeColor), layoutRectangle);
            }
            if (!ef2.IsEmpty)
            {
                graphics.DrawString(subtitle, subtitleFont, new SolidBrush(ForeColor), ef2);
            }
        }

        internal override void Reset()
        {
            LeftPanelBackColor = SystemColors.Desktop;
            BindingImage = null;
            Icon = null;
            BackColor = SystemColors.ControlLightLight;
            BackgroundImage = null;
            BackgroundImageLayout = ImageLayout.Tile;
            ForeColor = SystemColors.ControlText;
            Title = "Welcome to the [YourWizardName] Wizard.";
            Subtitle = "Enter a brief description of the wizard here.";
        }

        [DefaultValue(typeof (Color), "ControlLightLight")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

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

        [Description("The icon image of the step."), DefaultValue((string) null), Category("Appearance")]
        public virtual Image Icon
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
            get { return new Rectangle(0x68, 12, 0x30, 0x30); }
        }

        [DefaultValue(typeof (Color), "Desktop"), Description("The back color of the left panel."), Category("Appearance")]
        public virtual Color LeftPanelBackColor
        {
            get { return leftPanelBackColor; }
            set
            {
                if (leftPanelBackColor == value)
                {
                    return;
                }
                leftPanelBackColor = value;
                Invalidate(LeftRect);
            }
        }

        [Category("Appearance"), Description("The background image of the panel."), DefaultValue((string) null)]
        public virtual Image BindingImage
        {
            get { return bindingImage; }
            set
            {
                if (value != null && value != bindingImage)
                {
                    bindingImage = value;
                    OnBindingImageChanged();
                    Invalidate();
                }
            }
        }

        private void OnBindingImageChanged()
        {
            if (BindingImageChanged != null)
            {
                BindingImageChanged(this, EventArgs.Empty);
            }
        }

        protected virtual Rectangle LeftRect
        {
            get { return new Rectangle(0, 0, 0xa4, Height); }
        }

        [Category("Appearance"), DefaultValue("Enter a brief description of the wizard here."), Editor(typeof (MultilineStringEditor), typeof (UITypeEditor)), Description("The subtitle of the step.")]
        public virtual string Subtitle
        {
            get { return subtitle; }
            set
            {
                if (subtitle == value)
                {
                    return;
                }
                Region textRegionToInvalidate = GetTextRegionToInvalidate();
                subtitle = value;
                textRegionToInvalidate.Union(GetTextRegionToInvalidate());
                Invalidate(textRegionToInvalidate);
                return;
            }
        }

        [DefaultValue("Welcome to the [YourWizardName] Wizard."), Description("The title of the step."), Category("Appearance"), Editor(typeof (MultilineStringEditor), typeof (UITypeEditor))]
        public virtual string Title
        {
            get { return title; }
            set
            {
                if (title == value)
                {
                    return;
                }
                Region textRegionToInvalidate = GetTextRegionToInvalidate();
                title = value;
                textRegionToInvalidate.Union(GetTextRegionToInvalidate());
                Invalidate(textRegionToInvalidate);
            }
        }
    }
}