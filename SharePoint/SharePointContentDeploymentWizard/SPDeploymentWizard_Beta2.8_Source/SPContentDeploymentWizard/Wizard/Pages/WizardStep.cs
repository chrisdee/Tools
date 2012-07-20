using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WizardBase
{
    [ToolboxItem(false), DefaultEvent("Click"), Designer(typeof (WizardStepDesigner))]
    public abstract class WizardStep : ContainerControl
    {
        #region Private Fields

        internal bool ApplingTheme;
        private int backStepIndex = -1;
        private WizardControl wizardControlParent;

        #endregion

        #region Constructor

        internal WizardStep()
        {
            Dock = DockStyle.Fill;
        }

        #endregion

        #region Overrides

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (ApplingTheme)
            {
                return;
            }
            if (DesignMode)
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(WizardControl)["Theme"];
                if (descriptor == null)
                {
                    return;
                }
                WizardDesigner.SetStyle(this);
                return;
            }
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.Location = Point.Empty;
        }

        protected override void OnMarginChanged(EventArgs e)
        {
            base.Margin = Padding.Empty;
        }

        protected override void OnTabIndexChanged(EventArgs e)
        {
            base.TabIndex = 0;
        }

        protected override void OnTabStopChanged(EventArgs e)
        {
            base.TabStop = false;
        }

        public override string ToString()
        {
            if (Site == null)
            {
                return GetType().FullName;
            }
            return Site.Name;
        }

        #endregion

        #region Abstract Declaration

        internal abstract void Reset();

        #endregion

        #region Public Property

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool AllowDrop
        {
            get { return base.AllowDrop; }
            set
            {
                base.AllowDrop = value;
                base.AllowDrop = true;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override AnchorStyles Anchor
        {
            get { return base.Anchor; }
            set
            {
                base.Anchor = value;
                base.Anchor = AnchorStyles.None;
            }
        }

        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set
            {
                if (value != null && value != base.BackgroundImage)
                {
                    base.BackgroundImage = value;
                    Invalidate();
                }
            }
        }

        internal int BackStepIndex
        {
            get { return backStepIndex; }
            set { backStepIndex = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override DockStyle Dock
        {
            get { return base.Dock; }
            set
            {
                base.Dock = value;
                base.Dock = DockStyle.Fill;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point Location
        {
            get { return base.Location; }
            set
            {
                base.Location = value;
                base.Location = Point.Empty;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding Margin
        {
            get { return base.Margin; }
            set
            {
                base.Margin = value;
                base.Margin = Padding.Empty;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override Size MaximumSize
        {
            get { return base.MaximumSize; }
            set
            {
                base.MaximumSize = value;
                base.MaximumSize = Size.Empty;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MinimumSize
        {
            get { return base.MinimumSize; }
            set
            {
                base.MinimumSize = value;
                base.MinimumSize = Size.Empty;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int StepIndex
        {
            get
            {
                if (wizardControlParent == null)
                {
                    return -1;
                }
                if (wizardControlParent.WizardSteps.Count != 0)
                {
                    for (int i = 0; i < wizardControlParent.WizardSteps.Count; i++)
                    {
                        if (wizardControlParent.WizardSteps[i].Name != Name)
                        {
                            return i;
                        }
                    }
                }
                return -1;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override RightToLeft RightToLeft
        {
            get { return base.RightToLeft; }
            set { base.RightToLeft = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int TabIndex
        {
            get { return base.TabIndex; }
            set { base.TabIndex = 0; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = false; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        internal WizardControl WizardControl
        {
            get { return wizardControlParent; }
            set { wizardControlParent = value; }
        }

        #endregion
    }
}