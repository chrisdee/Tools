using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace WizardBase
{
    [DefaultEvent("Click"), Designer(typeof (WizardDesigner))]
    public class WizardControl : Control
    {
        #region Private Fields

        protected internal Button BackButton = new Button();
        private Panel buttonsPanel = new Panel();
        protected internal Button CancelButton = new Button();
        private int currentStepIndex = -1;
        private string finishButtonText;
        protected internal Button HelpButton = new Button();
        protected internal Button NextButton = new Button();
        private string nextButtonText;
        internal int parenthesisCounter;
        private WizardStepCollection wizardStepCollection;
        private Panel wizardStepsPanel = new Panel();

        #endregion

        #region Events

        [Category("WizardControl Buttons Action"), Description("The back button is clicked.")]
        public event WizardClickEventHandler BackButtonClick;

        [Description("The cancel button is clicked."), Category("WizardControl Buttons Action")]
        public event EventHandler CancelButtonClick;

        [Category("Property Changed"), Description("Ocurres after a current step index is changed.")]
        public event EventHandler CurrentStepIndexChanged;

        [Description("The finish button is clicked."), Category("WizardControl Buttons Action")]
        public event EventHandler FinishButtonClick;

        [Category("WizardControl Buttons Action"), Description("The help button is clicked.")]
        public event EventHandler HelpButtonClick;

        [Description("The next button is clicked."), Category("WizardControl Buttons Action")]
        public event WizardNextButtonClickEventHandler NextButtonClick;

        #endregion

        #region Constructor

        public WizardControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            finishButtonText = DefaultFinishButtonText;
            nextButtonText = DefaultNextButtonText;
            InitializeComponent();
            wizardStepCollection = new WizardStepCollection(this);
        }

        #endregion

        #region Private Methods

        private void InitializeComponent()
        {
            SuspendLayout();
            BackButton.Location = new Point(0xd5, 7);
            BackButton.Size = new Size(0x4b, 0x17);
            BackButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            BackButton.Text = DefaultBackButtonText;
            BackButton.Name = "BackButton";
            BackButton.Click += new EventHandler(OnBackButtonClick);
            NextButton.Location = new Point(0x120, 7);
            NextButton.Size = new Size(0x4b, 0x17);
            NextButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            NextButton.Text = DefaultNextButtonText;
            NextButton.Name = "NextButton";
            NextButton.Click += new EventHandler(OnRealNextButtonClick);
            CancelButton.Location = new Point(370, 7);
            CancelButton.Size = new Size(0x4b, 0x17);
            CancelButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            CancelButton.Text = DefaultCancelButtonText;
            CancelButton.Name = "CancelButton";
            CancelButton.Click += new EventHandler(OnCancelButtonClick);
            HelpButton.Location = new Point(0x1c5, 7);
            HelpButton.Size = new Size(0x4b, 0x17);
            HelpButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            HelpButton.Text = DefaultHelpButtonText;
            HelpButton.Name = "HelpButton";
            HelpButton.Click += new EventHandler(OnHelpButtonClick);
            wizardStepsPanel.Size = new Size(0x216, 0x16b);
            wizardStepsPanel.Location = Point.Empty;
            wizardStepsPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            wizardStepsPanel.Name = "WizardStepsPanel";
            wizardStepsPanel.Visible = false;
            buttonsPanel.Size = new Size(0x216, 0x26);
            buttonsPanel.Location = new Point(0, 0x16d);
            buttonsPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            buttonsPanel.Name = "ButtonsPanel";
            buttonsPanel.Visible = false;
            buttonsPanel.Controls.Add(BackButton);
            buttonsPanel.Controls.Add(NextButton);
            buttonsPanel.Controls.Add(CancelButton);
            buttonsPanel.Controls.Add(HelpButton);
            Size = new Size(0x216, 0x193);
            base.Controls.Add(wizardStepsPanel);
            base.Controls.Add(buttonsPanel);
            ResumeLayout();
        }

        private void DoReLayout(int newIndex)
        {
            SuspendLayout();
            if (wizardStepsPanel.Controls.Count > 0)
            {
                wizardStepsPanel.Controls.RemoveAt(0);
            }
            wizardStepsPanel.Controls.Add(WizardSteps[newIndex]);
            currentStepIndex = newIndex;
            if (CurrentStepIndex != 0)
            {
                BackButton.Enabled = true;
            }
            else
            {
                BackButton.Enabled = false;
            }
            if (CurrentStepIndex != (wizardStepCollection.Count - 1))
            {
                NextButton.Text = nextButtonText;
            }
            else
            {
                NextButton.Text = finishButtonText;
            }
            ResumeLayout();
        }

        private void OnRealNextButtonClick(object sender, EventArgs e)
        {
            if (CurrentStepIndex == (WizardSteps.Count - 1))
            {
                OnFinishButtonClick(sender, e);
                return;
            }
            OnNextButtonClick(sender, e);
        }

        private void ResetBackButtonEnabled()
        {
            if (currentStepIndex <= 0)
            {
                BackButton.Enabled = false;
            }
            else
            {
                if (currentStepIndex > 0)
                {
                    BackButton.Enabled = true;
                }
            }
        }

        private void ResetBackButtonVisible()
        {
            BackButtonVisible = true;
        }

        private void ResetCancelButtonEnabled()
        {
            CancelButtonEnabled = true;
        }

        private void ResetCancelButtonVisible()
        {
            CancelButtonVisible = true;
        }

        private void ResetHelpButtonEnabled()
        {
            HelpButtonEnabled = true;
        }

        private void ResetHelpButtonVisible()
        {
            HelpButtonVisible = true;
        }

        private void ResetNextButtonEnabled()
        {
            NextButtonEnabled = true;
        }

        private void ResetNextButtonVisible()
        {
            NextButtonVisible = true;
        }

        internal void UpdateButtons()
        {
            SuspendLayout();
            if (CurrentStepIndex != 0)
            {
                BackButton.Enabled = true;
            }
            else
            {
                BackButton.Enabled = false;
            }
            if (CurrentStepIndex != (wizardStepCollection.Count - 1))
            {
                NextButton.Text = nextButtonText;
            }
            else
            {
                NextButton.Text = finishButtonText;
            }
            ResumeLayout();
        }

        protected void OnChangeCurrentStepIndex(int newIndex)
        {
            OnChangeCurrentStepIndex(newIndex, false);
        }

        #endregion

        #region Virtual Methods

        protected virtual void InvalidateButtonsSeparator()
        {
            Rectangle rectangle = new Rectangle(buttonsPanel.Left, buttonsPanel.Top - 2, buttonsPanel.Width, 2);
            Invalidate(rectangle, false);
        }

        protected virtual void OnBackButtonClick(object sender, EventArgs e)
        {
            if (CurrentStepIndex == 0)
            {
                return;
            }
            if (DesignMode)
            {
                CurrentStepIndex--;
                return;
            }
            if (BackButtonClick == null)
            {
                int backStepIndex = WizardSteps[CurrentStepIndex].BackStepIndex;
                if (backStepIndex != -1)
                {
                    CurrentStepIndex = backStepIndex;
                    return;
                }
                CurrentStepIndex--;
                return;
            }
            else
            {
                WizardClickEventArgs args = new WizardClickEventArgs();
                BackButtonClick(this, args);
                if (args.Cancel)
                {
                    return;
                }
                int num = WizardSteps[CurrentStepIndex].BackStepIndex;
                if (num != -1)
                {
                    CurrentStepIndex = num;
                    return;
                }
                CurrentStepIndex--;
                return;
            }
        }

        protected virtual void OnCancelButtonClick(object sender, EventArgs e)
        {
            if (CancelButtonClick != null)
            {
                CancelButtonClick(sender, e);
            }
        }

        protected internal virtual void OnChangeCurrentStepIndex(int newIndex, bool force)
        {
            if (newIndex < 0 || newIndex >= WizardSteps.Count)
            {
                throw new ArgumentOutOfRangeException("newIndex", "The new index must be a valid index of the WizardSteps collection property.");
            }
            if (CurrentStepIndex != newIndex)
            {
                DoReLayout(newIndex);
                if (CurrentStepIndexChanged != null)
                {
                    CurrentStepIndexChanged(this, EventArgs.Empty);
                }
            }
            else if (force)
            {
                DoReLayout(newIndex);
            }
        }

        protected virtual void OnFinishButtonClick(object sender, EventArgs e)
        {
            if (FinishButtonClick != null)
            {
                FinishButtonClick(sender, e);
            }
        }

        protected virtual void OnHelpButtonClick(object sender, EventArgs e)
        {
            if (HelpButtonClick != null)
            {
                HelpButtonClick(sender, e);
            }
        }

        protected virtual void OnNextButtonClick(object sender, EventArgs e)
        {
            int num;
            if (DesignMode)
            {
                CurrentStepIndex++;
                return;
            }
            else
            {
                num = 0;
                if (!(WizardSteps[CurrentStepIndex] is StartStep))
                {
                    if ((WizardSteps[CurrentStepIndex] is FinishStep))
                    {
                        num = -1;
                    }
                }
                else
                {
                    num = 1;
                }
            }
            if (NextButtonClick == null)
            {
                bool noFinish = false;
                int num2 = 0;
                if (!(WizardSteps[CurrentStepIndex + 1] is StartStep))
                {
                    if (!(WizardSteps[CurrentStepIndex + 1] is FinishStep))
                    {
                        noFinish = true;
                    }
                    else
                    {
                        num2 = -1;
                    }
                }
                else
                {
                    num2 = 1;
                }
                if (((parenthesisCounter + num) + num2) >= 0)
                {
                    if ((((parenthesisCounter + num) + num2) != 0) || !noFinish)
                    {
                        WizardSteps[CurrentStepIndex + 1].BackStepIndex = CurrentStepIndex;
                        CurrentStepIndex++;
                        parenthesisCounter += num;
                    }
                }
                else
                {
                    throw new InvalidOperationException("The steps must be well formed, so there cannot be an IntermediateStep without enclosing.");
                }
            }
            else
            {
                WizardNextButtonClickEventArgs args = new WizardNextButtonClickEventArgs(this);
                NextButtonClick(this, args);
                if (args.Cancel)
                {
                    return;
                }
                if (args.NextStepIndex != -1)
                {
                    WizardSteps[args.NextStepIndex].BackStepIndex = CurrentStepIndex;
                    CurrentStepIndex = args.NextStepIndex;
                    parenthesisCounter += num;
                    return;
                }
                WizardSteps[CurrentStepIndex + 1].BackStepIndex = CurrentStepIndex;
                CurrentStepIndex++;
                parenthesisCounter += num;
                return;
            }
        }

        protected internal virtual void OnResetWizardSteps()
        {
            SuspendLayout();
            if (wizardStepsPanel.Controls.Count > 0)
            {
                wizardStepsPanel.Controls.RemoveAt(0);
            }
            buttonsPanel.Visible = false;
            wizardStepsPanel.Visible = false;
            BackButton.Enabled = true;
            currentStepIndex = -1;
            InvalidateButtonsSeparator();
            ResumeLayout();
            if (CurrentStepIndexChanged != null)
            {
                CurrentStepIndexChanged(this, EventArgs.Empty);
            }
        }

        protected internal virtual void OnSetFirstStep()
        {
            CurrentStepIndex = 0;
            SuspendLayout();
            wizardStepsPanel.Visible = true;
            buttonsPanel.Visible = true;
            InvalidateButtonsSeparator();
            ResumeLayout();
        }

        #endregion

        #region Overrides

        protected override void OnPaint(PaintEventArgs e)
        {
            if (WizardSteps.Count != 0)
            {
                Rectangle rectangle = new Rectangle(buttonsPanel.Left, buttonsPanel.Top - 2, buttonsPanel.Width, 2);
                ControlPaint.DrawBorder3D(e.Graphics, rectangle, Border3DStyle.Etched, Border3DSide.Top);
            }
            base.OnPaint(e);
        }

        protected override void OnTabIndexChanged(EventArgs e)
        {
            base.TabIndex = 0;
        }

        protected override void OnTabStopChanged(EventArgs e)
        {
            base.TabStop = false;
        }

        [Browsable(false)]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [Browsable(false)]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool AllowDrop
        {
            get { return base.AllowDrop; }
            set { base.AllowDrop = true; }
        }

        [Browsable(false)]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = SystemColors.Control; }
        }

        [Browsable(false)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = null; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = ImageLayout.None; }
        }

        protected override Size DefaultSize
        {
            get { return new Size(0x216, 0x193); }
        }

        [Browsable(false)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        [Browsable(false)]
        public override RightToLeft RightToLeft
        {
            get { return base.RightToLeft; }
            set { base.RightToLeft = value; }
        }

        #endregion

        #region Public Property

        [Category("WizardControl Buttons Behavior"), Description("Defines if the back button is enabled or disabled.")]
        public bool BackButtonEnabled
        {
            get { return BackButton.Enabled; }
            set { BackButton.Enabled = value; }
        }

        [Description("Gets or sets the back button text."), Category("WizardControl Buttons Appearance"), DefaultValue("< Back")]
        public string BackButtonText
        {
            get { return BackButton.Text; }
            set { BackButton.Text = value; }
        }

        [Description("Defines the visibility of the back button."), Category("WizardControl Buttons Behavior")]
        public bool BackButtonVisible
        {
            get { return BackButton.Visible; }
            set { BackButton.Visible = value; }
        }

        [Description("Defines if the cancel button is enabled or disabled."), Category("WizardControl Buttons Behavior")]
        public bool CancelButtonEnabled
        {
            get { return CancelButton.Enabled; }
            set { CancelButton.Enabled = value; }
        }

        [Description("Gets or sets the cancel button text."), DefaultValue("Cancel"), Category("WizardControl Buttons Appearance")]
        public string CancelButtonText
        {
            get { return CancelButton.Text; }
            set { CancelButton.Text = value; }
        }

        [Description("Defines the visibility of the cancel button."), Category("WizardControl Buttons Behavior")]
        public bool CancelButtonVisible
        {
            get { return CancelButton.Visible; }
            set { CancelButton.Visible = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual WizardStepCollection Controls
        {
            get { return wizardStepCollection; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Gets or sets the value of the current wizard step index based on the WizardSteps collection property."), DefaultValue(0), Category("Behavior")]
        public int CurrentStepIndex
        {
            get { return currentStepIndex; }
            set { OnChangeCurrentStepIndex(value); }
        }

        [Browsable(false)]
        public string DefaultBackButtonText
        {
            get { return "< Back"; }
        }

        [Browsable(false)]
        public string DefaultCancelButtonText
        {
            get { return "Cancel"; }
        }

        [Browsable(false)]
        public string DefaultFinishButtonText
        {
            get { return "Finish"; }
        }

        [Browsable(false)]
        public string DefaultHelpButtonText
        {
            get { return "Help"; }
        }

        [Browsable(false)]
        public string DefaultNextButtonText
        {
            get { return "Next >"; }
        }


        [Description("Gets or sets the finish button text."), DefaultValue("Finish"), Category("WizardControl Buttons Appearance")]
        public string FinishButtonText
        {
            get { return finishButtonText; }
            set
            {
                finishButtonText = value;
                if (CurrentStepIndex == (wizardStepCollection.Count - 1))
                {
                    NextButton.Text = finishButtonText;
                }
                else
                {
                    NextButton.Text = nextButtonText;
                }
            }
        }


        [Description("Defines if the help button is enabled or disabled."), Category("WizardControl Buttons Behavior")]
        public bool HelpButtonEnabled
        {
            get { return HelpButton.Enabled; }
            set { HelpButton.Enabled = value; }
        }

        [Category("WizardControl Buttons Appearance"), Description("Gets or sets the help button text."), DefaultValue("Help")]
        public string HelpButtonText
        {
            get { return HelpButton.Text; }
            set { HelpButton.Text = value; }
        }

        [Category("WizardControl Buttons Behavior"), Description("Defines the visibility of the help button.")]
        public bool HelpButtonVisible
        {
            get { return HelpButton.Visible; }
            set { HelpButton.Visible = value; }
        }

        [Description("Defines if the next button is enabled or disabled."), Category("WizardControl Buttons Behavior")]
        public bool NextButtonEnabled
        {
            get { return NextButton.Enabled; }
            set { NextButton.Enabled = value; }
        }

        [DefaultValue("Next >"), Category("WizardControl Buttons Appearance"), Description("Gets or sets the next button text.")]
        public string NextButtonText
        {
            get { return nextButtonText; }
            set
            {
                nextButtonText = value;
                if (CurrentStepIndex != (wizardStepCollection.Count - 1))
                {
                    NextButton.Text = nextButtonText;
                    return;
                }
                NextButton.Text = finishButtonText;
            }
        }

        [Category("WizardControl Buttons Behavior"), Description("Defines the visibility of the next button.")]
        public bool NextButtonVisible
        {
            get { return NextButton.Visible; }
            set { NextButton.Visible = value; }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int TabIndex
        {
            get { return base.TabIndex; }
            set { base.TabIndex = 0; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = false; }
        }

        [Editor(typeof (WizardStepCollectionEditor), typeof (UITypeEditor)), Description("Gets a collection containing the step. This property returns the same collection than the Controls property."), TypeConverter(typeof (WizardStepCollectionConverter)), Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual WizardStepCollection WizardSteps
        {
            get { return wizardStepCollection; }
        }

        #endregion
    }
}