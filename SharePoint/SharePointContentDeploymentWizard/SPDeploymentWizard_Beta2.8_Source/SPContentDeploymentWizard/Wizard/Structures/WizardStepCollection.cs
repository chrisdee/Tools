using System;
using System.Collections;
using System.Collections.Generic;

namespace WizardBase
{
    public class WizardStepCollection : IList
    {
        #region Private Fields

        protected List<WizardStep> WizardSteps;
        private WizardControl owner;

        #endregion

        #region Events

        public event EventHandler StepAdded;

        public event EventHandler StepRemoved;

        #endregion

        #region Constructor

        internal WizardStepCollection(WizardControl owner)
        {
            this.owner = owner;
            WizardSteps = new List<WizardStep>();
        }

        #endregion

        #region IList Members

        public int Add(object value)
        {
            WizardSteps.Add(value as WizardStep);
            if (Count != 1)
            {
                owner.UpdateButtons();
            }
            else
            {
                owner.OnSetFirstStep();
            }
            OnStepAdded();
            return WizardSteps.Count;
        }

        public void Clear()
        {
            WizardSteps.Clear();
            owner.OnResetWizardSteps();
        }

        public bool Contains(object value)
        {
            return WizardSteps.Contains(value as WizardStep);
        }

        public int IndexOf(object value)
        {
            return IndexOf(value as WizardStep);
        }

        public void Insert(int index, object value)
        {
            WizardSteps.Insert(index, value as WizardStep);
            owner.OnChangeCurrentStepIndex(index, true);
            if (Count != 1)
            {
                owner.UpdateButtons();
            }
            else
            {
                owner.OnSetFirstStep();
            }
            OnStepAdded();
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Remove(object value)
        {
            int i = WizardSteps.IndexOf(value as WizardStep);
            WizardSteps.Remove(value as WizardStep);
            owner.OnChangeCurrentStepIndex(i - 1, true);
            if (Count != 1)
            {
                owner.UpdateButtons();
            }
            else
            {
                owner.OnSetFirstStep();
            }
            OnStepRemoved();
        }

        public void RemoveAt(int index)
        {
            WizardSteps.RemoveAt(index);
            owner.OnChangeCurrentStepIndex(index, true);
            owner.OnResetWizardSteps();
            if (Count != 1)
            {
                owner.UpdateButtons();
            }
            else
            {
                owner.OnSetFirstStep();
            }
            OnStepRemoved();
        }

        public virtual WizardStep this[string key]
        {
            get
            {
                IEnumerator enumerator = GetEnumerator();
                try
                {
                    if (enumerator.MoveNext())
                    {
                        WizardStep current = (WizardStep) enumerator.Current;
                        if (current.Name.Equals(key, StringComparison.CurrentCulture))
                        {
                            WizardStep wizardStep = current;
                            return wizardStep;
                        }
                    }
                    return null;
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        public virtual WizardStep this[int index]
        {
            get { return WizardSteps[index]; }
            set
            {
                WizardSteps[index] = value;
                if (owner.CurrentStepIndex != index)
                {
                    owner.OnChangeCurrentStepIndex(index, true);
                }
            }
        }

        object IList.this[int index]
        {
            get { return WizardSteps[index]; }
            set
            {
                WizardSteps[index] = value as WizardStep;
                if (owner.CurrentStepIndex != index)
                {
                    owner.OnChangeCurrentStepIndex(index, true);
                }
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            WizardStep[] wizs = new WizardStep[Count];
            for (int i = 0; i < Count; i++)
            {
                wizs[i] = WizardSteps[i];
            }
            Array.Copy(wizs, 0, array, index, Count);
        }

        public int Count
        {
            get { return WizardSteps.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return WizardSteps.GetEnumerator();
        }

        #endregion

        #region Virtual Methods

        protected virtual void OnStepAdded()
        {
            if (StepAdded != null)
            {
                StepAdded(this, EventArgs.Empty);
            }
        }

        protected virtual void OnStepRemoved()
        {
            if (StepRemoved != null)
            {
                StepRemoved(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Public Property

        public WizardControl Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        #endregion
    }
}