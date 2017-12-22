using ReactNative.UIManager;
#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Automation.Peers;
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Automation.Peers;
#endif

namespace ReactNative.Views.ControlView
{
    /// <summary>
    /// A native control with a single Canvas child.
    /// </summary>
    public class ReactControl : UserControl, IAccessible
    {
        private readonly Canvas _canvas;

        /// <inheritdoc />
        public ReactControl()
        {
            Content = _canvas = new Canvas
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
#if WINDOWS_UWP
            UseSystemFocusVisuals = true;
#endif
        }

        /// <summary>
        /// The view children.
        /// </summary>
        public UIElementCollection Children => _canvas.Children;

        /// <summary>
        /// Keys that should be handled during <see cref="UIElement.KeyDownEvent"/>. 
        /// </summary>
        public int[] HandledKeyDownKeys { get; set; }

        /// <summary>
        /// Keys that should be handled during <see cref="UIElement.KeyUpEvent"/>. 
        /// </summary>
        public int[] HandledKeyUpKeys { get; set; }

        /// <inheritdoc />
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new AccessibleAutomationPeer<ReactControl>(this);
        }

        // TODO: implement runtime change raising event to screen reader #1228350
        /// <inheritdoc />
        public AccessibilityTrait[] AccessibilityTraits { get; set; }

        // TODO: implement runtime change raising event to screen reader #1228350
        /// <inheritdoc />
        public ImportantForAccessibility ImportantForAccessibility { get; set; }
    }
}
