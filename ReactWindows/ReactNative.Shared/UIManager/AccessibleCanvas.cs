#if WINDOWS_UWP
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Automation.Peers;
#else
using System.Windows.Controls;
using System.Windows.Automation.Peers;
#endif

namespace ReactNative.UIManager
{
    /// <summary>
    /// Canvas with ReactNative accessibility properties support. 
    /// </summary>
    public class AccessibleCanvas : Canvas, IAccessible
    {
        /// <inheritdoc />
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new AccessibleAutomationPeer<AccessibleCanvas>(this);
        }

        // TODO: implement runtime change raising event to screen reader #1562
        /// <inheritdoc />
        public AccessibilityTrait[] AccessibilityTraits { get; set; }

        // TODO: implement runtime change raising event to screen reader #1562
        /// <inheritdoc />
        public ImportantForAccessibility ImportantForAccessibility { get; set; } = ImportantForAccessibility.Auto;
    }
}
