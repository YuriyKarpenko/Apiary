using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IT;

namespace Apiary.VM
{
	class SliderProperty<T> : NotifyPropertyChangedBase
	{
		public event EventHandler<EventArgs<T>> SelectedChanged;

		protected Func<T, double> convertTo;
		protected Func<double, T> convertFrom;
		protected T max;
		protected T min;
		protected double selected;

		public double MaxValue { get { return this.convertTo(this.Max); } }
		public T Max { get { return this.max; } set { this.SetMax(value); } }

		public double MinValue { get { return this.convertTo(this.Min); } }
		public T Min { get { return this.min; } set { this.SetMin(value); } }

		public double SelectedValue { get { return this.selected; } set { this.SetSelected(this.selected); } }
		public T Selected { get { return this.convertFrom(this.selected); } set { this.SetSelected(this.convertTo(value)); } }


		public SliderProperty(Func<T, double> converterTo, Func<double, T> converterFrom, T minValue = default(T), T maxValue = default(T))
		{
			Contract.NotNull(converterTo, "converter");
			this.convertFrom = converterFrom;
			this.convertTo = converterTo;
			this.Reset(minValue, maxValue);
		}

		public void Reset(T minValue = default(T), T maxValue = default(T))
		{
			this.SetMax(maxValue);
			this.SetMin(minValue);
		}

		protected void SetMax(T value)
		{
			this.max = value;
			this.OnPropertyChanged(nameof(this.Max));
			this.OnPropertyChanged(nameof(this.MaxValue));
		}
		protected void SetMin(T value)
		{
			this.min = value;
			this.OnPropertyChanged(nameof(this.Min));
			this.OnPropertyChanged(nameof(this.MinValue));
		}
		protected void SetSelected(double value)
		{
			this.selected = value;
			this.OnPropertyChanged(nameof(this.Selected));
			this.OnPropertyChanged(nameof(this.SelectedValue));
			this.SelectedChanged?.Invoke(this, new EventArgs<T>(this.convertFrom(value)));
		}
	}
}
