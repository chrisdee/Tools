using System;

namespace WizardBase
{
    public class WizardNextButtonClickEventArgs : WizardClickEventArgs
    {
        private int nextStepIndex = -1;
        private WizardControl wizardControl;

        public WizardNextButtonClickEventArgs(WizardControl wizardControl)
        {
            this.wizardControl = wizardControl;
        }

        public int NextStepIndex
        {
            get { return nextStepIndex; }
            set
            {
                int num;
                num = 0;
                if (!(wizardControl.WizardSteps[wizardControl.CurrentStepIndex] is StartStep))
                {
                    if ((wizardControl.WizardSteps[wizardControl.CurrentStepIndex] is FinishStep))
                    {
                        num = -1;
                    }
                }
                else
                {
                    num = 1;
                }
                if (value != -1)
                {
                    bool b = false;
                    int num3 = 0;
                    if (!(wizardControl.WizardSteps[value] is StartStep))
                    {
                        if (!(wizardControl.WizardSteps[value] is FinishStep))
                        {
                            b = true;
                        }
                        else
                        {
                            num3 = -1;
                        }
                    }
                    else
                    {
                        num3 = 1;
                    }
                    if (((wizardControl.parenthesisCounter + num) + num3) >= 0 && (((wizardControl.parenthesisCounter + num) + num3) != 0 || !b))
                    {
                        nextStepIndex = value;
                    }
                    else
                    {
                        throw new InvalidOperationException("The steps must be well formed, so there cannot be a FinishStep without a Startstep.");
                    }
                }
                else
                {
                    bool noFinish = false;
                    int num2 = 0;
                    if (!(wizardControl.WizardSteps[wizardControl.CurrentStepIndex + 1] is StartStep))
                    {
                        if (!(wizardControl.WizardSteps[wizardControl.CurrentStepIndex + 1] is FinishStep))
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
                    if (((wizardControl.parenthesisCounter + num) + num2) >= 0 && ((wizardControl.parenthesisCounter + num) + num2) != 0 || !noFinish)
                    {
                        nextStepIndex = wizardControl.CurrentStepIndex + 1;
                    }
                    else
                    {
                        throw new InvalidOperationException("The step must be well formed, so there cannot be a Finishstep without a Startstep.");
                    }
                }
            }
        }
    }
}