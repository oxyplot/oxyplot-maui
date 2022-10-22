// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotViewBase.Properties.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Maui.Skia
{
    /// <summary>
    /// Base class for WPF PlotView implementations.
    /// </summary>
    public abstract partial class PlotViewBase
    {
        /// <summary>
        /// Identifies the <see cref="Controller"/> dependency property.
        /// </summary>
        public static readonly BindableProperty ControllerProperty =
            BindableProperty.Create(nameof(Controller), typeof(IPlotController), typeof(PlotViewBase));

        /// <summary>
        /// Identifies the <see cref="DefaultTrackerTemplate"/> dependency property.
        /// </summary>
        public static readonly BindableProperty DefaultTrackerTemplateProperty =
            BindableProperty.Create(
                nameof(DefaultTrackerTemplate), typeof(ControlTemplate), typeof(PlotViewBase));

        /// <summary>
        /// Identifies the <see cref="IsMouseWheelEnabled"/> dependency property.
        /// </summary>
        public static readonly BindableProperty IsMouseWheelEnabledProperty =
            BindableProperty.Create(nameof(IsMouseWheelEnabled), typeof(bool), typeof(PlotViewBase), true);

        /// <summary>
        /// Identifies the <see cref="Model"/> dependency property.
        /// </summary>
        public static readonly BindableProperty ModelProperty =
            BindableProperty.Create(nameof(Model), typeof(PlotModel), typeof(PlotViewBase), null, propertyChanged: ModelChanged);

        /// <summary>
        /// Identifies the <see cref="ZoomRectangleTemplate"/> dependency property.
        /// </summary>
        public static readonly BindableProperty ZoomRectangleTemplateProperty =
            BindableProperty.Create(
                nameof(ZoomRectangleTemplate), typeof(ControlTemplate), typeof(PlotViewBase));

        /// <summary>
        /// Gets or sets the Plot controller.
        /// </summary>
        /// <value>The Plot controller.</value>
        public IPlotController Controller
        {
            get => (IPlotController)this.GetValue(ControllerProperty);
            set => this.SetValue(ControllerProperty, value);
        }

        /// <summary>
        /// Gets or sets the default tracker template.
        /// </summary>
        public ControlTemplate DefaultTrackerTemplate
        {
            get => (ControlTemplate)this.GetValue(DefaultTrackerTemplateProperty);
            set => this.SetValue(DefaultTrackerTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsMouseWheelEnabled.
        /// </summary>
        public bool IsMouseWheelEnabled
        {
            get => (bool)this.GetValue(IsMouseWheelEnabledProperty);
            set => this.SetValue(IsMouseWheelEnabledProperty, value);
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public PlotModel Model
        {
            get => (PlotModel)this.GetValue(ModelProperty);
            set => this.SetValue(ModelProperty, value);
        }

        /// <summary>
        /// Gets or sets the zoom rectangle template.
        /// </summary>
        /// <value>The zoom rectangle template.</value>
        public ControlTemplate ZoomRectangleTemplate
        {
            get => (ControlTemplate)this.GetValue(ZoomRectangleTemplateProperty);
            set => this.SetValue(ZoomRectangleTemplateProperty, value);
        }
    }
}
