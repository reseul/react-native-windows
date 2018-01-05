namespace ReactNative.UIManager
{
    /// <summary>
    /// Partial accessibility support: accessibilityTraits and importantForAccessibility.
    /// Remaining accessibilityLabel and accessibilityLiveRegion properties are implemented
    /// for all views in a generic way in <see cref="BaseViewManager{TFrameworkElement,TLayoutShadowNode}"/>.
    /// </summary>
    public interface IAccessible
    {
        /// <summary>
        /// accessibilityTraits property.
        /// </summary>
        AccessibilityTrait[] AccessibilityTraits { get; set; }

        /// <summary>
        /// importantForAccessibility property.
        /// </summary>
        ImportantForAccessibility ImportantForAccessibility { get; set; }
    }
}