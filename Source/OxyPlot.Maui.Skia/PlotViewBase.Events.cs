// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotViewBase.Events.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using OxyPlot.Maui.Skia.Effects;

namespace OxyPlot.Maui.Skia
{
    /// <summary>
    /// Base class for WPF PlotView implementations.
    /// </summary>
    public abstract partial class PlotViewBase
    {
        /// <summary>
        /// The touch points of the previous touch event.
        /// </summary>
        private ScreenPoint[] previousTouchPoints;

        private void AddTouchEffect()
        {
            var touchEffect = new MyTouchEffect();
            touchEffect.TouchAction += TouchEffect_TouchAction;
            if (!InputTransparent)
            {
                this.Effects.Add(touchEffect);
            }
            this.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName is null) return;
                if (args.PropertyName != nameof(InputTransparent)) return;
                if (InputTransparent)
                {
                    if (this.Effects.Contains(touchEffect))
                    {
                        this.Effects.Remove(touchEffect);
                    }
                }
                else
                {
                    if (!this.Effects.Contains(touchEffect))
                    {
                        this.Effects.Add(touchEffect);
                    }
                    
                }
            };
        }

        private void TouchEffect_TouchAction(object sender, TouchActionEventArgs e)
        {
            switch (e.Type)
            {
                case TouchActionType.Pressed:
                    OnTouchDownEvent(e);
                    break;
                case TouchActionType.Moved:
                    OnTouchMoveEvent(e);
                    break;
                case TouchActionType.Released:
                    OnTouchUpEvent(e);
                    break;
                case TouchActionType.MouseWheel:
                    OnMouseWheelEvent(e);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Handles touch down events.
        /// </summary>
        /// <param name="e">The motion event arguments.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        private bool OnTouchDownEvent(TouchActionEventArgs e)
        {
            var args = ToTouchEventArgs(e, Scale);
            var handled = this.ActualController.HandleTouchStarted(this, args);
            this.previousTouchPoints = GetTouchPoints(e, Scale);
            return handled;
        }

        /// <summary>
        /// Handles touch move events.
        /// </summary>
        /// <param name="e">The motion event arguments.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        private bool OnTouchMoveEvent(TouchActionEventArgs e)
        {
            var currentTouchPoints = GetTouchPoints(e, Scale);
            var args = new XamarinOxyTouchEventArgs(currentTouchPoints, this.previousTouchPoints);
            var handled = this.ActualController.HandleTouchDelta(this, args);
            this.previousTouchPoints = currentTouchPoints;
            return handled;
        }

        /// <summary>
        /// Handles touch released events.
        /// </summary>
        /// <param name="e">The motion event arguments.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        private bool OnTouchUpEvent(TouchActionEventArgs e)
        {
            return this.ActualController.HandleTouchCompleted(this, ToTouchEventArgs(e, Scale));
        }

        /// <summary>
        /// Handles Mouse Wheel events.
        /// </summary>
        /// <param name="e">The motion event arguments.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        private bool OnMouseWheelEvent(TouchActionEventArgs e)
        {
            var args = new OxyMouseWheelEventArgs
            {
                Position = new ScreenPoint(e.Location.X / Scale, e.Location.Y / Scale),
                Delta = e.MouseWheelDelta,
                ModifierKeys = e.ModifierKeys
            };
            return this.ActualController.HandleMouseWheel(this, args);
        }

        /// <summary>
        /// Converts an <see cref="TouchActionEventArgs" /> to a <see cref="OxyTouchEventArgs" />.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        /// <param name = "scale">The resolution scale factor.</param>
        /// <returns>The converted event arguments.</returns>
        public static OxyTouchEventArgs ToTouchEventArgs(TouchActionEventArgs e, double scale)
        {
            return new XamarinOxyTouchEventArgs
            {
                Position = new ScreenPoint(e.Location.X / scale, e.Location.Y / scale),
                DeltaTranslation = new ScreenVector(0, 0),
                DeltaScale = new ScreenVector(1, 1),
                PointerCount = e.Locations.Length
            };
        }

        /// <summary>
        /// Gets the touch points from the specified <see cref="TouchActionEventArgs" /> argument.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        /// <param name = "scale">The resolution scale factor.</param>
        /// <returns>The touch points.</returns>
        public static ScreenPoint[] GetTouchPoints(TouchActionEventArgs e, double scale)
        {
            var result = new ScreenPoint[e.Locations.Length];
            for (int i = 0; i < e.Locations.Length; i++)
            {
                result[i] = new ScreenPoint(e.Locations[i].X / scale, e.Locations[i].Y / scale);
            }

            return result;
        }
    }
}
