namespace OxyPlot.Maui.Skia
{
    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />. This <see cref="IPlotView"/> is based on <see cref="Skia.SkiaRenderContext"/>.
    /// </summary>
    public partial class PlotView : PlotViewBase
    {
        public PlotView()
        {
            this.Controller = new PlotController();
        }

        /// <summary>
        /// Gets the SkiaRenderContext.
        /// </summary>
        private SkiaRenderContext SkiaRenderContext => (SkiaRenderContext)this.renderContext;

        /// <inheritdoc/>
        protected override void ClearBackground()
        {
            var color = this.ActualModel?.Background.IsVisible() == true
                ? this.ActualModel.Background.ToSKColor()
                : SKColors.Empty;

            this.SkiaRenderContext.SkCanvas.Clear(color);
        }

        private SKCanvasView _plotPresenter;

        /// <inheritdoc/>
        protected override View CreatePlotPresenter()
        {
            _plotPresenter = new SKCanvasView();
            _plotPresenter.PaintSurface += this.SkElement_PaintSurface;
            return _plotPresenter;
        }

        /// <inheritdoc/>
        protected override IRenderContext CreateRenderContext()
        {
            return new SkiaRenderContext();
        }

        /// <inheritdoc/>
        protected override void RenderOverride()
        {
            // Instead of rendering directly, invalidate the plot presenter.
            // Actual rendering is done in SkElement_PaintSurface.
            try
            {
                _plotPresenter.InvalidateSurface();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <inheritdoc/>
        protected override double UpdateDpi()
        {
            var scale = base.UpdateDpi();
            this.SkiaRenderContext.DpiScale = (float)scale;
            return scale;
        }

        /// <summary>
        /// This is called when the SKElement paints its surface.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The surface paint event args.</param>
        private void SkElement_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            this.SkiaRenderContext.SkCanvas = e.Surface.Canvas;
            base.RenderOverride();
            this.SkiaRenderContext.SkCanvas = null;
        }
    }
}
