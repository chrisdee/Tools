using System;
using System.ComponentModel;
using System.Globalization;

namespace WizardBase
{
    internal class WizardCollectionEnumConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            WizardStepCollection steps = (WizardStepCollection) value;
            return steps[steps.Owner.CurrentStepIndex].ToString();
        }
    }
}